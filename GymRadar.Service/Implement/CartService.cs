using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Enum;
using GymRadar.Model.Payload.Request.Cart;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Cart;
using GymRadar.Model.Payload.Response.PayOS;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GymRadar.Service.Implement
{
    public class CartService : BaseService<CartService>, ICartService
    {
        private readonly PayOS _payOS;
        private readonly PayOSSettings _payOSSettings;
        private readonly HttpClient _client;
        public CartService(IUnitOfWork<GymRadarContext> unitOfWork
            , ILogger<CartService> logger
            , IMapper mapper
            , IHttpContextAccessor httpContextAccessor
            , PayOS payOS, IOptions<PayOSSettings> settings, HttpClient client) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _payOSSettings = settings.Value;
            _payOS = payOS;
            _client = client;
        }

        public async Task<BaseResponse<CreatePaymentResult>> CreatePaymentUrlRegisterCreator(CreateQRRequest request)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(id) && u.Active == true);

            if (account == null)
            {
                return new BaseResponse<CreatePaymentResult>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                predicate: u => u.AccountId.Equals(id) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<CreatePaymentResult>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin người dùng",
                    data = null
                };
            }

            var gymCourse = await _unitOfWork.GetRepository<GymCourse>().SingleOrDefaultAsync(
                predicate: gc => gc.Id.Equals(request.GymCourseId) && gc.Active == true,
                include: gc => gc.Include(gc => gc.Gym));

            if (gymCourse == null)
            {
                return new BaseResponse<CreatePaymentResult>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tồn tại khóa học này",
                    data = null
                };
            }

            var pt = await _unitOfWork.GetRepository<Pt>().SingleOrDefaultAsync(
                predicate: pt => pt.Id.Equals(request.PTId) && pt.Active == true);
            if (pt == null)
            {
                return new BaseResponse<CreatePaymentResult>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "PT này không tồn tại",
                    data = null
                };
            }

            if (pt.GymId != gymCourse.GymId)
            {
                return new BaseResponse<CreatePaymentResult>
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "PT này không thuộc phòng gym này",
                    data = null
                };
            }

            var gymCoursePt = await _unitOfWork.GetRepository<GymCoursePt>().SingleOrDefaultAsync(
                predicate: gcp => gcp.Ptid.Equals(request.PTId) && gcp.GymCourseId.Equals(request.GymCourseId) && gcp.Active == true);

            if (gymCoursePt == null)
            {
                return new BaseResponse<CreatePaymentResult>
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "PT này không được liên kết với khóa học này",
                    data = null
                };
            }


            string buyerName = user.FullName;
            string buyerPhone = account.Phone;
            string buyerEmail = account.Email;

            Random random = new Random();
            long orderCode = (DateTime.Now.Ticks % 1000000000000000L) * 10 + random.Next(0, 1000);
            var description = "GymRadarQR";
            var signatureData = new Dictionary<string, object>
                {
                    { "amount", gymCourse.Price },
                    { "cancelUrl", _payOSSettings.ReturnUrlFail },
                    { "description", description },
                    { "expiredAt", DateTimeOffset.Now.AddMinutes(10).ToUnixTimeSeconds() },
                    { "orderCode", orderCode },
                    { "returnUrl", _payOSSettings.ReturnUrl }
                };

            var sortedSignatureData = new SortedDictionary<string, object>(signatureData);
            var dataForSignature = string.Join("&", sortedSignatureData.Select(p => $"{p.Key}={p.Value}"));
            var signature = ComputeHmacSha256(dataForSignature, _payOSSettings.ChecksumKey);

            DateTimeOffset expiredAt = DateTimeOffset.Now.AddMinutes(10);

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)gymCourse.Price,
                description: description,
                items: null,
                cancelUrl: _payOSSettings.ReturnUrlFail,
                returnUrl: _payOSSettings.ReturnUrl,
                signature: signature,
                buyerName: buyerName,
                buyerPhone: buyerPhone,
                buyerEmail: buyerEmail,

                buyerAddress: "HCM",
                expiredAt: (int)expiredAt.ToUnixTimeSeconds()
            );

            var paymentResult = await _payOS.createPaymentLink(paymentData);

            var transaction = new Model.Entity.Transaction
            {
                Id = Guid.NewGuid(),
                Status = StatusTransactionEnum.PENDING.GetDescriptionFromEnum(),
                Price = gymCourse.Price,
                OrderCode = orderCode,
                Description = description,
                UserId = user.Id,
                GymCourseId = request.GymCourseId,
                PtId = request.PTId,
                CreateAt = TimeUtil.GetCurrentSEATime(),
            };
            await _unitOfWork.GetRepository<Model.Entity.Transaction>().InsertAsync(transaction);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<CreatePaymentResult>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Tạo link thành công",
                data = paymentResult,
            };
        }

        public async Task<BaseResponse<TransactionResponse>> GetPaymentStatus(long orderCode)
        {
            try
            {
                var transaction = await _unitOfWork.GetRepository<Model.Entity.Transaction>()
                    .SingleOrDefaultAsync(predicate: t => t.OrderCode == orderCode);

                if (transaction == null)
                {
                    return new BaseResponse<TransactionResponse>
                    {
                        status = StatusCodes.Status404NotFound.ToString(),
                        message = "Không tìm thấy giao dịch",
                        data = null
                    };
                }
                var transactionResponse = new TransactionResponse
                {
                    Amount = transaction.Price,
                    Description = transaction.Description,
                    OrderCode = transaction.OrderCode,
                    Status = "00"
                };

                if (transaction.Status == StatusTransactionEnum.COMPLETED.GetDescriptionFromEnum())
                {
                    return new BaseResponse<TransactionResponse>
                    {
                        status = StatusCodes.Status200OK.ToString(),
                        message = "Giao dịch đã hoàn tất thành công",
                        data = transactionResponse
                    };
                }
                else if (transaction.Status == StatusTransactionEnum.CANCELLED.GetDescriptionFromEnum())
                {
                    transactionResponse.Status = "01";
                    return new BaseResponse<TransactionResponse>
                    {
                        status = StatusCodes.Status400BadRequest.ToString(),
                        message = "Giao dịch đã bị hủy",
                        data = transactionResponse
                    };
                }
                else
                {
                    return new BaseResponse<TransactionResponse>
                    {
                        status = StatusCodes.Status400BadRequest.ToString(),
                        message = "Giao dịch chưa được hoàn tất",
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra trạng thái giao dịch");
                return new BaseResponse<TransactionResponse>
                {
                    status = StatusCodes.Status500InternalServerError.ToString(),
                    message = "Lỗi hệ thống khi kiểm tra trạng thái giao dịch",
                    data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> HandlePaymentCallback(string paymentLinkId, long orderCode)
        {
            try
            {
                //if (!long.TryParse(paymentLinkId, out long parsedPaymentLinkId))
                //{
                //    return new BaseResponse<bool>
                //    {
                //        status = StatusCodes.Status400BadRequest.ToString(),
                //        message = "Định dạng paymentLinkId không hợp lệ",
                //        data = false
                //    };
                //}
                var paymentInfo = await GetPaymentInfo(paymentLinkId);

                var transaction = await _unitOfWork.GetRepository<Model.Entity.Transaction>()
                    .SingleOrDefaultAsync(predicate: t => t.OrderCode == orderCode);

                if (transaction == null)
                {
                    return new BaseResponse<bool>
                    {
                        status = StatusCodes.Status404NotFound.ToString(),
                        message = "Không tìm thấy giao dịch",
                        data = false
                    };
                }

                if (paymentInfo.Status == "PAID")
                {
                    transaction.Status = StatusTransactionEnum.COMPLETED.GetDescriptionFromEnum();
                    _unitOfWork.GetRepository<Model.Entity.Transaction>().UpdateAsync(transaction);
                    await _unitOfWork.CommitAsync();

                    return new BaseResponse<bool>
                    {
                        status = StatusCodes.Status200OK.ToString(),
                        message = "Xử lý giao dịch thành công",
                        data = true
                    };
                }
                else if (paymentInfo.Status == "CANCELLED")
                {
                    transaction.Status = StatusTransactionEnum.CANCELLED.GetDescriptionFromEnum();
                    _unitOfWork.GetRepository<Model.Entity.Transaction>().UpdateAsync(transaction);
                    await _unitOfWork.CommitAsync();

                    return new BaseResponse<bool>
                    {
                        status = StatusCodes.Status400BadRequest.ToString(),
                        message = "Giao dịch đã bị hủy",
                        data = false
                    };
                }
                else
                {
                    return new BaseResponse<bool>
                    {
                        status = StatusCodes.Status400BadRequest.ToString(),
                        message = "Giao dịch chưa được hoàn tất",
                        data = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý callback thanh toán");
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status500InternalServerError.ToString(),
                    message = "Lỗi hệ thống khi xử lý giao dịch",
                    data = false
                };
            }
        }

        private string? ComputeHmacSha256(string data, string checksumKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task<ExtendedPaymentInfo> GetPaymentInfo(string paymentLinkId)
        {
            try
            {
                var getUrl = $"https://api-merchant.payos.vn/v2/payment-requests/{paymentLinkId}";

                var request = new HttpRequestMessage(HttpMethod.Get, getUrl);
                request.Headers.Add("x-client-id", _payOSSettings.ClientId);
                request.Headers.Add("x-api-key", _payOSSettings.ApiKey);

                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                var paymentInfo = responseObject["data"].ToObject<ExtendedPaymentInfo>();

                return paymentInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting payment info.", ex);
            }
            return new ExtendedPaymentInfo();
        }
    }
}

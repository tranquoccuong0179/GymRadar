using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Request.Cart;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.PayOS;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace GymRadar.Service.Implement
{
    public class CartService : BaseService<CartService>, ICartService
    {
        private readonly PayOS _payOS;
        private readonly PayOSSettings _payOSSettings;
        public CartService(IUnitOfWork<GymRadarContext> unitOfWork
            , ILogger<CartService> logger
            , IMapper mapper
            , IHttpContextAccessor httpContextAccessor
            , PayOS payOS, IOptions<PayOSSettings> settings) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _payOSSettings = settings.Value;
            _payOS = payOS;
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
                predicate: gc => gc.Id.Equals(request.GymCourseId) && gc.Active == true);

            if(gymCourse == null)
            {
                return new BaseResponse<CreatePaymentResult>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy khóa tập",
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

            return new BaseResponse<CreatePaymentResult>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Tạo link thành công",
                data = paymentResult,
            };
        }

        private string? ComputeHmacSha256(string data, string checksumKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        public AccountService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<RegisterResponse>> RegisterAccount(RegisterRequest request)
        {
            var isEmailExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: u => u.Email.Equals(request.Email));
            if (isEmailExist != null)
            {
                return new BaseResponse<RegisterResponse>()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Email đã tồn tại",
                    data = null
                };
            }

            var isPhoneExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Phone.Equals(request.Phone));
            if (isPhoneExist != null)
            {
                return new BaseResponse<RegisterResponse>()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Số điện thoại đã tồn tại",
                    data = null
                };
            }

            var account = _mapper.Map<Account>(request);
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Dob = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                Age = 18,
                Weight = 0.0,
                Height = 0.0,
                Gender = "Unknown",
                Address = "Unknown",
                AccountId = account.Id,
                Active = false,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
                DeleteAt = null
            };
            await _unitOfWork.GetRepository<User>().InsertAsync(user);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse<RegisterResponse>()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Đăng kí tài khoản thành công",
                    data = _mapper.Map<RegisterResponse>(account)
                };
            }

            return new BaseResponse<RegisterResponse>()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Đăng kí tài khoản thất bại",
                data = null 
            };
        }

        public async Task<BaseResponse<RegisterResponse>> RegisterAdmin(RegisterAdminRequest request)
        {
            var account = _mapper.Map<Account>(request);
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);

            var admin = new Admin
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                AccountId = account.Id,
                Active = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };
            await _unitOfWork.GetRepository<Admin>().InsertAsync(admin);

            await _unitOfWork.CommitAsync();

            return new BaseResponse<RegisterResponse>()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Đăng kí tài khoản thành công",
                data = _mapper.Map<RegisterResponse>(account)
            };
        }
    }
}

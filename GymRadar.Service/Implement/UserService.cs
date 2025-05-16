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
    public class UserService : BaseService<UserService>, IUserService
    {
        public UserService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<GetUserResponse>> UpdateUser(UpdateUserRequest request)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(userId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<GetUserResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                predicate: u => u.AccountId.Equals(userId) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<GetUserResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người dùng",
                    data = null
                };
            }

            user.FullName = string.IsNullOrEmpty(request.FullName) ? user.FullName : request.FullName;
            user.Dob = request.Dob.HasValue ? request.Dob.Value : user.Dob;
            user.Weight = request.Weight.HasValue ? request.Weight.Value : user.Weight;
            user.Height = request.Height.HasValue ? request.Height.Value : user.Height;
            user.Gender = request.Gender.HasValue ? request.Gender.GetDescriptionFromEnum() : user.Gender;
            user.Address = string.IsNullOrEmpty(request.Address) ? user.Address : request.Address;

            return new BaseResponse<GetUserResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Cập nhật thành công",
                data = _mapper.Map<GetUserResponse>(user)
            };
        }
    }
}

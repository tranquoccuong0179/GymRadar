using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Paginate;
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

        public async Task<BaseResponse<bool>> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(id) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người dùng",
                    data = false,
                };
            }

            user.Active = false;
            user.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<User>().UpdateAsync(user);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(user.AccountId) && a.Active == true);
            if (account == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = false,
                };
            }

            account.Active = false;
            account.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Account>().UpdateAsync(account);

            await _unitOfWork.CommitAsync();

            return new BaseResponse<bool>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Xóa người dùng thành công",
                data = true
            };
        }

        public async Task<BaseResponse<IPaginate<GetUserResponse>>> GetAllUser(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<User>().GetPagingListAsync(
                selector: u => _mapper.Map<GetUserResponse>(u),
                predicate: u => u.Active == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetUserResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách người dùng thành công",
                data = response
            };
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

            _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            return new BaseResponse<GetUserResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Cập nhật thành công",
                data = _mapper.Map<GetUserResponse>(user)
            };
        }
    }
}

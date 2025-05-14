using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.GymCourse;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.GymCourse;
using GymRadar.Model.Payload.Response.PT;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class GymCourseService : BaseService<GymCourseService>, IGymCourseService
    {
        public GymCourseService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<GymCourseService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateGymCourseResponse>> CreateNewGymCourse(CreateGymCourseRequest request)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(userId) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<CreateGymCourseResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(user.Id) && g.Active == true);

            if (gym == null)
            {
                return new BaseResponse<CreateGymCourseResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không có phòng gym",
                    data = null
                };
            }

            var gymCourse = _mapper.Map<GymCourse>(request);
            gymCourse.GymId = gym.Id;
            await _unitOfWork.GetRepository<GymCourse>().InsertAsync(gymCourse);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessful)
            {
                return new BaseResponse<CreateGymCourseResponse>
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo khóa gym thành công",
                    data = _mapper.Map<CreateGymCourseResponse>(gymCourse)
                };
            }

            return new BaseResponse<CreateGymCourseResponse>
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo khóa gym thất bại",
                data = null
            };
        }

        public async Task<BaseResponse<IPaginate<GetGymCourseResponse>>> GetAllGymCourse(int page, int size)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(userId) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<IPaginate<GetGymCourseResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(user.Id) && g.Active == true);

            if (gym == null)
            {
                return new BaseResponse<IPaginate<GetGymCourseResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không có phòng gym",
                    data = null
                };
            }

            var gymCourses = await _unitOfWork.GetRepository<GymCourse>().GetPagingListAsync(
                selector: g => _mapper.Map<GetGymCourseResponse>(g),
                predicate: g => g.GymId.Equals(gym.Id) && g.Active == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetGymCourseResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách khóa tập thành công",
                data = gymCourses
            };
        }

        public async Task<BaseResponse<IPaginate<GetGymCourseResponse>>> GetAllGymCourseForUser(Guid id, int page, int size)
        {
            var gymCourses = await _unitOfWork.GetRepository<GymCourse>().GetPagingListAsync(
                selector: g => _mapper.Map<GetGymCourseResponse>(g),
                predicate: g => g.GymId.Equals(id) && g.Active == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetGymCourseResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách khóa tập thành công",
                data = gymCourses
            };
        }
    }
}

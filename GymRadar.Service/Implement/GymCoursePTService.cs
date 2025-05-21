using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.GymCoursePT;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.GymCoursePT;
using GymRadar.Model.Payload.Response.PTSlot;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class GymCoursePTService : BaseService<GymCoursePTService>, IGymCoursePTService
    {
        public GymCoursePTService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<GymCoursePTService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateGymCoursePTResponse>> CreateGymCoursePT(CreateGymCoursePTRequest request)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(userId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<CreateGymCoursePTResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = null
                };
            }

            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(userId) && g.Active == true);

            if (gym == null)
            {
                return new BaseResponse<CreateGymCoursePTResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin phòng gym",
                    data = null
                };
            }

            var gymCourse = await _unitOfWork.GetRepository<GymCourse>().SingleOrDefaultAsync(
                predicate: gc => gc.Id.Equals(request.GymCourseId) && gc.Active == true && gc.GymId.Equals(gym.Id));

            if (gymCourse == null)
            {
                return new BaseResponse<CreateGymCoursePTResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin khóa tập của phòng gym",
                    data = null
                };
            }

            var pt = await _unitOfWork.GetRepository<Pt>().SingleOrDefaultAsync(
                predicate: pt => pt.GymId.Equals(gym.Id) && pt.Active == true && pt.Id.Equals(request.Ptid));

            if (pt == null)
            {
                return new BaseResponse<CreateGymCoursePTResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin PT của phòng gym",
                    data = null
                };
            }

            var gymCoursePt = _mapper.Map<GymCoursePt>(request);

            await _unitOfWork.GetRepository<GymCoursePt>().InsertAsync(gymCoursePt);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<CreateGymCoursePTResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Thêm PT vào khóa tập thành công",
                data = _mapper.Map<CreateGymCoursePTResponse>(gymCoursePt)
            };
        }

        public async Task<BaseResponse<IPaginate<GetGymCoursePTResponse>>> GetAllGymCoursePT(Guid id, int page, int size)
        {
            var gymCourse = await _unitOfWork.GetRepository<GymCourse>().SingleOrDefaultAsync(
                predicate: g => g.Id.Equals(id) && g.Active == true);

            if (gymCourse == null)
            {
                return new BaseResponse<IPaginate<GetGymCoursePTResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin khóa học",
                    data = null
                };
            }

            var gymCoursePt = await _unitOfWork.GetRepository<GymCoursePt>().GetPagingListAsync(
                selector: gc => _mapper.Map<GetGymCoursePTResponse>(gc),
                predicate: gc => gc.GymCourseId.Equals(gymCourse.Id) && gc.Active == true,
                include: gc => gc.Include(gc => gc.GymCourse).Include(gc => gc.Pt),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetGymCoursePTResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách thông tin pt trong khóa học thành công",
                data = gymCoursePt
            };
        }

        public async Task<BaseResponse<GetGymCoursePTResponse>> GetGymCoursePT(Guid id)
        {
            var gymCoursePt = await _unitOfWork.GetRepository<GymCoursePt>().SingleOrDefaultAsync(
                selector: gc => _mapper.Map<GetGymCoursePTResponse>(gc),
                predicate: gc => gc.Id.Equals(id) && gc.Active == true,
                include: gc => gc.Include(gc => gc.GymCourse).Include(gc => gc.Pt));

            if (gymCoursePt == null)
            {
                return new BaseResponse<GetGymCoursePTResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin pt trong khóa học",
                    data = null
                };
            }

            return new BaseResponse<GetGymCoursePTResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin pt trong khóa học",
                data = gymCoursePt
            };
        }
    }
}

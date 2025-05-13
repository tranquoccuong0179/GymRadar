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
using GymRadar.Model.Payload.Response.PT;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class PTService : BaseService<PTService>, IPTService
    {
        public PTService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<PTService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateNewPTResponse>> CreateNewPT(RegisterAccountPTRequest request)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(userId));
            if (user == null)
            {
                return new BaseResponse<CreateNewPTResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người tạo",
                    data = null
                };
            }

            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(userId));

            if (gym == null)
            {
                return new BaseResponse<CreateNewPTResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy phòng gym",
                    data = null
                };
            }

            var account = _mapper.Map<Account>(request);
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);

            var pt = _mapper.Map<Pt>(request.CreateNewPT);
            pt.AccountId = account.Id;
            pt.GymId = gym.Id;
            await _unitOfWork.GetRepository<Pt>().InsertAsync(pt);

            await _unitOfWork.CommitAsync();

            var response = _mapper.Map<CreateNewPTResponse>(pt);
            response.Register = _mapper.Map<RegisterResponse>(account);

            return new BaseResponse<CreateNewPTResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Đăng kí PT thành công",
                data = response
            };
        }

        public async Task<BaseResponse<IPaginate<GetPTResponse>>> GetAllPT(int page, int size)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(userId));

            if (account == null)
            {
                return new BaseResponse<IPaginate<GetPTResponse>>
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
                return new BaseResponse<IPaginate<GetPTResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy phòng gym",
                    data = null
                };
            }

            var pts = await _unitOfWork.GetRepository<Pt>().GetPagingListAsync(
                selector: p => _mapper.Map<GetPTResponse>(p),
                predicate: p => p.GymId.Equals(gym.Id) && p.Active == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetPTResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách PT của phòng gym thành công",
                data = pts
            };
        }

        public async Task<BaseResponse<IPaginate<GetPTResponse>>> GetAllPTForAdmin(int page, int size)
        {
            var pts = await _unitOfWork.GetRepository<Pt>().GetPagingListAsync(
                selector: p => _mapper.Map<GetPTResponse>(p),
                predicate: p => p.Active == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetPTResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách PT thành công",
                data = pts
            };
        }

        public async Task<BaseResponse<IPaginate<GetPTResponse>>> GetAllPTForUser(Guid id, int page, int size)
        {
            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.Id.Equals(id) && g.Active == true);
            if (gym == null)
            {
                return new BaseResponse<IPaginate<GetPTResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy phòng gym",
                    data = null
                };
            }

            var pts = await _unitOfWork.GetRepository<Pt>().GetPagingListAsync(
                selector: p => _mapper.Map<GetPTResponse>(p),
                predicate: p => p.GymId.Equals(gym.Id) && p.Active == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetPTResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách PT của phòng gym thành công",
                data = pts
            };
        }
    }
}

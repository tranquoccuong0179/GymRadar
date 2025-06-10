using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.Premium;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Premium;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class PremiumService : BaseService<PremiumService>, IPremiumService
    {
        public PremiumService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<PremiumService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateNewPremiumResponse>> CreatePremium(CreateNewPremiumRequest request)
        {
            var premiumExist = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: p => p.Name.Equals(request.Name));
            if (premiumExist != null)
            {
                return new BaseResponse<CreateNewPremiumResponse>
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Premium này đã tồn tại",
                    data = null
                };
            }
            var premium = _mapper.Map<Premium>(request);
            await _unitOfWork.GetRepository<Premium>().InsertAsync(premium);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<CreateNewPremiumResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Tạo gói premium thành công",
                data = _mapper.Map<CreateNewPremiumResponse>(premium)
            };
        }

        public async Task<BaseResponse<IPaginate<GetPremiumResponse>>> GetAllPremium(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<Premium>().GetPagingListAsync(
                selector: p => _mapper.Map<GetPremiumResponse>(p),
                predicate: p => p.Active == true,
                page: page,
                size: size);


            return new BaseResponse<IPaginate<GetPremiumResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách gói premium thành công",
                data = response
            };
        }

        public async Task<BaseResponse<GetPremiumResponse>> GetPremiumById(Guid id)
        {
            var response = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                selector: p => _mapper.Map<GetPremiumResponse>(p),
                predicate: p => p.Id.Equals(id) && p.Active == true);

            if (response == null)
            {
                return new BaseResponse<GetPremiumResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy premium",
                    data = null
                };
            }

            return new BaseResponse<GetPremiumResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin gói premium thành công",
                data = response
            };
        }
    }
}

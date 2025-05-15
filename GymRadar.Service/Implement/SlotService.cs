using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.Slot;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Slot;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class SlotService : BaseService<SlotService>, ISlotService
    {
        public SlotService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<SlotService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateNewSlotResponse>> CreateSlot([FromBody] CreateNewSlotRequest request)
        {
            var slot = _mapper.Map<Slot>(request);

            await _unitOfWork.GetRepository<Slot>().InsertAsync(slot);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<CreateNewSlotResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Tạo slot thành công",
                data = _mapper.Map<CreateNewSlotResponse>(slot)
            };
        }

        public async Task<BaseResponse<IPaginate<GetSlotResponse>>> GetAllSlot(int page, int size)
        {
            var slots = await _unitOfWork.GetRepository<Slot>().GetPagingListAsync(
                selector: s => _mapper.Map<GetSlotResponse>(s),
                predicate: s => s.Active == true,
                page: page,
                size: size);


            return new BaseResponse<IPaginate<GetSlotResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách slot thành công",
                data = slots
            };
        }

        public async Task<BaseResponse<GetSlotResponse>> GetSlot(Guid id)
        {
            var slot = await _unitOfWork.GetRepository<Slot>().SingleOrDefaultAsync(
                selector: s => _mapper.Map<GetSlotResponse>(s),
                predicate: s => s.Id.Equals(id) && s.Active == true);

            if (slot == null)
            {
                return new BaseResponse<GetSlotResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy slot",
                    data = null
                };
            }

            return new BaseResponse<GetSlotResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy slot thành công",
                data = slot
            };
        }
    }
}

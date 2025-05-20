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
using GymRadar.Model.Utils;
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

        public async Task<BaseResponse<CreateNewSlotResponse>> CreateSlot(CreateNewSlotRequest request)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(userId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<CreateNewSlotResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(userId) && g.Active == true);

            if (gym == null)
            {
                return new BaseResponse<CreateNewSlotResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy phòng gym",
                    data = null
                };
            }

            var existingSlotByName = await _unitOfWork.GetRepository<Slot>().SingleOrDefaultAsync(
                predicate: s => s.GymId.Equals(gym.Id) && s.Name.Equals(request.Name) && s.Active == true);

            if (existingSlotByName != null)
            {
                return new BaseResponse<CreateNewSlotResponse>
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Tên slot đã tồn tại",
                    data = null
                };
            }

            var isExactTimeMatch = await _unitOfWork.GetRepository<Slot>().SingleOrDefaultAsync(
                predicate: s => s.GymId.Equals(gym.Id) && s.Active == true &&
                       s.StartTime == request.StartTime && s.EndTime == request.EndTime);

            if (isExactTimeMatch != null)
            {
                return new BaseResponse<CreateNewSlotResponse>
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = $"Khung giờ {request.StartTime} - {request.EndTime} đã tồn tại trong slot {isExactTimeMatch.Name}",
                    data = null
                };
            }

            var isTimeConflict = await _unitOfWork.GetRepository<Slot>().SingleOrDefaultAsync(
                predicate: s => s.GymId.Equals(gym.Id) && s.Active == true &&
                               request.StartTime < s.EndTime && request.EndTime > s.StartTime);

            if (isTimeConflict != null)
            {
                return new BaseResponse<CreateNewSlotResponse>
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = $"Khoảng thời gian từ {request.StartTime} đến {request.EndTime} bị trùng với slot {isTimeConflict.Name} ({isTimeConflict.StartTime} - {isTimeConflict.EndTime})",
                    data = null
                };
            }

            var newSlot = _mapper.Map<Slot>(request);
            newSlot.GymId = gym.Id;
            await _unitOfWork.GetRepository<Slot>().InsertAsync(newSlot);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<CreateNewSlotResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Tạo slot thành công",
                data = _mapper.Map<CreateNewSlotResponse>(newSlot)
            };
        }

        public async Task<BaseResponse<bool>> DeleteSlot(Guid id)
        {
            var slot = await _unitOfWork.GetRepository<Slot>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(id) && s.Active == true);

            if (slot == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy slot",
                    data = false
                };
            }

            slot.Active = false;
            slot.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Slot>().UpdateAsync(slot);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<bool>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Xóa slot thành công",
                data = true
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

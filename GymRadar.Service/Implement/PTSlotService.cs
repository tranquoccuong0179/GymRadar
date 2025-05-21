using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.PTSlot;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.PTSlot;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class PTSlotService : BaseService<PTSlotService>, IPTSlotService
    {
        public PTSlotService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<PTSlotService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<bool>> ActiveSlot(Guid id)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(userId));

            if (account == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = false
                };
            }

            var pt = await _unitOfWork.GetRepository<Pt>().SingleOrDefaultAsync(
                predicate: p => p.AccountId.Equals(userId) && p.Active == true);

            if (pt == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin PT",
                    data = false
                };
            }

            var ptSlot = await _unitOfWork.GetRepository<Ptslot>().SingleOrDefaultAsync(
                predicate: ps => ps.Id.Equals(id) && ps.Ptid.Equals(pt.Id));

            if (ptSlot == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy khung giờ này của pt",
                    data = false
                };
            }

            ptSlot.Active = true;
            ptSlot.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Ptslot>().UpdateAsync(ptSlot);
            await _unitOfWork.CommitAsync();


            return new BaseResponse<bool>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Kích hoạt khung giờ thành công",
                data = true
            };
        }

        public async Task<BaseResponse<CreatePTSlotResponse>> CreatePTSlot(CreatePTSlotRequest request)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(userId));

            if (account == null)
            {
                return new BaseResponse<CreatePTSlotResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = null
                };
            }

            var pt = await _unitOfWork.GetRepository<Pt>().SingleOrDefaultAsync(
                predicate: p => p.AccountId.Equals(userId) && p.Active == true);

            if (pt == null)
            {
                return new BaseResponse<CreatePTSlotResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin PT",
                    data = null
                };
            }

            var slot = await _unitOfWork.GetRepository<Slot>().SingleOrDefaultAsync(
                predicate: s => s.GymId.Equals(pt.GymId) && s.Active == true);

            if (slot == null)
            {
                return new BaseResponse<CreatePTSlotResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy khung giờ",
                    data = null
                };
            }

            var existingPtSlot = await _unitOfWork.GetRepository<Ptslot>().SingleOrDefaultAsync(
                predicate: ps => ps.Ptid.Equals(pt.Id) && ps.SlotId.Equals(request.SlotId));

            if (existingPtSlot != null)
            {
                return new BaseResponse<CreatePTSlotResponse>
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "PT đã đăng ký khung giờ này rồi",
                    data = null
                };
            }

            var ptSlot = _mapper.Map<Ptslot>(request);
            ptSlot.Ptid = pt.Id;

            await _unitOfWork.GetRepository<Ptslot>().InsertAsync(ptSlot);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<CreatePTSlotResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "PT đăng kí khung giờ thành công",
                data = _mapper.Map<CreatePTSlotResponse>(ptSlot)
            };
        }

        public async Task<BaseResponse<IPaginate<GetPTSlotResponse>>> GetPTSlot(int page, int size)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(userId));

            if (account == null)
            {
                return new BaseResponse<IPaginate<GetPTSlotResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = null
                };
            }

            var pt = await _unitOfWork.GetRepository<Pt>().SingleOrDefaultAsync(
                predicate: p => p.AccountId.Equals(userId) && p.Active == true);

            if (pt == null)
            {
                return new BaseResponse<IPaginate<GetPTSlotResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin PT",
                    data = null
                };
            }

            var ptSlots = await _unitOfWork.GetRepository<Ptslot>().GetPagingListAsync(
                selector: ps => _mapper.Map<GetPTSlotResponse>(ps),
                predicate: ps => ps.Ptid.Equals(pt.Id),
                include: ps => ps.Include(ps => ps.Slot),
                page: page,
                size: size);


            return new BaseResponse<IPaginate<GetPTSlotResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách thông tin khung giờ cho PT thành công",
                data = ptSlots
            };
        }

        public async Task<BaseResponse<bool>> UnActiveSlot(Guid id)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(userId));

            if (account == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = false
                };
            }

            var pt = await _unitOfWork.GetRepository<Pt>().SingleOrDefaultAsync(
                predicate: p => p.AccountId.Equals(userId) && p.Active == true);

            if (pt == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin PT",
                    data = false
                };
            }

            var ptSlot = await _unitOfWork.GetRepository<Ptslot>().SingleOrDefaultAsync(
                predicate: ps => ps.Id.Equals(id) && ps.Ptid.Equals(pt.Id));

            if (ptSlot == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy khung giờ này của pt",
                    data = false
                };
            }

            ptSlot.Active = false;
            ptSlot.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Ptslot>().UpdateAsync(ptSlot);
            await _unitOfWork.CommitAsync();


            return new BaseResponse<bool>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Hủy khung giờ thành công",
                data = true
            };
        }
    }
}

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
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class PTSlotService : BaseService<PTSlotService>, IPTSlotService
    {
        public PTSlotService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<PTSlotService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
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
    }
}

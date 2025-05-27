using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.PTSlot;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.PTSlot;

namespace GymRadar.Service.Interface
{
    public interface IPTSlotService
    {
        Task<BaseResponse<CreatePTSlotResponse>> CreatePTSlot(CreatePTSlotRequest request);
        Task<BaseResponse<bool>> ActiveSlot(Guid id);
        Task<BaseResponse<bool>> UnActiveSlot(Guid id);
        Task<BaseResponse<GetPTSlot>> GetPTSlot(DateOnly date);
        Task<BaseResponse<GetPTSlot>> GetPTSlotForUser(Guid id, DateOnly date);
    }
}

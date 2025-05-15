using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.Slot;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Slot;

namespace GymRadar.Service.Interface
{
    public interface ISlotService
    {
        Task<BaseResponse<CreateNewSlotResponse>> CreateSlot(CreateNewSlotRequest request);
        Task<BaseResponse<IPaginate<GetSlotResponse>>> GetAllSlot(int page, int size);
        Task<BaseResponse<GetSlotResponse>> GetSlot(Guid id);
        Task<BaseResponse<bool>> DeleteSlot(Guid id);
    }
}

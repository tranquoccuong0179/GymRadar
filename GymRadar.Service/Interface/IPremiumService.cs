using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.Premium;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Premium;

namespace GymRadar.Service.Interface
{
    public interface IPremiumService
    {
        Task<BaseResponse<CreateNewPremiumResponse>> CreatePremium(CreateNewPremiumRequest request);
        Task<BaseResponse<IPaginate<GetPremiumResponse>>> GetAllPremium(int page, int size);
        Task<BaseResponse<GetPremiumResponse>> GetPremiumById(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.PT;

namespace GymRadar.Service.Interface
{
    public interface IPTService
    {
        Task<BaseResponse<CreateNewPTResponse>> CreateNewPT(RegisterAccountPTRequest request);
        Task<BaseResponse<IPaginate<GetPTResponse>>> GetAllPT(int page, int size);
        Task<BaseResponse<IPaginate<GetPTResponse>>> GetAllPTForAdmin(int page, int size);
        Task<BaseResponse<IPaginate<GetPTResponse>>> GetAllPTForUser(Guid id, int page, int size);
        Task<BaseResponse<bool>> DeletePT(Guid id);

    }
}

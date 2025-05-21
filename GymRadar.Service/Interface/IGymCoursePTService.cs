using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.GymCoursePT;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.GymCoursePT;

namespace GymRadar.Service.Interface
{
    public interface IGymCoursePTService
    {
        Task<BaseResponse<CreateGymCoursePTResponse>> CreateGymCoursePT(CreateGymCoursePTRequest request);
        
        Task<BaseResponse<IPaginate<GetGymCoursePTResponse>>> GetAllGymCoursePT(Guid id, int page, int size);

        Task<BaseResponse<GetGymCoursePTResponse>> GetGymCoursePT(Guid id);
    }
}

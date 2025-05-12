using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Gym;

namespace GymRadar.Service.Interface
{
    public interface IGymService
    {
        Task<BaseResponse<CreateNewGymResponse>> CreateNewGym(RegisterAccountGymRequest request);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.User;

namespace GymRadar.Service.Interface
{
    public interface IAccountService
    {
        Task<BaseResponse<RegisterResponse>> RegisterAccount(RegisterRequest request);
        Task<BaseResponse<RegisterResponse>> RegisterAdmin(RegisterAdminRequest request);
        Task<BaseResponse<GetUserProfileResponse>> UserProfile();

    }
}

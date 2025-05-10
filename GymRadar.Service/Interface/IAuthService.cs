using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Request.Auth;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Auth;

namespace GymRadar.Service.Interface
{
    public interface IAuthService
    {
        Task<BaseResponse<AuthenticationResponse>> Authenticate(AuthenticationRequest request);

    }
}

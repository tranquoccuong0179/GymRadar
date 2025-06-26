using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.User;

namespace GymRadar.Service.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<IPaginate<GetUserResponse>>> GetAllUser(int page, int size);

        Task<BaseResponse<bool>> DeleteUser(Guid id);

        Task<BaseResponse<GetUserResponse>> UpdateUser(UpdateUserRequest request);
    }
}

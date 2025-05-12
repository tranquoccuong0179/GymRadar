using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Gym;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class GymService : BaseService<GymService>, IGymService
    {
        public GymService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<GymService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateNewGymResponse>> CreateNewGym(RegisterAccountGymRequest request)
        {
            var account = _mapper.Map<Account>(request);
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);

            var gym = _mapper.Map<Gym>(request.CreateNewGym);
            gym.AccountId = account.Id;
            await _unitOfWork.GetRepository<Gym>().InsertAsync(gym);

            await _unitOfWork.CommitAsync();

            var response = _mapper.Map<CreateNewGymResponse>(gym);
            response.Register = _mapper.Map<RegisterResponse>(account);


            return new BaseResponse<CreateNewGymResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Đăng ký phòng gym thành công",
                data = response
            };
        }
    }
}

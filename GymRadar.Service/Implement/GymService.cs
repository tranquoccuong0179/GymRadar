﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Gym;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Utils;
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

        public async Task<BaseResponse<bool>> DeleteGym(Guid id)
        {
            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.Id.Equals(id) && g.Active == true);

            if (gym == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy phòng gym này",
                    data = false
                };
            }

            gym.Active = false;
            gym.UpdateAt = TimeUtil.GetCurrentSEATime();
            gym.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Gym>().UpdateAsync(gym);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<bool>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Xóa thành công phòng gym này",
                data = true
            };
        }

        public async Task<BaseResponse<IPaginate<GetGymResponse>>> GetAllGym(int page, int size)
        {
            var gyms = await _unitOfWork.GetRepository<Gym>().GetPagingListAsync(
                selector: g => _mapper.Map<GetGymResponse>(g),
                predicate: g => g.Active == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetGymResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách các phòng gym thành công",
                data = gyms
            };
        }

        public async Task<BaseResponse<GetGymResponse>> GetGymById(Guid id)
        {
            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                selector : g => _mapper.Map<GetGymResponse>(g),
                predicate: g => g.Id.Equals(id) && g.Active == true);

            if (gym == null)
            {
                return new BaseResponse<GetGymResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy phòng gym này",
                    data = null
                };
            }

            return new BaseResponse<GetGymResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin phòng gym thành công",
                data = gym
            };
        }
    }
}

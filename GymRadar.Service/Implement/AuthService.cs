using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Enum;
using GymRadar.Model.Payload.Request.Auth;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Auth;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        public AuthService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<AuthService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
        {
            Expression<Func<Account, bool>> searchFilter = p =>
                  p.Phone.Equals(request.Phone) &&
                  p.Password.Equals(PasswordUtil.HashPassword(request.Password)) &&
                  (p.Role == RoleEnum.ADMIN.GetDescriptionFromEnum() ||
                  p.Role == RoleEnum.GYM.GetDescriptionFromEnum() ||
                  p.Role == RoleEnum.PT.GetDescriptionFromEnum() ||
                  p.Role == RoleEnum.USER.GetDescriptionFromEnum());
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: searchFilter);
            if (account == null)
            {
                return new BaseResponse<AuthenticationResponse>()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Số điện thoại hoặc mật khẩu không đúng",
                    data = null
                };
            }

            RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role);
            Tuple<string, Guid> guildClaim = new Tuple<string, Guid>("accountId", account.Id);
            var token = JwtUtil.GenerateJwtToken(account, guildClaim);

            var response = new AuthenticationResponse()
            {
                Id = account.Id,
                Phone = account.Phone,
                Role = account.Role,
                AccessToken = token,
            };

            return new BaseResponse<AuthenticationResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Đăng nhập thành công",
                data = response
            };
        }
    }
}

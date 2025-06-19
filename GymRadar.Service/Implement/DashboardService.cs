using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Enum;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Dashboard;
using GymRadar.Model.Payload.Response.GymCourse;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class DashboardService : BaseService<DashboardService>, IDashboardService
    {
        public DashboardService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<DashboardService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<List<GetDashboardTransactionResponse>>> GetDashboardTransaction(DateOnly startDate, DateOnly endDate)
        {
            var transactions = await _unitOfWork.GetRepository<Transaction>().GetListAsync(
                predicate: t => DateOnly.FromDateTime(t.CreateAt.Value) >= startDate 
                && DateOnly.FromDateTime(t.CreateAt.Value) <= endDate 
                && (t.Type.Equals(TypeTransactionEnum.Premium.GetDescriptionFromEnum()) || t.Type.Equals(TypeTransactionEnum.Course.GetDescriptionFromEnum()))
                && t.Status.Equals(StatusTransactionEnum.COMPLETED.GetDescriptionFromEnum()));

            var result = transactions
            .GroupBy(t => DateOnly.FromDateTime(t.CreateAt.Value))
            .Select(g => new GetDashboardTransactionResponse
            {
                Date = g.Key,
                SubscriptionIncome = g.Sum(t => t.Type.Equals(TypeTransactionEnum.Premium.GetDescriptionFromEnum()) ? t.Price.Value : 0),
                TransactionIncome = g.Sum(t => t.Type.Equals(TypeTransactionEnum.Course.GetDescriptionFromEnum()) ? t.Price.Value : 0),
                TotalRevenue = g.Sum(t => t.Type.Equals(TypeTransactionEnum.Premium.GetDescriptionFromEnum()) ? t.Price.Value : 0) +
                              g.Sum(t => t.Type.Equals(TypeTransactionEnum.Course.GetDescriptionFromEnum()) ? t.Price.Value : 0),
                Profit = g.Sum(t => t.Type.Equals(TypeTransactionEnum.Premium.GetDescriptionFromEnum()) ? t.Price.Value : 0) +
                         g.Sum(t => t.Type.Equals(TypeTransactionEnum.Course.GetDescriptionFromEnum()) ? t.Price.Value * 0.1 : 0)
            })
            .OrderBy(r => r.Date)
            .ToList();

            return new BaseResponse<List<GetDashboardTransactionResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Thành công",
                data = result
            };
        }

        public async Task<BaseResponse<List<GetDashboardTransactionResponseByGym>>> GetDashboardTransactionByGym(DateOnly startDate, DateOnly endDate)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(userId) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<List<GetDashboardTransactionResponseByGym>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(user.Id) && g.Active == true);

            if (gym == null)
            {
                return new BaseResponse<List<GetDashboardTransactionResponseByGym>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không có phòng gym",
                    data = null
                };
            }

            var transactions = await _unitOfWork.GetRepository<Transaction>().GetListAsync(
                predicate: t => DateOnly.FromDateTime(t.CreateAt.Value) >= startDate
                && DateOnly.FromDateTime(t.CreateAt.Value) <= endDate
                && t.Type.Equals(TypeTransactionEnum.Course.GetDescriptionFromEnum())
                && t.GymCourse.GymId.Equals(gym.Id)
                && t.Status.Equals(StatusTransactionEnum.COMPLETED.GetDescriptionFromEnum()),
                include: t => t.Include(t => t.GymCourse).ThenInclude(gc => gc.Gym));

            var result = transactions
            .GroupBy(t => DateOnly.FromDateTime(t.CreateAt.Value))
            .Select(g => new GetDashboardTransactionResponseByGym
            {
                Date = g.Key,
                TotalRevenue = g.Where(t => t.Price.HasValue).Sum(t => t.Price.Value),
                AppCommission = g.Where(t => t.Price.HasValue).Sum(t => t.Price.Value) * 0.1,
                PaybackToGym = g.Where(t => t.Price.HasValue).Sum(t => t.Price.Value) * 0.9
            })
            .OrderBy(r => r.Date)
            .ToList();

            return new BaseResponse<List<GetDashboardTransactionResponseByGym>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Thành công",
                data = result
            };
        }
    }
}

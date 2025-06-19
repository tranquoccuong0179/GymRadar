using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Dashboard;

namespace GymRadar.Service.Interface
{
    public interface IDashboardService
    {
        Task<BaseResponse<List<GetDashboardTransactionResponse>>> GetDashboardTransaction(DateOnly startDate, DateOnly endDate);
        Task<BaseResponse<List<GetDashboardTransactionResponseByGym>>> GetDashboardTransactionByGym(DateOnly startDate, DateOnly endDate);
    }
}

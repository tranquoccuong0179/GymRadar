
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.Dashboard;
using GymRadar.Service.Interface;

namespace GymRadar.API.Controllers
{
    public class DashboardController : BaseController<DashboardController>
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService) : base(logger)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet(ApiEndPointConstant.Dashboard.GetProfit)]
        [ProducesResponseType(typeof(BaseResponse<GetDashboardTransactionResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetProfit([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            var response = await _dashboardService.GetDashboardTransaction(startDate, endDate);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

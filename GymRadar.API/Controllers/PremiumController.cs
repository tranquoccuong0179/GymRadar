
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.Premium;
using GymRadar.Model.Payload.Request.Premium;
using GymRadar.Model.Paginate;

namespace GymRadar.API.Controllers
{
    public class PremiumController : BaseController<PremiumController>
    {
        private readonly IPremiumService _premiumService;
        public PremiumController(ILogger<PremiumController> logger, IPremiumService premiumService) : base(logger)
        {
            _premiumService = premiumService;
        }

        [HttpPost(ApiEndPointConstant.Premium.CreatePremium)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewPremiumResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewPremiumResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreatePremium([FromBody] CreateNewPremiumRequest request)
        {
            var response = await _premiumService.CreatePremium(request);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Premium.GetAllPremium)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPremiumResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPremium([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _premiumService.GetAllPremium(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Premium.GetPremiumById)]
        [ProducesResponseType(typeof(BaseResponse<GetPremiumResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetPremiumResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetPremiumById([FromRoute] Guid id)
        {
            var response = await _premiumService.GetPremiumById(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

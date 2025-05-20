
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.PTSlot;
using GymRadar.Model.Payload.Request.PTSlot;

namespace GymRadar.API.Controllers
{
    public class PTSlotController : BaseController<PTSlotController>
    {
        private readonly IPTSlotService _ptSlotService;
        public PTSlotController(ILogger<PTSlotController> logger, IPTSlotService ptSlotService) : base(logger)
        {
            _ptSlotService = ptSlotService;
        }

        [HttpPost(ApiEndPointConstant.PTSlot.CreatePTSlot)]
        [ProducesResponseType(typeof(BaseResponse<CreatePTSlotResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreatePTSlotResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<CreatePTSlotResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreatePTSlot([FromBody] CreatePTSlotRequest request)
        {
            var response = await _ptSlotService.CreatePTSlot(request);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

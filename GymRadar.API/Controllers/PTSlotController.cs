
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.PTSlot;
using GymRadar.Model.Payload.Request.PTSlot;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet(ApiEndPointConstant.PTSlot.GetAllPTSlot)]
        [ProducesResponseType(typeof(BaseResponse<GetPTSlot>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetPTSlot>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPTSlot([FromQuery, Required] DateOnly date)
        {
            var response = await _ptSlotService.GetPTSlot(date);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.PTSlot.GetAllPTSlotForUser)]
        [ProducesResponseType(typeof(BaseResponse<GetPTSlot>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetPTSlot>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPTSlotForUser([FromRoute] Guid id, [FromQuery, Required] DateOnly date)
        {
            var response = await _ptSlotService.GetPTSlotForUser(id, date);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPut(ApiEndPointConstant.PTSlot.ActivePTSlot)]
        [ProducesResponseType(typeof(BaseResponse<CreatePTSlotResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreatePTSlotResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> ActivePTSlot([FromRoute] Guid id)
        {
            var response = await _ptSlotService.ActiveSlot(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPut(ApiEndPointConstant.PTSlot.UnActivePTSlot)]
        [ProducesResponseType(typeof(BaseResponse<CreatePTSlotResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreatePTSlotResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UnActivePTSlot([FromRoute] Guid id)
        {
            var response = await _ptSlotService.UnActiveSlot(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

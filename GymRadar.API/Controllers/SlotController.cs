
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.Slot;
using GymRadar.Model.Payload.Request.Slot;
using GymRadar.Model.Paginate;

namespace GymRadar.API.Controllers
{
    public class SlotController : BaseController<SlotController>
    {
        private readonly ISlotService _slotService;
        public SlotController(ILogger<SlotController> logger, ISlotService slotService) : base(logger)
        {
            _slotService = slotService;
        }

        [HttpPost(ApiEndPointConstant.Slot.CreateSlot)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewSlotResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewSlotResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateSlot([FromBody] CreateNewSlotRequest request)
        {
            var response = await _slotService.CreateSlot(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Slot.GetAllSlot)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetSlotResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllSlot([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _slotService.GetAllSlot(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Slot.GetSlot)]
        [ProducesResponseType(typeof(BaseResponse<GetSlotResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetSlotResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetSlot([FromRoute] Guid id)
        {
            var response = await _slotService.GetSlot(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.Slot.DeleteSlot)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteSlot([FromRoute] Guid id)
        {
            var response = await _slotService.DeleteSlot(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

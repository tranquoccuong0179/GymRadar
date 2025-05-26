
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Request.Booking;
using GymRadar.Model.Payload.Response.Booking;
using GymRadar.Model.Paginate;
using GymRadar.Model.Enum;

namespace GymRadar.API.Controllers
{
    public class BookingController : BaseController<BookingController>
    {
        private readonly IBookingService _bookingService;
        public BookingController(ILogger<BookingController> logger, IBookingService bookingService) : base(logger)
        {
            _bookingService = bookingService;
        }

        [HttpPost(ApiEndPointConstant.Booking.CreateBooking)]
        [ProducesResponseType(typeof(BaseResponse<CreateBookingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateBookingResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CreateBookingResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var response = await _bookingService.CreateBooking(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Booking.GetBookingForUser)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetBookingResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetBookingResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetBookingForUser([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookingService.GetBookingForUser(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Booking.GetBookingForPT)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetBookingResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetBookingResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetBookingForPT([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookingService.GetBookingForPT(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Booking.GetBooking)]
        [ProducesResponseType(typeof(BaseResponse<GetBookingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetBookingResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<GetBookingResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetBooking([FromRoute] Guid id)
        {
            var response = await _bookingService.GetBookingById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPut(ApiEndPointConstant.Booking.UpdateBooking)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateBooking([FromRoute] Guid id, [FromQuery] StatusBookingEnum status)
        {
            var response = await _bookingService.UpdateBooking(id, status);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}


using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.Slot;
using GymRadar.Model.Payload.Request.Slot;
using GymRadar.Model.Paginate;
using Microsoft.AspNetCore.Authorization;

namespace GymRadar.API.Controllers
{
    public class SlotController : BaseController<SlotController>
    {
        private readonly ISlotService _slotService;
        public SlotController(ILogger<SlotController> logger, ISlotService slotService) : base(logger)
        {
            _slotService = slotService;
        }

        /// <summary>
        /// API tạo khung giờ (slot) mới.
        /// </summary>
        /// <remarks>
        /// - API này cho phép tạo một khung giờ mới với thông tin như tên, thời gian bắt đầu và kết thúc.
        /// - Yêu cầu xác thực (bearer token) để lấy thông tin tài khoản người tạo (chủ phòng gym).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   POST /api/v1/slot
        ///   Authorization: Bearer &lt;JWT_token&gt;
        ///   Content-Type: application/json
        ///   {
        ///     "name": "Slot 1",
        ///     "startTime": "07:00:00",
        ///     "endTime": "08:00:00"
        ///   }
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Tạo slot thành công. Trả về `BaseResponse&lt;CreateNewSlotResponse&gt;` chứa thông tin khung giờ vừa tạo.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: thiếu `name`, `startTime` lớn hơn hoặc bằng `endTime`).
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Tạo slot thành công",
        ///     "data": {
        ///       "name": "Slot 1",
        ///       "startTime": "07:00:00",
        ///       "endTime": "08:00:00"
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="request">Thông tin khung giờ cần tạo, bao gồm tên, thời gian bắt đầu và kết thúc.</param>
        /// <returns>
        /// - `200 OK`: Tạo slot thành công.
        /// - `400 Bad Request`: Tham số đầu vào không hợp lệ.
        /// </returns>
        /// <response code="200">Trả về thông tin khung giờ vừa tạo khi yêu cầu thành công.</response>
        /// <response code="400">Trả về lỗi nếu tham số đầu vào không hợp lệ.</response>
        [HttpPost(ApiEndPointConstant.Slot.CreateSlot)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewSlotResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewSlotResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateSlot([FromBody] CreateNewSlotRequest request)
        {
            var response = await _slotService.CreateSlot(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách tất cả slot.
        /// </summary>
        /// <remarks>
        /// - API này cho phép lấy danh sách các slot với hỗ trợ phân trang thông qua `page` và `size`.
        /// - Nếu không cung cấp `page`, mặc định là 1. Nếu không cung cấp `size`, mặc định là 10.
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/slot?page=1&amp;size=10
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy danh sách slot thành công. Trả về `BaseResponse&lt;IPaginate&lt;GetSlotResponse&gt;&gt;` chứa danh sách khung giờ và thông tin phân trang.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: `page` hoặc `size` nhỏ hơn 1).
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Lấy danh sách slot thành công",
        ///     "data": {
        ///       "size": 10,
        ///       "page": 1,
        ///       "total": 2,
        ///       "totalPages": 1,
        ///       "items": [
        ///         {
        ///           "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///           "name": "Slot 1",
        ///           "startTime": "07:00:00",
        ///           "endTime": "08:00:00"
        ///         }
        ///       ]
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="page">Số trang (tùy chọn, mặc định là 1).</param>
        /// <param name="size">Kích thước trang (tùy chọn, mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách slot thành công.
        /// </returns>
        /// <response code="200">Trả về danh sách khung giờ và thông tin phân trang khi yêu cầu thành công.</response>
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

        /// <summary>
        /// API lấy thông tin chi tiết một khung giờ theo ID.
        /// </summary>
        /// <remarks>
        /// - API này trả về thông tin chi tiết của khung giờ dựa trên `id` cung cấp.
        /// - API không yêu cầu xác thực (công khai cho người dùng).
        /// - API dùng để người dùng xem chi tiết khung giờ (ví dụ: để kiểm tra lịch trình).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/slot/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy slot thành công. Trả về `BaseResponse&lt;GetSlotResponse&gt;` chứa thông tin khung giờ.
        ///   - `404 NotFound`: Không tìm thấy khung giờ với `id` cung cấp.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Lấy slot thành công",
        ///     "data": {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "name": "Slot 1",
        ///       "startTime": "07:00:00",
        ///       "endTime": "08:00:00"
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="id">ID của khung giờ cần lấy thông tin.</param>
        /// <returns>
        /// - `200 OK`: Lấy slot thành công.
        /// - `404 NotFound`: Không tìm thấy khung giờ.
        /// </returns>
        /// <response code="200">Trả về thông tin chi tiết khung giờ khi yêu cầu thành công.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy khung giờ.</response>
        [HttpGet(ApiEndPointConstant.Slot.GetSlot)]
        [ProducesResponseType(typeof(BaseResponse<GetSlotResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetSlotResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetSlot([FromRoute] Guid id)
        {
            var response = await _slotService.GetSlot(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa khung giờ theo ID.
        /// </summary>
        /// <remarks>
        /// - API này cho phép xóa một slot dựa trên `id` cung cấp.
        /// - API dùng để quản lý khung giờ (xóa khung giờ không còn sử dụng).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   DELETE /api/v1/slot/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Xóa slot thành công. Trả về `BaseResponse&lt;bool&gt;` với giá trị `true`.
        ///   - `404 NotFound`: Không tìm thấy khung giờ với `id` cung cấp.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Xóa slot thành công",
        ///     "data": true
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="id">ID của khung giờ cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa slot thành công.
        /// - `404 NotFound`: Không tìm thấy khung giờ.
        /// </returns>
        /// <response code="200">Trả về `true` khi xóa slot thành công.</response>
        /// <response code="404">Trả về lỗi và `false` nếu không tìm thấy khung giờ.</response>
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


using GymRadar.API.Constant;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response.Gym;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Request.GymCourse;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Response.GymCourse;
using Microsoft.AspNetCore.Authorization;
using GymRadar.Model.Payload.Response.PT;

namespace GymRadar.API.Controllers
{
    public class GymCourseController : BaseController<GymCourseController>
    {
        private readonly IGymCourseService _gymCourseService;
        public GymCourseController(ILogger<GymCourseController> logger, IGymCourseService gymCourseService) : base(logger)
        {
            _gymCourseService = gymCourseService;
        }

        /// <summary>
        /// API tạo khóa học mới cho phòng gym, dành cho chủ phòng gym.
        /// </summary>
        /// <remarks>
        /// - API này cho phép chủ phòng gym tạo một khóa học mới với thông tin như tên, giá, thời gian, loại khóa học, mô tả và hình ảnh.
        /// - API yêu cầu xác thực (JWT) và người dùng phải có vai trò chủ phòng gym.
        /// - Loại khóa học (`Type`) có thể là `Normal` hoặc `WithPT`.
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   POST /api/v1/course
        ///   Authorization: Bearer &lt;JWT_token&gt;
        ///   Content-Type: application/json
        ///   {
        ///     "name": "Gói 1",
        ///     "price": 500000,
        ///     "duration": 30,
        ///     "type": "Normal",
        ///     "description": "Khóa học dành cho người mới bắt đầu",
        ///     "image": "blabla"
        ///   }
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Tạo khóa học thành công. Trả về `BaseResponse&lt;CreateGymCourseResponse&gt;` chứa thông tin khóa học vừa tạo.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: thiếu `name`, `price` không hợp lệ, hoặc `type` không đúng).
        ///   - `401 Unauthorized`: Không cung cấp token hợp lệ.
        ///   - `403 Forbidden`: Người dùng không có quyền chủ phòng gym.
        ///   - `404 NotFound`: Không tìm thấy phòng gym liên quan đến chủ phòng gym.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Tạo khóa gym thành công",
        ///     "data": {
        ///       "name": "Gói 1",
        ///       "price": 500000,
        ///       "duration": 30,
        ///       "type": "Normal",
        ///       "description": "Khóa học dành cho người mới bắt đầu",
        ///       "image": "blabla...."
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="request">Thông tin khóa học cần tạo, bao gồm tên, giá, thời gian, loại, mô tả và hình ảnh.</param>
        /// <returns>
        /// - `200 OK`: Tạo khóa học thành công.
        /// - `400 Bad Request`: Tham số đầu vào không hợp lệ.
        /// - `401 Unauthorized`: Không cung cấp token hợp lệ.
        /// - `403 Forbidden`: Người dùng không có quyền chủ phòng gym.
        /// - `404 NotFound`: Không tìm thấy phòng gym.
        /// </returns>
        /// <response code="200">Trả về thông tin khóa học vừa tạo khi yêu cầu thành công.</response>
        /// <response code="400">Trả về lỗi nếu tham số đầu vào không hợp lệ.</response>
        /// <response code="401">Trả về lỗi nếu không cung cấp token hợp lệ.</response>
        /// <response code="403">Trả về lỗi nếu người dùng không có quyền chủ phòng gym.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy phòng gym.</response>
        [HttpPost(ApiEndPointConstant.GymCourse.CreateGymCourse)]
        [ProducesResponseType(typeof(BaseResponse<CreateGymCourseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateGymCourseResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CreateGymCourseResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateGymCourse([FromBody] CreateGymCourseRequest request)
        {
            var response = await _gymCourseService.CreateNewGymCourse(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách tất cả khóa học thuộc phòng gym của chủ phòng gym.
        /// </summary>
        /// <remarks>
        /// - API này cho phép chủ phòng gym lấy danh sách các khóa học thuộc phòng gym của họ với hỗ trợ phân trang thông qua `page` và `size`.
        /// - Nếu không cung cấp `page`, mặc định là 1. Nếu không cung cấp `size`, mặc định là 10.
        /// - API yêu cầu xác thực (JWT) và người dùng phải có vai trò chủ phòng gym.
        /// - API dùng để quản lý danh sách khóa học của phòng gym (xem thông tin chi tiết, trạng thái).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/course?page=1&amp;size=10
        ///   Authorization: Bearer &lt;JWT_token&gt;
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy danh sách khóa học thành công. Trả về `BaseResponse&lt;IPaginate&lt;GetGymCourseResponse&gt;&gt;` chứa danh sách khóa học và thông tin phân trang.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: `page` hoặc `size` nhỏ hơn 1).
        ///   - `401 Unauthorized`: Không cung cấp token hợp lệ.
        ///   - `403 Forbidden`: Người dùng không có quyền chủ phòng gym.
        ///   - `404 NotFound`: Không tìm thấy khóa học nào thuộc phòng gym của chủ.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Lấy danh sách khóa tập thành công",
        ///     "data": {
        ///       "size": 10,
        ///       "page": 1,
        ///       "total": 2,
        ///       "totalPages": 1,
        ///       "items": [
        ///         {
        ///           "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///           "name": "Gói 1",
        ///           "price": 500000,
        ///           "duration": 30,
        ///           "type": "Normal",
        ///           "description": "Khóa học dành cho người mới bắt đầu",
        ///           "image": "bla bla"
        ///         }
        ///       ]
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="page">Số trang (tùy chọn, mặc định là 1).</param>
        /// <param name="size">Kích thước trang (tùy chọn, mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách khóa học thành công.
        /// - `400 Bad Request`: Tham số đầu vào không hợp lệ.
        /// - `401 Unauthorized`: Không cung cấp token hợp lệ.
        /// - `403 Forbidden`: Người dùng không có quyền chủ phòng gym.
        /// - `404 NotFound`: Không tìm thấy khóa học nào thuộc phòng gym.
        /// </returns>
        /// <response code="200">Trả về danh sách khóa học và thông tin phân trang khi yêu cầu thành công.</response>
        /// <response code="400">Trả về lỗi nếu tham số `page` hoặc `size` không hợp lệ.</response>
        /// <response code="401">Trả về lỗi nếu không cung cấp token hợp lệ.</response>
        /// <response code="403">Trả về lỗi nếu người dùng không có quyền chủ phòng gym.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy khóa học nào thuộc phòng gym.</response>
        [HttpGet(ApiEndPointConstant.GymCourse.GetAllGymCourse)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetGymCourseResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetGymCourseResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetGymCourseResponse>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetGymCourseResponse>>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetGymCourseResponse>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllGymCourse([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _gymCourseService.GetAllGymCourse(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.GymCourse.GetAllPTForGymCourse)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPTForGymCourse([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _gymCourseService.GetAllPTForGymCourse(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

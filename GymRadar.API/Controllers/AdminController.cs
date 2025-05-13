using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response.PT;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Paginate;

namespace GymRadar.API.Controllers
{
    public class AdminController : BaseController<AdminController>
    {
        private readonly IPTService _ptService;
        public AdminController(ILogger<AdminController> logger, IPTService ptService) : base(logger)
        {
            _ptService = ptService;
        }

        /// <summary>
        /// API lấy danh sách huấn luyện viên cá nhân (PT) với phân trang, dành cho Admin.
        /// </summary>
        /// <remarks>
        /// - API này cho phép Admin lấy danh sách PT với hỗ trợ phân trang thông qua các tham số `page` và `size`.
        /// - Nếu không cung cấp `page`, mặc định là 1. Nếu không cung cấp `size`, mặc định là 10.
        /// - API yêu cầu xác thực (JWT) và quyền `Admin` để truy cập.
        /// - API dùng để quản lý danh sách PT (xem thông tin chi tiết, trạng thái).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/admin/get-pt?page=1&amp;size=10
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy danh sách PT thành công. Trả về `BaseResponse&lt;IPaginate&lt;GetPTResponse&gt;&gt;` chứa danh sách PT và thông tin phân trang.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: `page` hoặc `size` nhỏ hơn 1).
        ///   - `401 Unauthorized`: Không cung cấp token hợp lệ.
        ///   - `403 Forbidden`: Người dùng không có quyền Admin.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Lấy danh sách PT thành công",
        ///     "data": {
        ///       "size": 10,
        ///       "page": 1,
        ///       "total": 2,
        ///       "totalPages": 1,
        ///       "items": [
        ///         {
        ///           "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///           "fullName": "Phong Lâm",
        ///           "dob": "1990-05-15",
        ///           "weight": 75.5,
        ///           "height": 175.0,
        ///           "goalTraining": "Tăng cơ",
        ///           "experience": 5,
        ///           "gender": "Male"
        ///         }
        ///       ]
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="page">Số trang (tùy chọn, mặc định là 1).</param>
        /// <param name="size">Kích thước trang (tùy chọn, mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách PT thành công.
        /// - `400 Bad Request`: Tham số đầu vào không hợp lệ.
        /// - `401 Unauthorized`: Không cung cấp token hợp lệ.
        /// - `403 Forbidden`: Người dùng không có quyền Admin.
        /// </returns>
        /// <response code="200">Trả về danh sách PT và thông tin phân trang khi yêu cầu thành công.</response>
        /// <response code="400">Trả về lỗi nếu tham số `page` hoặc `size` không hợp lệ.</response>
        /// <response code="401">Trả về lỗi nếu không cung cấp token hợp lệ.</response>
        /// <response code="403">Trả về lỗi nếu người dùng không có quyền Admin.</response>
        [HttpGet(ApiEndPointConstant.Admin.GetAllPT)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status403Forbidden)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPT([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _ptService.GetAllPTForAdmin(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

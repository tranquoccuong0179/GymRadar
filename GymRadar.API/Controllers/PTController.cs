
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.PT;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.PT;

namespace GymRadar.API.Controllers
{
    public class PTController : BaseController<PTController>
    {
        private readonly IPTService _ptService;
        public PTController(ILogger<PTController> logger, IPTService ptService) : base(logger)
        {
            _ptService = ptService;
        }

        /// <summary>
        /// API tạo mới huấn luyện viên cá nhân (PT) và tài khoản liên kết.
        /// </summary>
        /// <remarks>
        /// - API này cho phép tạo một tài khoản mới cho huấn luyện viên cá nhân (PT) và thông tin PT bằng cách cung cấp thông tin qua `RegisterAccountPTRequest`.
        /// - Tất cả các trường trong `RegisterAccountPTRequest` và `CreateNewPTRequest` đều bắt buộc, trừ các trường tùy chọn (nếu có).
        /// - Trường `gender` trong `createNewPT` chỉ chấp nhận hai giá trị: `"Male"` (Nam) hoặc `"Female"` (Nữ).
        /// - Yêu cầu xác thực (bearer token) để lấy thông tin tài khoản người tạo (chủ phòng gym).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   POST /api/v1/pt
        ///   ```
        /// - Ví dụ nội dung yêu cầu:
        ///   ```json
        ///   {
        ///     "phone": "0363919179",
        ///     "password": "0363919179",
        ///     "email": "pt@gmail.com",
        ///     "createNewPT": {
        ///       "fullName": "PT1",
        ///       "dob": "1990-05-12",
        ///       "weight": 75,
        ///       "height": 175,
        ///       "goalTraining": "Muscle Gain",
        ///       "experience": 3,
        ///       "gender": "Male"
        ///     }
        ///   }
        ///   ```
        /// - Lưu ý về `gender`:
        ///   - Chỉ chấp nhận `"Male"` (Nam) hoặc `"Female"` (Nữ).
        ///   - Ví dụ: `"Male"` hoặc `"Female"`. Các giá trị khác sẽ gây lỗi `400 Bad Request`.
        /// - Kết quả trả về:
        ///   - `200 OK`: Tạo tài khoản và PT thành công. Trả về `BaseResponse&lt;CreateNewPTResponse&gt;` chứa thông tin tài khoản và PT.
        ///   - `400 Bad Request`: Thông tin đầu vào không hợp lệ (ví dụ: thiếu trường bắt buộc, email không hợp lệ, số điện thoại đã tồn tại, hoặc `gender` không phải `"Male"`/`"Female"`).
        ///   - `401 Unauthorized`: Token xác thực không hợp lệ hoặc thiếu.
        ///   - `404 Not Found`: Không tìm thấy tài khoản người tạo hoặc phòng gym liên kết.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "data": {
        ///       "register": {
        ///         "phone": "0363919179",
        ///         "email": "pt@gmail.com"
        ///       },
        ///       "fullName": "PT1",
        ///       "dob": "1990-05-12",
        ///       "weight": 75,
        ///       "height": 175,
        ///       "goalTraining": "Muscle Gain",
        ///       "experience": 3,
        ///       "gender": "Male"
        ///     },
        ///     "message": "Đăng ký PT thành công"
        ///   }
        ///   ```
        /// - Ví dụ phản hồi lỗi (400 Bad Request):
        ///   ```json
        ///   {
        ///     "status": "400",
        ///     "data": null,
        ///     "message": "Giới tính không hợp lệ. Chỉ chấp nhận 'Male' hoặc 'Female'"
        ///   }
        ///   ```
        /// - Ví dụ phản hồi lỗi (401 Unauthorized):
        ///   ```json
        ///   {
        ///     "status": "401",
        ///     "data": null,
        ///     "message": "Không thể xác thực người dùng: Token không hợp lệ hoặc thiếu"
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="request">Thông tin đăng ký tài khoản và PT. Phải bao gồm `phone`, `password`, `email`, và `createNewPT` với các trường bắt buộc. Trường `gender` chỉ chấp nhận `"Male"` hoặc `"Female"`.</param>
        /// <returns>
        /// - `200 OK`: Tạo tài khoản và PT thành công.
        /// - `400 Bad Request`: Thông tin đầu vào không hợp lệ.
        /// - `401 Unauthorized`: Token xác thực không hợp lệ hoặc thiếu.
        /// - `404 Not Found`: Không tìm thấy tài khoản người tạo hoặc phòng gym.
        /// </returns>
        /// <response code="200">Trả về thông tin tài khoản và PT khi tạo thành công.</response>
        /// <response code="400">Trả về lỗi nếu yêu cầu không hợp lệ (thiếu trường, email không hợp lệ, số điện thoại đã tồn tại, hoặc `gender` không hợp lệ).</response>
        /// <response code="401">Trả về lỗi nếu token xác thực không hợp lệ hoặc thiếu.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy tài khoản người tạo hoặc phòng gym.</response>
        [HttpPost(ApiEndPointConstant.PT.CreateNewPT)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewPTResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewPTResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewPTResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewPTResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNewPT([FromBody] RegisterAccountPTRequest request)
        {
            var response = await _ptService.CreateNewPT(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách tất cả huấn luyện viên cá nhân (PT) thuộc phòng gym của chủ phòng gym.
        /// </summary>
        /// <remarks>
        /// - API này cho phép chủ phòng gym lấy danh sách PT thuộc phòng gym của họ với hỗ trợ phân trang thông qua `page` và `size`.
        /// - Nếu không cung cấp `page`, mặc định là 1. Nếu không cung cấp `size`, mặc định là 10.
        /// - API yêu cầu xác thực (JWT) và người dùng phải có vai trò chủ phòng gym.
        /// - API dùng để quản lý danh sách PT của phòng gym (xem thông tin chi tiết, trạng thái).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/pt?page=1&amp;size=10
        ///   Authorization: Bearer &lt;JWT_token&gt;
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy danh sách PT thành công. Trả về `BaseResponse&lt;IPaginate&lt;GetPTResponse&gt;&gt;` chứa danh sách PT và thông tin phân trang.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: `page` hoặc `size` nhỏ hơn 1).
        ///   - `401 Unauthorized`: Không cung cấp token hợp lệ.
        ///   - `403 Forbidden`: Người dùng không phải chủ phòng gym hoặc không có quyền truy cập.
        ///   - `404 NotFound`: Không tìm thấy PT nào thuộc phòng gym của chủ.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Lấy danh sách PT của phòng gym thành công",
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
        /// - `401 Unauthorized`: Không cung cấp token hợp lệ.
        /// - `403 Forbidden`: Người dùng không có quyền chủ phòng gym.
        /// - `404 NotFound`: Không tìm thấy PT nào thuộc phòng gym.
        /// </returns>
        /// <response code="200">Trả về danh sách PT và thông tin phân trang khi yêu cầu thành công.</response>
        /// <response code="401">Trả về lỗi nếu không cung cấp token hợp lệ.</response>
        /// <response code="403">Trả về lỗi nếu người dùng không có quyền chủ phòng gym.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy PT nào thuộc phòng gym.</response>
        [HttpGet(ApiEndPointConstant.PT.GetAllPT)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPT([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _ptService.GetAllPT(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa huấn luyện viên cá nhân (PT) theo ID, dành cho chủ phòng gym.
        /// </summary>
        /// <remarks>
        /// - API này cho phép chủ phòng gym xóa một PT dựa trên `id` cung cấp.
        /// - API yêu cầu xác thực (JWT) và người dùng phải có vai trò chủ phòng gym.
        /// - API chỉ xóa PT thuộc phòng gym của chủ sở hữu.
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   DELETE /api/v1/pt/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///   Authorization: Bearer &lt;JWT_token&gt;
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Xóa PT thành công. Trả về `BaseResponse&lt;bool&gt;` với giá trị `true`.
        ///   - `401 Unauthorized`: Không cung cấp token hợp lệ.
        ///   - `403 Forbidden`: Người dùng không có quyền chủ phòng gym hoặc không sở hữu PT.
        ///   - `404 NotFound`: Không tìm thấy PT với `id` cung cấp.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Xóa PT thành công",
        ///     "data": true
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="id">ID của PT cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa PT thành công.
        /// - `401 Unauthorized`: Không cung cấp token hợp lệ.
        /// - `403 Forbidden`: Người dùng không có quyền chủ phòng gym.
        /// - `404 NotFound`: Không tìm thấy PT.
        /// </returns>
        /// <response code="200">Trả về `true` khi xóa PT thành công.</response>
        /// <response code="401">Trả về `false` lỗi nếu không cung cấp token hợp lệ.</response>
        /// <response code="403">Trả về `false lỗi nếu người dùng không có quyền chủ phòng gym.</response>
        /// <response code="404">Trả về `false lỗi nếu không tìm thấy PT.</response>
        [HttpDelete(ApiEndPointConstant.PT.DeletePT)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeletePT([FromRoute] Guid id)
        {
            var response = await _ptService.DeletePT(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cho phép huấn luyện viên cá nhân (PT) tự cập nhật thông tin hồ sơ cá nhân.
        /// </summary>
        /// <remarks>
        /// - API này cho phép PT cập nhật thông tin cá nhân, bao gồm họ tên, ngày sinh, cân nặng, chiều cao, mục tiêu tập luyện, kinh nghiệm, và giới tính.
        /// - API yêu cầu xác thực (JWT) và chỉ dành cho người dùng có vai trò PT.
        /// - Các trường có giá trị `null` trong yêu cầu sẽ giữ nguyên giá trị cũ; các trường có giá trị mới sẽ được thay thế.
        /// - Thông tin được cập nhật dựa trên tài khoản PT đang đăng nhập (lấy từ token).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   PUT /api/v1/pt
        ///   Authorization: Bearer &lt;JWT_token&gt;
        ///   Content-Type: application/json
        ///   {
        ///     "fullName": "PT1",
        ///     "dob": "1988-03-20",
        ///     "weight": 60.0,
        ///     "height": 165.0,
        ///     "goalTraining": "Tăng cơ",
        ///     "experience": 7,
        ///     "gender": "Female"
        ///   }
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Cập nhật hồ sơ PT thành công. Trả về `BaseResponse&lt;GetPTResponse&gt;` chứa thông tin PT đã cập nhật.
        ///   - `401 Unauthorized`: Không cung cấp token hợp lệ.
        ///   - `404 NotFound`: Không tìm thấy hồ sơ PT.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Cập nhật hồ sơ PT thành công",
        ///     "data": {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "fullName": "PT1",
        ///       "dob": "1988-03-20",
        ///       "weight": 60.0,
        ///       "height": 165.0,
        ///       "goalTraining": "Tăng cơ",
        ///       "experience": 7,
        ///       "gender": "Female"
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="request">Thông tin cần cập nhật cho hồ sơ PT, các trường `null` giữ nguyên giá trị cũ.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật hồ sơ PT thành công.
        /// - `401 Unauthorized`: Không cung cấp token hợp lệ.
        /// - `404 NotFound`: Không tìm thấy hồ sơ PT.
        /// </returns>
        /// <response code="200">Trả về thông tin PT đã cập nhật khi yêu cầu thành công.</response>
        /// <response code="401">Trả về lỗi nếu không cung cấp token hợp lệ.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy hồ sơ PT.</response>
        [HttpPut(ApiEndPointConstant.PT.UpdatePT)]
        [ProducesResponseType(typeof(BaseResponse<GetPTResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetPTResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdatePT([FromBody] UpdatePTRequest request)
        {
            var response = await _ptService.UpdatePT(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin chi tiết huấn luyện viên cá nhân (PT) theo ID.
        /// </summary>
        /// <remarks>
        /// - API này trả về thông tin chi tiết của PT dựa trên `id` cung cấp, bao gồm họ tên, ngày sinh, cân nặng, chiều cao, mục tiêu tập luyện, kinh nghiệm, và giới tính.
        /// - API không yêu cầu xác thực (công khai cho người dùng).
        /// - API dùng để người dùng xem chi tiết PT (ví dụ: để chọn PT phù hợp).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/pt/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy thông tin PT thành công. Trả về `BaseResponse&lt;GetPTResponse&gt;` chứa thông tin PT.
        ///   - `404 NotFound`: Không tìm thấy PT với `id` cung cấp.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Lấy thông tin PT thành công",
        ///     "data": {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "fullName": "PT1",
        ///       "dob": "1988-03-20",
        ///       "weight": 60.0,
        ///       "height": 165.0,
        ///       "goalTraining": "Tăng cơ",
        ///       "experience": 7,
        ///       "gender": "Female"
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="id">ID của PT cần lấy thông tin.</param>
        /// <returns>
        /// - `200 OK`: Lấy thông tin PT thành công.
        /// - `404 NotFound`: Không tìm thấy PT.
        /// </returns>
        /// <response code="200">Trả về thông tin chi tiết PT khi yêu cầu thành công.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy PT.</response>
        [HttpGet(ApiEndPointConstant.PT.GetPT)]
        [ProducesResponseType(typeof(BaseResponse<GetPTResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetPTResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetPT([FromRoute] Guid id)
        {
            var response = await _ptService.GetPT(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

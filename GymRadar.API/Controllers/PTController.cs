
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.PT;

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
    }
}

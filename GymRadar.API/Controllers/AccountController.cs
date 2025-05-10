
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Request.User;

namespace GymRadar.API.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;
        public AccountController(ILogger<AccountController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }


        /// <summary>
        /// API đăng ký tài khoản mới cho người dùng.
        /// </summary>
        /// <remarks>
        /// - API này cho phép tạo tài khoản mới bằng cách cung cấp thông tin qua `RegisterRequest`.
        /// - Tất cả các trường trong `RegisterRequest` đều bắt buộc.
        /// - Không yêu cầu xác thực (public API).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   POST /api/v1/account
        ///   ```
        /// - Ví dụ nội dung yêu cầu:
        ///   ```json
        ///   {
        ///     "phone": "0363919179",
        ///     "email": "cuongtq@gmail.com",
        ///     "password": "0363919179",
        ///    }
        ///    ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Cập nhật tài khoản thành công. Trả về `BaseResponse&lt;RegisterResponse&gt;` chứa thông tin tài khoản đã được cập nhật.
        ///   - `400 Bad Request`: Thông tin đầu vào không hợp lệ (ví dụ: thiếu trường hoặc dữ liệu không đúng định dạng).
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "data": {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "email": "cuongtq@gmail.com",
        ///       "phone": "0363919179",
        ///     "message": "Đăng kí tài khoản thành công"
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="request">Thông tin đăng ký của người dùng. Phải bao gồm `phone`, `email`, `password`.</param>
        /// <returns>
        /// - `200 OK`: Đăng ký thành công.  
        /// - `400 Bad Request`: Thông tin đầu vào không hợp lệ (ví dụ: thiếu trường, email không hợp lệ).
        /// </returns>
        /// <response code="200">Trả về kết quả đăng ký khi tài khoản được tạo thành công.</response>
        /// <response code="400">Trả về lỗi nếu yêu cầu không hợp lệ.</response>
        [HttpPost(ApiEndPointConstant.Account.RegisterAccount)]
        [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNewAccount([FromBody] RegisterRequest request)
        {
            var response = await _accountService.RegisterAccount(request);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

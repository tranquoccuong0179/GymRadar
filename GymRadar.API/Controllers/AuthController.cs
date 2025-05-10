
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Request.Auth;
using GymRadar.Model.Payload.Response.Auth;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymRadar.API.Controllers
{
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthService _authService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger)
        {
            _authService = authService;
        }

        /// <summary>
        /// API xác thực người dùng và trả về token.
        /// </summary>
        /// <remarks>
        /// - API này cho phép người dùng xác thực bằng thông tin đăng nhập thông qua `AuthenticationRequest`.
        /// - Không yêu cầu xác thực (public API).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   POST /api/v1/auth
        ///   ```
        /// - Ví dụ nội dung yêu cầu:
        ///   ```json
        ///   {
        ///     "phone": "0363919179",
        ///     "password": "0363919179",
        ///    }
        /// - Kết quả trả về:
        ///   - `200 OK`: Xác thực thành công. Trả về `BaseResponse&lt;AuthenticationResponse&gt;` chứa token và thông tin người dùng.
        ///   - `400 Bad Request`: Thông tin đầu vào không hợp lệ (ví dụ: thiếu trường, phone hoặc mật khẩu không đúng).
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "data": {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "phone": "0363919179",
        ///       "role": "USER",
        ///       "accessToken": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAx..."
        ///     },
        ///     "message": "Đăng nhập thành công"
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="request">Thông tin xác thực của người dùng. Phải bao gồm `phone` và `password`.</param>
        /// <returns>
        /// - `200 OK`: Xác thực thành công. Trả về `BaseResponse&lt;AuthenticationResponse&gt;` chứa token và thông tin người dùng.
        /// - `400 Bad Request`: Thông tin đầu vào không hợp lệ (ví dụ: thiếu trường, phone hoặc mật khẩu không đúng).
        /// </returns>
        /// <response code="200">Trả về token và thông tin người dùng khi xác thực thành công.</response>
        /// <response code="400">Trả về lỗi nếu thông tin xác thực không hợp lệ.</response>
        [HttpPost(ApiEndPointConstant.Authentication.Auth)]
        [ProducesResponseType(typeof(BaseResponse<AuthenticationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AuthenticationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            var response = await _authService.Authenticate(request);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

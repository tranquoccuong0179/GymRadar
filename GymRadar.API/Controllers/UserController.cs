
using GymRadar.API.Constant;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.PT;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymRadar.API.Controllers
{
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger, IUserService userService) : base(logger)
        {
            _userService = userService;
        }

        /// <summary>
        /// API cho phép người dùng (User) cập nhật thông tin hồ sơ cá nhân.
        /// </summary>
        /// <remarks>
        /// - API này cho phép User cập nhật thông tin cá nhân, bao gồm họ tên, ngày sinh, cân nặng, chiều cao, giới tính, và địa chỉ.
        /// - API yêu cầu xác thực (JWT) và chỉ dành cho người dùng có vai trò User.
        /// - Các trường có giá trị `null` trong yêu cầu sẽ giữ nguyên giá trị cũ; các trường có giá trị mới sẽ được thay thế.
        /// - Thông tin được cập nhật dựa trên tài khoản User đang đăng nhập (lấy từ token).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   PUT /api/v1/user
        ///   Authorization: Bearer &lt;JWT_token&gt;
        ///   Content-Type: application/json
        ///   {
        ///     "fullName": "User1",
        ///     "dob": "1990-05-15",
        ///     "weight": 78.0,
        ///     "height": 178.0,
        ///     "gender": "Male",
        ///     "address": "123 Lê Văn Việt"
        ///   }
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Cập nhật hồ sơ thành công. Trả về `BaseResponse&lt;GetUserResponse&gt;` chứa thông tin hồ sơ đã cập nhật.
        ///   - `401 Unauthorized`: Không cung cấp token hợp lệ.
        ///   - `404 NotFound`: Không tìm thấy hồ sơ của người dùng.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Cập nhật hồ sơ thành công",
        ///     "data": {
        ///       "fullName": "User1",
        ///       "dob": "1990-05-15",
        ///       "weight": 78.0,
        ///       "height": 178.0,
        ///       "gender": "Male",
        ///       "address": "123 Lê Văn Việt"
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="request">Thông tin cần cập nhật cho hồ sơ người dùng, các trường `null` giữ nguyên giá trị cũ.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật hồ sơ thành công.
        /// - `401 Unauthorized`: Không cung cấp token hợp lệ.
        /// - `404 NotFound`: Không tìm thấy hồ sơ.
        /// </returns>
        /// <response code="200">Trả về thông tin hồ sơ đã cập nhật khi yêu cầu thành công.</response>
        /// <response code="401">Trả về lỗi nếu không cung cấp token hợp lệ.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy hồ sơ.</response>
        [HttpPut(ApiEndPointConstant.User.UpdateUser)]
        [ProducesResponseType(typeof(BaseResponse<GetUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetUserResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdatePT([FromBody] UpdateUserRequest request)
        {
            var response = await _userService.UpdateUser(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.User.GetAllUser)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetUserResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllUser([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _userService.GetAllUser(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.User.DeleteUser)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var response = await _userService.DeleteUser(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

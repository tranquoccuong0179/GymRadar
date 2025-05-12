
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.Gym;
using GymRadar.Model.Payload.Request.User;

namespace GymRadar.API.Controllers
{
    public class GymController : BaseController<GymController>
    {
        private readonly IGymService _gymService;
        public GymController(ILogger<GymController> logger, IGymService gymService) : base(logger)
        {
            _gymService = gymService;
        }

        /// <summary>
        /// API tạo mới phòng gym và tài khoản liên kết.
        /// </summary>
        /// <remarks>
        /// - API này cho phép tạo tài khoản mới cho phòng gym và thông tin phòng gym bằng cách cung cấp thông tin qua `RegisterAccountGym`.
        /// - Tất cả các trường trong `RegisterAccountGym` và `CreateNewGymRequest` đều bắt buộc (trừ các trường tùy chọn như `HotResearch`).
        /// - Không yêu cầu xác thực (public API).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   POST /api/v1/gym
        ///   ```
        /// - Ví dụ nội dung yêu cầu:
        ///   ```json
        ///   {
        ///     "phone": "0363919179",
        ///     "password": "0363919179",
        ///     "email": "gym@gmail.com",
        ///     "createNewGym": {
        ///       "gymName": "GymRadar",
        ///       "since": 2025,
        ///       "address": "Nhà Phong Lâm",
        ///       "representName": "Phong Lâm",
        ///       "taxCode": "123456789",
        ///       "longitude": 105.8342,
        ///       "latitude": 21.0278,
        ///       "qrcode": "QR123456",
        ///     }
        ///   }
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Tạo tài khoản và phòng gym thành công. Trả về `BaseResponse&lt;CreateNewGymResponse&gt;` chứa thông tin tài khoản và phòng gym.
        ///   - `400 Bad Request`: Thông tin đầu vào không hợp lệ (ví dụ: thiếu trường, email không hợp lệ, hoặc số điện thoại đã tồn tại).
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "data": {
        ///       "register": {
        ///         "phone": "0363919179",
        ///         "email": "gym@gmail.com"
        ///       },
        ///       "gymName": "GymRadar",
        ///       "since": 2020,
        ///       "address": "Nhà Phong Lâm",
        ///       "representName": "Phong Lâm",
        ///       "taxCode": "123456789",
        ///       "longitude": 105.8342,
        ///       "latitude": 21.0278,
        ///       "qrcode": "QR123456",
        ///       "hotResearch": false
        ///     },
        ///     "message": "Đăng ký phòng gym thành công"
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="request">Thông tin đăng ký tài khoản và phòng gym. Phải bao gồm `phone`, `password`, `email`, và `createNewGym` với các trường bắt buộc.</param>
        /// <returns>
        /// - `200 OK`: Tạo tài khoản và phòng gym thành công.
        /// - `400 Bad Request`: Thông tin đầu vào không hợp lệ (ví dụ: thiếu trường, email không hợp lệ, hoặc số điện thoại đã tồn tại).
        /// </returns>
        /// <response code="200">Trả về kết quả khi tài khoản và phòng gym được tạo thành công.</response>
        /// <response code="400">Trả về lỗi nếu yêu cầu không hợp lệ.</response>
        [HttpPost(ApiEndPointConstant.Gym.CreateNewGym)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewGymResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateNewGymResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNewGym([FromBody] RegisterAccountGymRequest request)
        {
            var response = await _gymService.CreateNewGym(request);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

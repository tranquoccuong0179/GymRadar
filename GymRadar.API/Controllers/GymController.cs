
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.Gym;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Response.PT;
using GymRadar.Service.Implement;
using GymRadar.Model.Payload.Response.GymCourse;

namespace GymRadar.API.Controllers
{
    public class GymController : BaseController<GymController>
    {
        private readonly IGymService _gymService;
        private readonly IPTService _ptService;
        private readonly IGymCourseService _courseService;
        public GymController(ILogger<GymController> logger, IGymService gymService, IPTService ptService, IGymCourseService courseService) : base(logger)
        {
            _gymService = gymService;
            _ptService = ptService;
            _courseService = courseService;
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

        /// <summary>
        /// API lấy danh sách phòng gym với phân trang.
        /// </summary>
        /// <remarks>
        /// - API này cho phép lấy danh sách phòng gym với hỗ trợ phân trang thông qua các tham số `page` và `size`.
        /// - Nếu không cung cấp `page`, mặc định là 1. Nếu không cung cấp `size`, mặc định là 10.
        /// - API không yêu cầu xác thực (công khai cho người dùng).
        /// - API dùng cho người dùng tìm kiếm phòng gym.
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/gym?page=1&amp;size=10
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy danh sách phòng gym thành công. Trả về `BaseResponse&lt;IPaginate&lt;GetGymResponse&gt;&gt;` chứa danh sách phòng gym và thông tin phân trang.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: `page` hoặc `size` nhỏ hơn 1).
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Lấy danh sách các phòng gym thành công",
        ///     "data": {
        ///       "size": 10,
        ///       "page": 1,
        ///       "total": 2,
        ///       "totalPages": 1,
        ///       "items": [
        ///         {
        ///           "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///           "gymName": "Gym PL",
        ///           "since": 2020,
        ///           "address": "FPT",
        ///           "representName": "PL",
        ///           "taxCode": "1234567890",
        ///           "longitude": 105.801,
        ///           "latitude": 21.013,
        ///           "qrcode": "qrcode123456",
        ///           "hotResearch": true
        ///         }
        ///       ]
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="page">Số trang (tùy chọn, mặc định là 1).</param>
        /// <param name="size">Kích thước trang (tùy chọn, mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách phòng gym thành công.
        /// - `400 Bad Request`: Tham số đầu vào không hợp lệ.
        /// </returns>
        /// <response code="200">Trả về danh sách phòng gym và thông tin phân trang khi yêu cầu thành công.</response>
        /// <response code="400">Trả về lỗi nếu tham số `page` hoặc `size` không hợp lệ.</response>
        [HttpGet(ApiEndPointConstant.Gym.GetAllGym)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetGymResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllGym([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _gymService.GetAllGym(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin chi tiết một phòng gym theo ID.
        /// </summary>
        /// <remarks>
        /// - API này trả về thông tin chi tiết của phòng gym dựa trên `id` cung cấp.
        /// - API không yêu cầu xác thực (công khai cho người dùng).
        /// - API dùng cho người dùng xem chi tiết phòng gym.
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/gym/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy thông tin phòng gym thành công. Trả về `BaseResponse&lt;GetGymResponse&gt;` chứa thông tin phòng gym.
        ///   - `404 NotFound`: Không tìm thấy phòng gym với `id` cung cấp.
        /// - Ví dụ phản hồi thành công (200 OK):
        ///   ```json
        ///   {
        ///     "status": "200",
        ///     "message": "Lấy thông tin phòng gym thành công",
        ///     "data": {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "gymName": "GymPL",
        ///       "since": 2020,
        ///       "address": "FPT",
        ///       "representName": "PL",
        ///       "taxCode": "1234567890",
        ///       "longitude": 105.801,
        ///       "latitude": 21.013,
        ///       "qrcode": "qrcode123456",
        ///       "hotResearch": true
        ///     }
        ///   }
        ///   ```
        /// </remarks>
        /// <param name="id">ID của phòng gym cần lấy thông tin.</param>
        /// <returns>
        /// - `200 OK`: Lấy thông tin phòng gym thành công.
        /// - `404 NotFound`: Không tìm thấy phòng gym này.
        /// </returns>
        /// <response code="200">Trả về thông tin chi tiết phòng gym khi yêu cầu thành công.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy phòng gym.</response>
        [HttpGet(ApiEndPointConstant.Gym.GetGym)]
        [ProducesResponseType(typeof(BaseResponse<GetGymResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetGymResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetGym([FromRoute] Guid id)
        {
            var response = await _gymService.GetGymById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách huấn luyện viên cá nhân (PT) của một phòng gym cho người dùng.
        /// </summary>
        /// <remarks>
        /// - API này cho phép người dùng lấy danh sách PT thuộc phòng gym được chỉ định bởi `id` (gymId) với hỗ trợ phân trang thông qua `page` và `size`.
        /// - Nếu không cung cấp `page`, mặc định là 1. Nếu không cung cấp `size`, mặc định là 10.
        /// - API không yêu cầu xác thực (công khai cho người dùng).
        /// - API dùng để người dùng xem danh sách PT của một phòng gym cụ thể.
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/v1/gym/3fa85f64-5717-4562-b3fc-2c963f66afa6/pts?page=1&amp;size=10
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy danh sách PT thành công. Trả về `BaseResponse&lt;IPaginate&lt;GetPTResponse&gt;&gt;` chứa danh sách PT và thông tin phân trang.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: `page` hoặc `size` nhỏ hơn 1).
        ///   - `404 NotFound`: Không tìm thấy phòng gym với `id` cung cấp hoặc không có PT nào thuộc phòng gym.
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
        ///           "fullName": "Phong Lâm,
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
        /// <param name="id">ID của phòng gym để lấy danh sách PT (gymId).</param>
        /// <param name="page">Số trang (tùy chọn, mặc định là 1).</param>
        /// <param name="size">Kích thước trang (tùy chọn, mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách PT thành công.
        /// - `404 NotFound`: Không tìm thấy phòng gym hoặc PT.
        /// </returns>
        /// <response code="200">Trả về danh sách PT và thông tin phân trang khi yêu cầu thành công.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy phòng gym hoặc PT.</response>
        [HttpGet(ApiEndPointConstant.Gym.GetAllPT)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPTResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPT([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _ptService.GetAllPTForUser(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách các khóa học của một phòng gym cho người dùng.
        /// </summary>
        /// <remarks>
        /// - API này cho phép người dùng lấy danh sách các khóa học thuộc phòng gym được chỉ định bởi `id` (GymId) với hỗ trợ phân trang thông qua `page` và `size`.
        /// - Nếu không cung cấp `page`, mặc định là 1. Nếu không cung cấp `size`, mặc định là 10.
        /// - API không yêu cầu xác thực (công khai cho người dùng).
        /// - API dùng để người dùng xem các khóa học của một phòng gym cụ thể (ví dụ: Yoga, Gym với PT).
        /// - Ví dụ yêu cầu:
        ///   ```
        ///   GET /api/gym/3fa85f64-5717-4562-b3fc-2c963f66afa6/courses?page=1&amp;size=10
        ///   ```
        /// - Kết quả trả về:
        ///   - `200 OK`: Lấy danh sách khóa học thành công. Trả về `BaseResponse&lt;IPaginate&lt;GetGymCourseResponse&gt;&gt;` chứa danh sách khóa học và thông tin phân trang.
        ///   - `400 Bad Request`: Tham số đầu vào không hợp lệ (ví dụ: `page` hoặc `size` nhỏ hơn 1).
        ///   - `404 NotFound`: Không tìm thấy phòng gym với `id` cung cấp hoặc không có khóa học nào thuộc phòng gym.
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
        /// <param name="id">ID của phòng gym để lấy danh sách khóa học (GymId).</param>
        /// <param name="page">Số trang (tùy chọn, mặc định là 1).</param>
        /// <param name="size">Kích thước trang (tùy chọn, mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách khóa học thành công.
        /// - `400 Bad Request`: Tham số đầu vào không hợp lệ.
        /// - `404 NotFound`: Không tìm thấy phòng gym hoặc khóa học.
        /// </returns>
        /// <response code="200">Trả về danh sách khóa học và thông tin phân trang khi yêu cầu thành công.</response>
        /// <response code="400">Trả về lỗi nếu tham số `page` hoặc `size` không hợp lệ.</response>
        /// <response code="404">Trả về lỗi nếu không tìm thấy phòng gym.</response>
        [HttpGet(ApiEndPointConstant.Gym.GetAllCourse)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetGymCourseResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllCourse([FromRoute]Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _courseService.GetAllGymCourseForUser(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

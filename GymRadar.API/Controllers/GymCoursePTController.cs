
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using GymRadar.Model.Payload.Response.GymCoursePT;
using GymRadar.Model.Payload.Request.GymCoursePT;
using GymRadar.Model.Paginate;

namespace GymRadar.API.Controllers
{
    public class GymCoursePTController : BaseController<GymCoursePTController>
    {
        private readonly IGymCoursePTService _gymCoursePTService;
        public GymCoursePTController(ILogger<GymCoursePTController> logger, IGymCoursePTService gymCoursePTService) : base(logger)
        {
            _gymCoursePTService = gymCoursePTService;
        }

        [HttpPost(ApiEndPointConstant.GymCoursePT.CreateGymCoursePT)]
        [ProducesResponseType(typeof(BaseResponse<CreateGymCoursePTResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateGymCoursePTResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<CreateGymCoursePTResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateGymCoursePT([FromBody] CreateGymCoursePTRequest request)
        {
            var response = await _gymCoursePTService.CreateGymCoursePT(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.GymCoursePT.GetAllGymCoursePT)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<CreateGymCoursePTResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<CreateGymCoursePTResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllGymCoursePT([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size) 
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _gymCoursePTService.GetAllGymCoursePT(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.GymCoursePT.GetGymCoursePT)]
        [ProducesResponseType(typeof(BaseResponse<GetGymCoursePTResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetGymCoursePTResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetGymCoursePT([FromRoute] Guid id)
        {
            var response = await _gymCoursePTService.GetGymCoursePT(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

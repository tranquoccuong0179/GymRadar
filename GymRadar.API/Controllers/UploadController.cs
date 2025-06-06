
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymRadar.API.Controllers
{
    public class UploadController : BaseController<UploadController>
    {
        private readonly IUploadService _uploadService;
        public UploadController(ILogger<UploadController> logger, IUploadService uploadService) : base(logger)
        {
            _uploadService = uploadService;
        }

        [HttpPost(ApiEndPointConstant.Upload.UploadImage)]
        public async Task<string> UploadImage(IFormFile file)
        {
            var response = await _uploadService.UploadImage(file);
            return response;
        }
    }
}

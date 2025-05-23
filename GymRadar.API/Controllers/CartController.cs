
using GymRadar.API.Constant;
using GymRadar.Model.Payload.Request.Cart;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace GymRadar.API.Controllers
{
    public class CartController : BaseController<CartController>
    {
        private readonly ICartService _cartService;
        public CartController(ILogger<CartController> logger, ICartService cartService) : base(logger)
        {
            _cartService = cartService;
        }

        [HttpPost(ApiEndPointConstant.Cart.CreateQR)]
        [ProducesResponseType(typeof(BaseResponse<CreatePaymentResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreatePaymentResult>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateQR([FromBody] CreateQRRequest request)
        {
            var response = await _cartService.CreatePaymentUrlRegisterCreator(request);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}


using Azure.Core;
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



        [HttpPost(ApiEndPointConstant.Cart.CreateQRNotPT)]
        [ProducesResponseType(typeof(BaseResponse<CreatePaymentResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreatePaymentResult>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateQRNotPT([FromBody] CreateQRNotPTRequest request)
        {
            var response = await _cartService.CreatePaymentNotPT(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPost(ApiEndPointConstant.Cart.CreateQRPremium)]
        [ProducesResponseType(typeof(BaseResponse<CreatePaymentResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreatePaymentResult>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateQRPremium([FromBody] CreateQRPremiumRequest request)
        {
            var response = await _cartService.CreatePaymentUrlPremium(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Cart.ReturnUrl)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> ReturnUrl()
        {
            string responseCode = Request.Query["code"].ToString();
            string id = Request.Query["id"].ToString();
            string cancel = Request.Query["cancel"].ToString();
            string status = Request.Query["status"].ToString();
            string orderCode = Request.Query["orderCode"];

            if (!long.TryParse(orderCode, out long parsedOrderCode))
            {
                //var errorUrl = $"exp://192.168.1.223:8081/--/order-process?code=01";
                var errorUrl = $"exp://172.20.10.2:8081/--/order-process?code=01";
                return Redirect(errorUrl);
            }

            var response = await _cartService.HandlePaymentCallback(id, parsedOrderCode);
            //var redirectUrl = response.status == StatusCodes.Status200OK.ToString()
            //    ? $"exp://192.168.1.223:8081/--/order-process?code=00&message&orderCode={parsedOrderCode}"
            //    : $"exp://192.168.1.223:8081/--/order-process?code=01";
            var redirectUrl = response.status == StatusCodes.Status200OK.ToString()
                ? $"exp://172.20.10.2:8081/--/order-process?code=00&message&orderCode={parsedOrderCode}"
                : $"exp://172.20.10.2:8081/--/order-process?code=01";

            return Redirect(redirectUrl);
        }

        [HttpGet(ApiEndPointConstant.Cart.GetPaymentStatus)]
        public async Task<IActionResult> GetPaymentStatus([FromQuery] long orderCode)
        {
            var response = await _cartService.GetPaymentStatus(orderCode);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

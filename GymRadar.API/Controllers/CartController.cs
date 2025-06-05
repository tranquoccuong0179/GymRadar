
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
                return BadRequest(new
                {
                    code = "01",
                    message = "Định dạng orderCode không hợp lệ"
                });
            }

            var response = await _cartService.HandlePaymentCallback(id, parsedOrderCode);
            if (response.status == StatusCodes.Status200OK.ToString())
            {
                return Ok(new
                {
                    code = "00",
                    message = response.message,
                    orderCode = parsedOrderCode
                });
            }
            else
            {
                return BadRequest(new
                {
                    code = "01",
                    message = response.message
                });
            }
        }

        [HttpGet(ApiEndPointConstant.Cart.GetPaymentStatus)]
        public async Task<IActionResult> GetPaymentStatus([FromQuery] long orderCode)
        {
            var response = await _cartService.GetPaymentStatus(orderCode);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

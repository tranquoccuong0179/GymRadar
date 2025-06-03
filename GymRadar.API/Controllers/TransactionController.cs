
using GymRadar.API.Constant;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Response.Transaction;
using GymRadar.Model.Payload.Response;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymRadar.API.Controllers
{
    public class TransactionController : BaseController<TransactionController>
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService) : base(logger)
        {
            _transactionService = transactionService;
        }

        [HttpGet(ApiEndPointConstant.Transaction.GetAllTransaction)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetTransactionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetTransactionResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllTransactionForUser([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _transactionService.GetAllTransactionForUser(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Transaction.GetAllTransactionForGym)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetTransactionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetTransactionResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllTransactionForGym([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _transactionService.GetAllTransactionForGym(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}

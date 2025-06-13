using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Transaction;

namespace GymRadar.Service.Interface
{
    public interface ITransactionService
    {
        Task<BaseResponse<IPaginate<GetTransactionResponse>>> GetAllTransactionForUser(int page, int size);
        Task<BaseResponse<IPaginate<GetTransactionResponse>>> GetAllTransactionForAdmin(int page, int size);
        Task<BaseResponse<IPaginate<GetTransactionResponse>>> GetAllTransactionForGym(int page, int size);
        Task<BaseResponse<GetTransactionResponse>> GetTransactionById(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Request.Cart;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Cart;
using Net.payOS.Types;

namespace GymRadar.Service.Interface
{
    public interface ICartService
    {
        Task<BaseResponse<CreatePaymentResult>> CreatePaymentUrlRegisterCreator(CreateQRRequest request);
        Task<BaseResponse<bool>> HandlePaymentCallback(string paymentLinkId, long orderCode);
        Task<BaseResponse<TransactionResponse>> GetPaymentStatus(long orderCode);
    }
}

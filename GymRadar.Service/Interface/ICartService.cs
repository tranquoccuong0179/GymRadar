using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Request.Cart;
using GymRadar.Model.Payload.Response;
using Net.payOS.Types;

namespace GymRadar.Service.Interface
{
    public interface ICartService
    {
        Task<BaseResponse<CreatePaymentResult>> CreatePaymentUrlRegisterCreator(List<CreateQRRequest> request);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.Cart
{
    public class TransactionResponse
    {
        public string? Status {  get; set; }
        public long? OrderCode { get; set; }
        public double? Amount { get; set; }
        public string? Description { get; set; }
    }
}

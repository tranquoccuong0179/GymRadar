using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.Cart
{
    public class ExtendedPaymentInfo
    {
        public int Amount { get; set; }
        public string Description { get; set; }
        public string BuyerName { get; set; }
        public string BuyerPhone { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerAddress { get; set; }
        public string Status { get; set; }
        public Guid ProductId { get; set; }
    }
}

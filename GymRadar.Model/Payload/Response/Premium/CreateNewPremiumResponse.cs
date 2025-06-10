using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.Premium
{
    public class CreateNewPremiumResponse
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }
    }
}

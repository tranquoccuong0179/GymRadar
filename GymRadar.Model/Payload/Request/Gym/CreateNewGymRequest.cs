using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Request.Gym
{
    public class CreateNewGymRequest
    {
        public string GymName { get; set; } = null!;

        public int Since { get; set; }

        public string Address { get; set; } = null!;

        public string RepresentName { get; set; } = null!;

        public string TaxCode { get; set; } = null!;

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Qrcode { get; set; } = null!;

    }
}

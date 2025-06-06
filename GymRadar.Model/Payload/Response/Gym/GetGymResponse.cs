using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Response.GymImage;

namespace GymRadar.Model.Payload.Response.Gym
{
    public class GetGymResponse
    {
        public Guid Id { get; set; }

        public string? GymName { get; set; }

        public string? MainImage { get; set; }

        public int? Since { get; set; }

        public string? Address { get; set; }

        public string? RepresentName { get; set; }

        public string? TaxCode { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string? Qrcode { get; set; }

        public bool? HotResearch { get; set; }

        public List<GetGymImageResponse>? Images { get; set; }
    }
}

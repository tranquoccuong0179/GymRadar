using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.PT
{
    public class GetPTResponse
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public string? Phone {  get; set; }

        public string? Email { get; set; }

        public DateOnly? Dob { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public string? GoalTraining { get; set; }

        public int? Experience { get; set; }

        public string? Gender { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Enum;
using GymRadar.Model.Payload.Response.User;

namespace GymRadar.Model.Payload.Response.PT
{
    public class CreateNewPTResponse
    {
        public RegisterResponse? Register { get; set; }
        public string? FullName { get; set; }

        public DateOnly? Dob { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public string? GoalTraining { get; set; }

        public int? Experience { get; set; }

        public string? Gender { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Enum;

namespace GymRadar.Model.Payload.Request.User
{
    public class UpdateUserRequest
    {
        public string? FullName { get; set; }

        public DateOnly? Dob { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public GenderEnum? Gender { get; set; }

        public string? Address { get; set; }
    }
}

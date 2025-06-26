using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.User
{
    public class GetUserResponse
    {
        public Guid? Id { get; set; }

        public string? FullName { get; set; }

        public DateOnly? Dob { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }
    }
}

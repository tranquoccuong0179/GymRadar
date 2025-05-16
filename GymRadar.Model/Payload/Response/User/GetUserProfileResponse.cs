using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.User
{
    public class GetUserProfileResponse
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone {  get; set; }
        public DateOnly? DOB { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
    }
}

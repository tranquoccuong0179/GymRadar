using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Request.Auth
{
    public class AuthenticationRequest
    {
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

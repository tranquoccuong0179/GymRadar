using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Request.PT;

namespace GymRadar.Model.Payload.Request.User
{
    public class RegisterAccountPTRequest
    {
        public string Phone { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public CreateNewPTRequest CreateNewPT { get; set; } = null!;
    }
}

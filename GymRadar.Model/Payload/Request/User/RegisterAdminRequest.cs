using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Request.User
{
    public class RegisterAdminRequest
    {
        public string Phone { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;


    }
}

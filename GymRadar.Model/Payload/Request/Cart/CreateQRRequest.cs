using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Request.Cart
{
    public class CreateQRRequest
    {
        public Guid GymCourseId { get; set; }
        public Guid PTId { get; set; }
    }
}

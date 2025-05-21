using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.GymCoursePT
{
    public class CreateGymCoursePTResponse
    {
        public Guid? Ptid { get; set; }

        public Guid? GymCourseId { get; set; }

        public int? Session { get; set; }
    }
}

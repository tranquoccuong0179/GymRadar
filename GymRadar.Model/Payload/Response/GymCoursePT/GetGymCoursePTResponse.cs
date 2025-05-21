using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Response.GymCourse;
using GymRadar.Model.Payload.Response.PT;

namespace GymRadar.Model.Payload.Response.GymCoursePT
{
    public class GetGymCoursePTResponse
    {
        public Guid Id { get; set; }
        public GetPTResponse? PT { get; set; }
        public GetGymCourseResponse? Course {  get; set; }
        public int Session {  get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.GymCourse
{
    public class GetGymCourseResponse
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public double Price { get; set; }

        public long Duration { get; set; }

        public string? Type { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }
    }
}

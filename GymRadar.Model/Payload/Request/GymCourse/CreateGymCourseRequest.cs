using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Enum;

namespace GymRadar.Model.Payload.Request.GymCourse
{
    public class CreateGymCourseRequest
    {
        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public long Duration { get; set; }

        public TypeCourseEnum Type { get; set; }

        public string Description { get; set; } = null!;

        public string Image { get; set; } = null!;
    }
}

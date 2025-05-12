using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Enum;

namespace GymRadar.Model.Payload.Request.PT
{
    public class CreateNewPTRequest
    {
        public string FullName { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public string GoalTraining { get; set; } = null!;

        public int Experience { get; set; }

        public GenderEnum Gender { get; set; }
    }
}

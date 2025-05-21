using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Response.Slot;

namespace GymRadar.Model.Payload.Response.PTSlot
{
    public class GetPTSlotResponse
    {
        public Guid Id { get; set; }
        public GetSlotResponse? Slot { get; set; }
        public bool? Active { get; set; }
    }
}

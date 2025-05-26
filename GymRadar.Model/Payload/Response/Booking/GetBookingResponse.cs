using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Payload.Response.PT;
using GymRadar.Model.Payload.Response.Slot;
using GymRadar.Model.Payload.Response.User;

namespace GymRadar.Model.Payload.Response.Booking
{
    public class GetBookingResponse
    {
        public Guid Id { get; set; }

        public DateOnly? Date { get; set; }

        public GetUserResponse? User {  get; set; }
        
        public GetPTResponse? PT { get; set; }

        public GetSlotResponse? Slot { get; set; }
    }
}

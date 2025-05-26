using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.Booking
{
    public class CreateBookingResponse
    {
        public Guid? PtSlotId { get; set; }

        public DateOnly? Date { get; set; }

    }
}

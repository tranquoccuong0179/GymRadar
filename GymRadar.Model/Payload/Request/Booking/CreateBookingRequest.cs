﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Request.Booking
{
    public class CreateBookingRequest
    {
        public Guid? PtSlotId { get; set; }

        public DateOnly? Date { get; set; }

    }
}

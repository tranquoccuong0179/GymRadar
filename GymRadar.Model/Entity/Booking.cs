using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class Booking
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? PtSlotId { get; set; }

    public DateOnly? Date { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public string? Status { get; set; }

    public virtual Ptslot? PtSlot { get; set; }

    public virtual User? User { get; set; }
}

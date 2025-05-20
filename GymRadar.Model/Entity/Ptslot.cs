using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class Ptslot
{
    public Guid Id { get; set; }

    public Guid SlotId { get; set; }

    public Guid Ptid { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Pt Pt { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;
}

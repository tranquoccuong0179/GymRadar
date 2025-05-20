using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class Slot
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public Guid? GymId { get; set; }

    public virtual Gym? Gym { get; set; }

    public virtual ICollection<Ptslot> Ptslots { get; set; } = new List<Ptslot>();
}

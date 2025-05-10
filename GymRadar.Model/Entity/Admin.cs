using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class Admin
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? AccountId { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? Account { get; set; }
}

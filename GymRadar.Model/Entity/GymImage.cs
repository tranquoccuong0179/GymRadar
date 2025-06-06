using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class GymImage
{
    public Guid Id { get; set; }

    public string? Url { get; set; }

    public Guid? GymId { get; set; }

    public virtual Gym? Gym { get; set; }
}

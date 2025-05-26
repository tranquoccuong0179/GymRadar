using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class Transaction
{
    public Guid Id { get; set; }

    public string? Status { get; set; }

    public double? Price { get; set; }

    public long? OrderCode { get; set; }

    public string? Description { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GymCourseId { get; set; }

    public Guid? PtId { get; set; }

    public virtual GymCourse? GymCourse { get; set; }

    public virtual Pt? Pt { get; set; }

    public virtual User? User { get; set; }
}

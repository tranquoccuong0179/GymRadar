using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class GymCoursePt
{
    public Guid Id { get; set; }

    public Guid Ptid { get; set; }

    public Guid GymCourseId { get; set; }

    public int? Session { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual GymCourse GymCourse { get; set; } = null!;

    public virtual Pt Pt { get; set; } = null!;
}

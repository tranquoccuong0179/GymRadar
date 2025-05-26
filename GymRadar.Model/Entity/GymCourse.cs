using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class GymCourse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public int Duration { get; set; }

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Image { get; set; } = null!;

    public Guid GymId { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Gym Gym { get; set; } = null!;

    public virtual ICollection<GymCoursePt> GymCoursePts { get; set; } = new List<GymCoursePt>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

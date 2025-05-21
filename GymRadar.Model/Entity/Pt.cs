using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class Pt
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public double Weight { get; set; }

    public double Height { get; set; }

    public string GoalTraining { get; set; } = null!;

    public int Experience { get; set; }

    public string Gender { get; set; } = null!;

    public Guid AccountId { get; set; }

    public Guid GymId { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Gym Gym { get; set; } = null!;

    public virtual ICollection<GymCoursePt> GymCoursePts { get; set; } = new List<GymCoursePt>();

    public virtual ICollection<Ptslot> Ptslots { get; set; } = new List<Ptslot>();
}

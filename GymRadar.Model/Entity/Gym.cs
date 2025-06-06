using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class Gym
{
    public Guid Id { get; set; }

    public string GymName { get; set; } = null!;

    public int Since { get; set; }

    public string Address { get; set; } = null!;

    public string RepresentName { get; set; } = null!;

    public string TaxCode { get; set; } = null!;

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public string Qrcode { get; set; } = null!;

    public bool HotResearch { get; set; }

    public Guid AccountId { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public string? MainImage { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<GymCourse> GymCourses { get; set; } = new List<GymCourse>();

    public virtual ICollection<GymImage> GymImages { get; set; } = new List<GymImage>();

    public virtual ICollection<Pt> Pts { get; set; } = new List<Pt>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}

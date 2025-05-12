using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class Account
{
    public Guid Id { get; set; }

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Gym> Gyms { get; set; } = new List<Gym>();

    public virtual ICollection<Pt> Pts { get; set; } = new List<Pt>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

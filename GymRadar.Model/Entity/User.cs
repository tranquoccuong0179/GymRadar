using System;
using System.Collections.Generic;

namespace GymRadar.Model.Entity;

public partial class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public int Age { get; set; }

    public double Weight { get; set; }

    public double Height { get; set; }

    public string Gender { get; set; } = null!;

    public string Address { get; set; } = null!;

    public Guid AccountId { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

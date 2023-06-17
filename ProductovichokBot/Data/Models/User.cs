using System;
using System.Collections.Generic;

namespace ProductovichokBot.Data.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? TelegramUserNickname { get; set; }

    public int? RoleId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Surname { get; set; }

    public virtual ICollection<Code> Codes { get; set; } = new List<Code>();

    public virtual ICollection<Order> OrderClients { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderCouriers { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderPickers { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
}

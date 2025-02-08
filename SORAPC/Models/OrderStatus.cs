using System;
using System.Collections.Generic;

namespace SORAPC.Models;

public partial class OrderStatus
{
    public int IdOrderStatus { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

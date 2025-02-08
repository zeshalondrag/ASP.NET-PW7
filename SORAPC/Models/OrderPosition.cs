using System;
using System.Collections.Generic;

namespace SORAPC.Models;

public partial class OrderPosition
{
    public int IdOrderPosition { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}

using System;
using System.Collections.Generic;

namespace SORAPC.Models;

public partial class ProductCategory
{
    public int IdProductCategory { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

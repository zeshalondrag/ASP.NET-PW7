﻿using System;
using System.Collections.Generic;

namespace SORAPC.Models;

public partial class Review
{
    public int IdReview { get; set; }

    public int? ProductId { get; set; }

    public int? UsersId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? Users { get; set; }
}

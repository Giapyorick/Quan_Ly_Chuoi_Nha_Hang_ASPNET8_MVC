using System;
using System.Collections.Generic;
using GiapDepTrai.Models;

namespace GiapDepTrai1.Models;

public partial class TblCartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int MenuItemId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string? Note { get; set; }

    public virtual TblCart Cart { get; set; } = null!;

    public virtual TblMenuItem MenuItem { get; set; } = null!;
}

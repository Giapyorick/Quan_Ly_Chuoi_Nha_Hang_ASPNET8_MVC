using System;
using System.Collections.Generic;
using GiapDepTrai.Models;

namespace GiapDepTrai1.Models;

public partial class TblCart
{
    public int CartId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TblCustomer? Customers { get; set; }

    public virtual ICollection<TblCartItem> CartItem { get; set; } = new List<TblCartItem>();
}

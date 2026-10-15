using System;
using System.Collections.Generic;
using GiapDepTrai.Models;
using GiapDepTrai1.Models;

namespace GiapDepTrai.Models;

public partial class TblMenuItem
{
    public int MenuItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Descriptions { get; set; }

    public decimal? Price { get; set; }

    public int? CategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsAvailable { get; set; }

    public int? BranchId { get; set; }

    public int? Count_Saled { get; set; }

    public virtual TblBranch? Branch { get; set; }

    public virtual TblCategory? Category { get; set; }
    public virtual ICollection<TblComment> TblComments { get; set; } = new List<TblComment>();
    public virtual ICollection<TblCartItem> CartItems { get; set; } = new List<TblCartItem>();
    public virtual ICollection<TblEmployeeOrderDetail> TblEmployeeOrderDetails { get; set; } = new List<TblEmployeeOrderDetail>();
    public virtual ICollection<TblReservationMenuItem> TblReservationMenuItems { get; set; } = new List<TblReservationMenuItem>();

}

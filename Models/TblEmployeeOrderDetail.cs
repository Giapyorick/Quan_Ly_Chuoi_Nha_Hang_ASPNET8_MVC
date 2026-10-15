using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiapDepTrai.Models;

public partial class TblEmployeeOrderDetail
{
    public int OrderDetailId { get; set; }

    [Required]
    public int OrderId { get; set; }
    [Required]
    public int MenuItemId { get; set; }
    [Required]
    public int Quantity { get; set; }

    public string? Notes { get; set; }

    public virtual TblMenuItem MenuItem { get; set; } = null!;

    public virtual TblEmployeeOrder Order { get; set; } = null!;
}

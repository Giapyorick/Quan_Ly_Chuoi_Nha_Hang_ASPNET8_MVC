using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiapDepTrai.Models;

public partial class TblEmployeeOrder
{
    public int OrderId { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public int TableId { get; set; }

    public DateTime? OrderTime { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }
    [Required]
    public int? BranchId { get; set; }


    public virtual TblBranch? Branch { get; set; }

    public virtual TblEmployee Employee { get; set; } = null!;

    public virtual TblTable Table { get; set; } = null!;

    public virtual ICollection<TblEmployeeOrderDetail> TblEmployeeOrderDetails { get; set; } = new HashSet<TblEmployeeOrderDetail>();

    public virtual ICollection<TblInvoice> TblInvoices { get; set; } = new HashSet<TblInvoice>();
}

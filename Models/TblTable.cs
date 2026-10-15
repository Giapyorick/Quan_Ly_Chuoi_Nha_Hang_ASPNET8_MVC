using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblTable
{
    public int TableId { get; set; }

    public string? TableName { get; set; }

    public int? TableTypeId { get; set; }

    public int? BranchId { get; set; }

    public string? Status { get; set; }

    public virtual TblBranch? Branch { get; set; }

    public virtual TblTableType? TableType { get; set; }

    public virtual ICollection<TblEmployeeOrder> TblEmployeeOrders { get; set; } = new List<TblEmployeeOrder>();

    public virtual ICollection<TblReservation> TblReservations { get; set; } = new List<TblReservation>();
}

using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblBranch
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = null!;

    public string AddressBranch { get; set; } = null!;

    public string? Phone { get; set; }

    public string StatusBranch { get; set; } = null!;
    public virtual ICollection<TblReservation> TblReservations { get; set; } = new List<TblReservation>();

    public virtual ICollection<TblEmployeeOrder> TblEmployeeOrders { get; set; } = new List<TblEmployeeOrder>();

    public virtual ICollection<TblEmployee> TblEmployees { get; set; } = new List<TblEmployee>();

    public virtual ICollection<TblMenuItem> TblMenuItems { get; set; } = new List<TblMenuItem>();

    public virtual ICollection<TblTable> TblTables { get; set; } = new List<TblTable>();

    public virtual ICollection<TblEmployee> Employees { get; set; } = new List<TblEmployee>();
}

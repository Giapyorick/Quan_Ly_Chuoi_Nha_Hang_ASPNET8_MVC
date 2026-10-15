using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblTableType
{
    public int TableTypeId { get; set; }

    public string? TypeName { get; set; }

    public int? Seats { get; set; }

    public virtual ICollection<TblTable> TblTables { get; set; } = new List<TblTable>();
}

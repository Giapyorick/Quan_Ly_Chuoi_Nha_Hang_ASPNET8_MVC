using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;
    public bool? IsActive { get; set; }

    public virtual ICollection<TblMenuItem> TblMenuItems { get; set; } = new List<TblMenuItem>();
}

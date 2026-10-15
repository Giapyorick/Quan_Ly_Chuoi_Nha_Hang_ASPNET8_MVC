using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblNavigationHome
{
    public int? NavigationId { get; set; }

    public string? Nav1 { get; set; }

    public string? PicNav1 { get; set; }

    public string? Nav2 { get; set; }

    public string? PicNav2 { get; set; }

    public string? Nav3 { get; set; }

    public string? PicNav3 { get; set; }

    public bool? IsActive { get; set; }
}

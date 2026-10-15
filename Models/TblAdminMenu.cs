using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblAdminMenu
{
    public int AdmenuId { get; set; }

    public string? AdmenuName { get; set; }

    public bool? IsActive { get; set; }

    public string? AreaName { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public int? Levels { get; set; }

    public int? ParentId { get; set; }

    public string? Link { get; set; }

    public int? MenuOrder { get; set; }

    public int? Position { get; set; }

    public string? ItemTaget { get; set; }

    public string? Idname { get; set; }

    public string? Icon { get; set; }

    public string? Roles { get; set; }
}

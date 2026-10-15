using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblStoryHome
{
    public int StoryId { get; set; }

    public string? Title1 { get; set; }

    public string? Contents1 { get; set; }

    public string? Pic1 { get; set; }

    public string? Title2 { get; set; }

    public string? Contents2 { get; set; }

    public string? Pic2 { get; set; }

    public bool? IsActive { get; set; }

    public string? MainContents { get; set; }
}

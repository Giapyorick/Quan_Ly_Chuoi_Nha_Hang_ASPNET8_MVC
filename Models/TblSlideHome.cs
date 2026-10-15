using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblSlideHome
{
    public int Slide1Id { get; set; }

    public string? Title1 { get; set; }

    public string? Title2 { get; set; }

    public string? Contents1 { get; set; }

    public string? Contents2 { get; set; }

    public bool? IsActive { get; set; }

    public string? Img1 { get; set; }

    public string? Img2 { get; set; }
}

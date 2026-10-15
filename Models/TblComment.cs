using System;
using System.Collections.Generic;
using GiapDepTrai.Models;

namespace GiapDepTrai.Models;

public partial class TblComment
{
    public int CommentId { get; set; }

    public string Contents { get; set; } = null!;

    public DateTime CommentDate { get; set; }

    public int? MenuItemId { get; set; }

    public int? CustomerId { get; set; }

    public virtual TblCustomer? Customer { get; set; }

    public virtual TblMenuItem MenuItem { get; set; } = null!;
}

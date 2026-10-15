using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblInvoice
{
    public int InvoiceId { get; set; }

    public int OrderId { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalAmount { get; set; }

    public string? Status { get; set; }

    public virtual TblEmployeeOrder Order { get; set; } = null!;


    public virtual ICollection<TblPayment> TblPayments { get; set; } = new List<TblPayment>();
}
using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblPayment
{
    public int PaymentId { get; set; }

    public int InvoiceId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime? PaymentDate { get; set; }


    public virtual TblInvoice Invoice { get; set; } = null!;
}

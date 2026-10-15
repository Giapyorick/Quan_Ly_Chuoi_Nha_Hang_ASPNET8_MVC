using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblReservationPayment
{
    public int PaymentId { get; set; }

    public int ReservationId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime? PaymentDate { get; set; }

    public decimal AmountPaid { get; set; }
    public string? Notes { get; set; }
    public int? CustomerId { get; set; }
    public virtual TblCustomer? CustomerPayment { get; set; }
    public virtual TblReservation Reservation { get; set; } = null!;
}

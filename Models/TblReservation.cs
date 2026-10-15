using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiapDepTrai.Models;

public partial class TblReservation
{
    public int ReservationId { get; set; }

    public int CustomerId { get; set; }

    public int TableId { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? ReservationTime { get; set; }

    public int NumberOfGuests { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }
    public int? BranchId { get; set; }

    public virtual TblBranch? Branch { get; set; }
    
    public virtual TblCustomer Customer { get; set; } = null!;

    public virtual TblTable Table { get; set; } = null!;
    public virtual ICollection<TblReservationPayment> TblReservationPayments { get; set; } = new List<TblReservationPayment>();
    public virtual ICollection<TblReservationMenuItem> TblReservationMenuItems { get; set; } = new List<TblReservationMenuItem>();
}

using System;
using System.Collections.Generic;
using GiapDepTrai.Models;
using GiapDepTrai1.Models;

namespace GiapDepTrai.Models;

public partial class TblCustomer
{
    public int? CustomerId { get; set; }

    public string? FullName { get; set; } = null!;

    public string? Gender { get; set; } = null!;

    public string? Phone { get; set; } = null!;

    public string? Address { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateOnly? BirthofDate { get; set; }
    public string? IdentityCard { get; set; }
    public string? Img { get; set; }
    public bool? Status { get; set; }
    public string? Passwork { get; set; }
     public virtual ICollection<TblCart> TblCarts { get; set; } = new List<TblCart>();
    public virtual ICollection<TblComment> TblComments { get; set; } = new List<TblComment>();
    public virtual ICollection<TblReservationPayment> TblReservationPayments { get; set; } = new List<TblReservationPayment>();
    public virtual ICollection<TblReservation> TblReservations { get; set; } = new List<TblReservation>();
}

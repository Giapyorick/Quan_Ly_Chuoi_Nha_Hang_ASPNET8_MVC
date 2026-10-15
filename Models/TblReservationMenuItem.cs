using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace GiapDepTrai.Models;

public partial class TblReservationMenuItem
{
    public int Id { get; set; }

    public int ReservationId { get; set; }
    public int MenuItemId { get; set; }
    public int Quantity {get; set;}
    public string? Note { get; set; }

    public virtual TblMenuItem MenuItem { get; set; } = null!;

    public virtual TblReservation Reservation { get; set; } = null!;
}

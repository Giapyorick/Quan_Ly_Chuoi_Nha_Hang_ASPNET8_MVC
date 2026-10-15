

using System.ComponentModel.DataAnnotations;
using GiapDepTrai1.Models;

namespace GiapDepTrai.Models;

public class AddNewReservation
{
     public int CustomerId { get; set; }
    public int TableId { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime ReservationTime { get; set; }

    public int NumberOfGuests { get; set; }
    public string? Notes { get; set; }
    public List<AddNewCartItem>? CartItems { get; set; }

    public List<AddNewReservationMenuItem> MenuItems { get; set; } = new List<AddNewReservationMenuItem>();

}

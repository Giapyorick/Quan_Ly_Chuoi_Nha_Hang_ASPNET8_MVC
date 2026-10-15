using GiapDepTrai.Models;
public class AddNewCartItem
{

	public int MenuItemId { get; set; }
	public int CartId { get; set; } 
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public string? Note { get; set; }
}
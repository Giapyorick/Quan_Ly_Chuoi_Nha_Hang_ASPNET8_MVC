namespace GiapDepTrai.Models;

public partial class AddNewContact
{
    public int ContactID { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Message { get; set; }

    public DateTime? DateTime { get; set; }

}
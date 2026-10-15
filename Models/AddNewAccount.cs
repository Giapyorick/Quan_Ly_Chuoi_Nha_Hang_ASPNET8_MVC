namespace GiapDepTrai.Models
{
    public class AddNewAccount
    {
        public int AccountId { get; set; }

        public string UserName { get; set; } = null!;

        public string PassWords { get; set; } = null!;

        public int? EmployeeId { get; set; }

        public string? Roles { get; set; } 
    }
}

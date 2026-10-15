using System.ComponentModel.DataAnnotations.Schema;

namespace GiapDepTrai.Models
{
    public class AddNewEmployee
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Position { get; set; } = null!;

        public int? BranchId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string? Gender { get; set; }
        public DateOnly? DateofBirth { get; set; }
        public string? IdentityCard { get; set; }
        public string? Addresss { get; set; }
        public bool? Status { get; set; }
        public string? Img { get; set; }
        [NotMapped]
        public IFormFile? formFile { get; set; }
    }
}

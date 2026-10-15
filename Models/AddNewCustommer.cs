using System.ComponentModel.DataAnnotations.Schema;

namespace GiapDepTrai.Models
{
    public class AddNewCustommer
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? Email { get; set; }

        public DateOnly? BirthofDate { get; set; }

        public string? IdentityCard { get; set; }

        public string? Img { get; set; }

        public bool? Status { get; set; }
        public string? Passwork { get; set; }
        [NotMapped]
        public IFormFile? formFile { get; set; }

    }
}

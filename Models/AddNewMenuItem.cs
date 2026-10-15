using System.ComponentModel.DataAnnotations.Schema;

namespace GiapDepTrai.Models
{
    public class AddNewMenuItem
    {
        public int MenuItemId { get; set; }

        public string ItemName { get; set; } = null!;

        public string? Descriptions { get; set; }

        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        public bool? IsAvailable { get; set; }

        public int? BranchId { get; set; }
        [NotMapped]
        public IFormFile? formFile { get; set; }
        public int? Count_Saled { get; set; }
    }
}

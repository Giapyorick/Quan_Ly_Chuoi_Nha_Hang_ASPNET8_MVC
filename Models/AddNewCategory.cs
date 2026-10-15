namespace GiapDepTrai.Models
{
    public class AddNewCategory
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}

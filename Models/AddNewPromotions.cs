namespace GiapDepTrai.Models
{
    public class AddNewPromotions
    {
        public int PromotionId { get; set; }

        public string PromotionName { get; set; } = null!;

        public string? Description { get; set; }

        public string? DiscountType { get; set; }

        public decimal DiscountValue { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public decimal? MinOrderAmount { get; set; }
    }
}

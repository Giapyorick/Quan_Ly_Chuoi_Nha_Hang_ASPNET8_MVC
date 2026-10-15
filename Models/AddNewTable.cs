namespace GiapDepTrai.Models
{
    public class AddNewTable
    {
        public int TableId { get; set; }

        public string? TableName { get; set; }

        public int? TableTypeId { get; set; }

        public int? BranchId { get; set; }
        public string? Status { get; set; }
    }
}

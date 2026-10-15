namespace GiapDepTrai.Models
{
    public class AddNewBranch
    {
        public int BranchId { get; set; }

        public string BranchName { get; set; } = null!;

        public string AddressBranch { get; set; } = null!;

        public string? Phone { get; set; }

        public string StatusBranch { get; set; } = null!;

        public virtual ICollection<TblMenuItem> MenuItems { get; set; } = new List<TblMenuItem>();
    }
}

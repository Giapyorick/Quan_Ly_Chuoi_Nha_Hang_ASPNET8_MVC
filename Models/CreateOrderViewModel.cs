namespace GiapDepTrai.Models
{
    public class CreateOrderViewModel
    {
        public TblEmployeeOrder Order { get; set; } = new TblEmployeeOrder();
        public List<TblEmployeeOrderDetail> OrderDetails { get; set; } = new List<TblEmployeeOrderDetail>()
        {
        new TblEmployeeOrderDetail() 
        };
    }
}

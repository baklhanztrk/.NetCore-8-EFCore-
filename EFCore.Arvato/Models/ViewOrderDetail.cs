using EFCore.Arvato.Core.Orders;

namespace EFCore.Arvato.Models
{
    public class ViewOrderDetail
    {
        public Order Detail { get; set; }
        public OrderComment OrderCommnetDetail { get; set; }
    }
}

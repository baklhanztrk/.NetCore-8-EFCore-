using System.ComponentModel.DataAnnotations;

namespace EFCore.Arvato.Core.Orders
{
    public class OrderDetail
    {
        public Order Detail { get; set; }
        public OrderComment OrderCommnetDetail { get; set; }
    }
}

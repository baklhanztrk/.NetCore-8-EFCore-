using System.ComponentModel.DataAnnotations;

namespace EFCore.Arvato.Core.Orders
{
    public class OrderComment
    {
        public long Id { get; set; }
        public long Order_Id { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public long CreatedAt { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Arvato.Core.Orders
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        public  long AccountId { get; set; }
      
        public long OrderId { get; set; }
      
        public  string OrderNumber { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public string OrderType { get; set; } = "b2c";
        public string Status { get; set; } = "Received";
        public string SalesChannel { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;

        public string Carrier { get; set; }
      
        public  long UserId { get; set; }

        public long UpdatedAt { get; set; }
        public long CreatedAt { get; set; }

        

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Arvato.Models
{
    public sealed class ViewOrder 
    {
       
        public required long AccountId { get; set; }
       
        public required long OrderId { get; set; }
        public string OrderNumber { get; set; }=string.Empty;
        public DateTime ? OrderDate { get; set; } = DateTime.Now;
        public string OrderType { get; set; } = "b2c";
        public string Status { get; set; }=string.Empty;
        public string SalesChannel { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;

        public string Carrier { get; set; }
       
        public required long UserId { get; set; }

        public long UpdatedAt { get; set; }
        public long CreatedAt { get; set; }

        //public ViewOrderComment Comment { get; set; }

    }


    public sealed class ViewOrderComment
    {       
        public required long OrderId { get; set; }

        public required long UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public long CreatedAt { get; set; }

    }
}

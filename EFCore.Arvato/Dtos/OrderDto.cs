using EFCore.Arvato.Models;

namespace EFCore.Arvato.Dtos
{
    public record OrderDto(long AccountId,string OrderNumber,string City,string Carrier,string District,string SalesChannel, string Status, DateTime? OrderDate, string OrderType,long UserId,long CreateAt,long OrderId,ViewOrderComment Comment);
    
}

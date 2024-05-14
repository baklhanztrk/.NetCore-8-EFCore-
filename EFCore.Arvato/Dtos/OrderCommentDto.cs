using EFCore.Arvato.Models;

namespace EFCore.Arvato.Dtos
{
  
        public record OrderCommentDto(long OrderId,string Comment,long UserId,long CreatedAt);
    
}

using Microsoft.AspNetCore.Identity;

namespace EFCore.Arvato.Core.Auth
{
    public sealed class User : IdentityUser<Guid>
    {  
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            //public string FullName => string.Join(" ", FirstName, LastName);
            public long CreatedAt { get; set; }
        
    }
}

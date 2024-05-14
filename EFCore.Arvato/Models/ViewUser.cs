using Microsoft.AspNetCore.Identity;

namespace EFCore.Arvato.Models
{
    public sealed class ViewUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        //public string FullName => string.Join(" ", FirstName, LastName);

        public long CreatedAt { get; set; }
    }
}

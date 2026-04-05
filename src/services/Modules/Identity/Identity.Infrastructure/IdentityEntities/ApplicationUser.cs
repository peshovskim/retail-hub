using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.IdentityEntities;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedOn { get; set; }
}

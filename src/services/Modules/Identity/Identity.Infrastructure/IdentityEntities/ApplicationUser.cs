using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.IdentityEntities;

public sealed class ApplicationUser : IdentityUser<int>
{
    public Guid Uid { get; set; }

    public DateTime CreatedOn { get; set; }
}

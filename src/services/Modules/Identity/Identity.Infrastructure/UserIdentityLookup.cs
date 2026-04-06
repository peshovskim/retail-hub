using Identity.Infrastructure.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using RetailHub.SharedKernel.Application.Common.Abstractions;

namespace Identity.Infrastructure;

internal sealed class UserIdentityLookup : IUserIdentityLookup
{
    private readonly RetailHubIdentityDbContext _dbContext;

    public UserIdentityLookup(RetailHubIdentityDbContext dbContext) => _dbContext = dbContext;

    public async Task<int?> GetUserIdByUidAsync(Guid uid, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<ApplicationUser>()
            .AsNoTracking()
            .Where(u => u.Uid == uid)
            .Select(u => (int?)u.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}

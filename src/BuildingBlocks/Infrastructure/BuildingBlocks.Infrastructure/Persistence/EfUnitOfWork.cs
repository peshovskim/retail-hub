using Microsoft.EntityFrameworkCore;
using RetailHub.BuildingBlocks.Application.Common.Abstractions;

namespace RetailHub.BuildingBlocks.Infrastructure.Persistence;

public sealed class EfUnitOfWork<TDbContext>(TDbContext db) : IUnitOfWork
    where TDbContext : DbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);
}

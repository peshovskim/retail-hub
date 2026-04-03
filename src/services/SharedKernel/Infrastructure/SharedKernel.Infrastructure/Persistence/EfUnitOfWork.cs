using Microsoft.EntityFrameworkCore;
using RetailHub.SharedKernel.Application.Common.Abstractions;

namespace RetailHub.SharedKernel.Infrastructure.Persistence;

public sealed class EfUnitOfWork<TDbContext> : IUnitOfWork
    where TDbContext : DbContext
{
    private readonly TDbContext _db;

    public EfUnitOfWork(TDbContext db)
    {
        _db = db;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _db.SaveChangesAsync(cancellationToken);
}

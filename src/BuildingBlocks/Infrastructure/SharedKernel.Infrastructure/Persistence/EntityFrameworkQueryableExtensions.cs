using Microsoft.EntityFrameworkCore;

namespace RetailHub.SharedKernel.Infrastructure.Persistence;

public static class EntityFrameworkQueryableExtensions
{
    public static IQueryable<T> AsNoTrackingIf<T>(this IQueryable<T> query, bool noTracking)
        where T : class =>
        noTracking ? query.AsNoTracking() : query;
}

using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Responses;
using Catalog.Infrastructure.Persistence.Read.Product.Factories;
using Microsoft.EntityFrameworkCore;
using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure.Persistence.Read.Product.Queries;

internal sealed class ProductQueries : IProductReadRepository
{
    private readonly CatalogReadDbContext _db;
    private readonly ProductReadFactory _readFactory;

    public ProductQueries(CatalogReadDbContext db, ProductReadFactory readFactory)
    {
        _db = db;
        _readFactory = readFactory;
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAllActiveProductsAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _db.Set<ProductEntity>()
            .Where(p => p.DeletedOn == null)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows.Select(_readFactory.ToResponse).ToList();
    }
}

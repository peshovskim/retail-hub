using Catalog.Application.Product.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure.Persistence.Write.Product.Repository;

internal sealed class ProductRepository : IProductRepository
{
    private readonly CatalogWriteDbContext _db;

    public ProductRepository(CatalogWriteDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        await _db.Products.AddAsync(product, cancellationToken).ConfigureAwait(false);
    }
}

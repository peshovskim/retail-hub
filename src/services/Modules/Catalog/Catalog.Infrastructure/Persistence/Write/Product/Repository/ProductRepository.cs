using Catalog.Application.Product.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure.Persistence.Write.Product.Repository;

internal sealed class ProductRepository : IProductRepository
{
    private readonly CatalogWriteDbContext _dbContext;

    public ProductRepository(CatalogWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);
    }
}

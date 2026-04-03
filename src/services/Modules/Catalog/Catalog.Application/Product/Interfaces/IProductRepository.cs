using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Application.Product.Interfaces;

public interface IProductRepository
{
    Task AddAsync(ProductEntity product, CancellationToken cancellationToken = default);
}

using Catalog.Application.Product.Queries.GetProducts;
using Catalog.Application.Product.Responses;

namespace Catalog.Application.Product.Interfaces;

public interface IProductReadRepository
{
    Task<ProductListResult> ListActiveProductsAsync(GetProductsQuery criteria, CancellationToken cancellationToken = default);

    Task<ProductResponse?> GetActiveProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

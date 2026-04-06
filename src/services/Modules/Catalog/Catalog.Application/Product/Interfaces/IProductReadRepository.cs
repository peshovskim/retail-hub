using Catalog.Application.Product.Queries.GetProducts;
using Catalog.Application.Product.Responses;

namespace Catalog.Application.Product.Interfaces;

public interface IProductReadRepository
{
    Task<ProductListResult> ListActiveProductsAsync(GetProductsQuery criteria, CancellationToken cancellationToken = default);

    Task<ProductResponse?> GetActiveProductByUidAsync(Guid uid, CancellationToken cancellationToken = default);

    Task<ProductResponse?> GetActiveProductByInternalIdAsync(
        int productId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<int, Guid>> GetProductUidsByIdsAsync(
        IEnumerable<int> productIds,
        CancellationToken cancellationToken = default);
}

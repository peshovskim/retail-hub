using Catalog.Application.Category.Responses;

namespace Catalog.Application.Category.Interfaces;

public interface ICategoryReadRepository
{
    Task<IReadOnlyList<CategoryResponse>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CategoryMenuSourceRow>> GetAllActiveCategoriesAsync(
        CancellationToken cancellationToken = default);
}


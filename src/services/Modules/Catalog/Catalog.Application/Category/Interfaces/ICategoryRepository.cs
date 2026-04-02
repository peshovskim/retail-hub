using CategoryEntity = Catalog.Domain.Category.Domain.Category;

namespace Catalog.Application.Category.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(CategoryEntity category, CancellationToken cancellationToken = default);
}

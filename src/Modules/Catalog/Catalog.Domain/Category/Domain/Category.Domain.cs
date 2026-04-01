namespace Catalog.Domain.Category.Domain;

public sealed partial class Category
{
    public void SoftDelete(DateTime utcNow)
    {
        DeletedOn = utcNow;
    }

    public void Restore()
    {
        DeletedOn = null;
    }

    public bool IsActive => DeletedOn is null;
}

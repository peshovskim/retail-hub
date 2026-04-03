namespace Catalog.Domain.Product.Domain;

public sealed partial class Product
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

namespace RetailHub.Api.Options;

/// <summary>
/// Azure Blob settings. Catalog image URLs come from SQL (<c>ImageUrl</c>); this section is for optional blob client use.
/// </summary>
public sealed class AzureStorageOptions
{
    public const string SectionName = "AzureStorage";

    /// <summary>When empty, <c>BlobServiceClient</c> is not registered in DI.</summary>
    public string ConnectionString { get; set; } = string.Empty;

    public string ProductImagesContainer { get; set; } = "product-images";
}

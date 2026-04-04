namespace RetailHub.Api.Contracts;

public sealed class UploadedFile
{
    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public string Sha256Hash { get; set; } = null!;

    public byte[] Content { get; set; } = null!;
}

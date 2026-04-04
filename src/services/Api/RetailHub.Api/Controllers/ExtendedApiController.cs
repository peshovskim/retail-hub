using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using RetailHub.Api.Contracts;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.Api.Controllers;

public class ExtendedApiController : ControllerBase
{
    protected const string PdfContentType = "application/pdf";

    protected Guid UserUid
    {
        get
        {
            ClaimsIdentity? identity = User.Identity as ClaimsIdentity;

            Claim? claim = identity?.FindFirst("uid");

            if (Guid.TryParse(claim?.Value, out Guid userUid))
            {
                return userUid;
            }

            throw new InvalidOperationException("Identity must never be null");
        }
    }

    protected static HttpStatusCode GetStatusCode(ResultType resultType)
    {
        return resultType switch
        {
            ResultType.NotFound => HttpStatusCode.NotFound,
            ResultType.Forbidden => HttpStatusCode.Forbidden,
            ResultType.Conflicted => HttpStatusCode.Conflict,
            ResultType.Invalid => HttpStatusCode.NotAcceptable,
            ResultType.Unauthorized => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError,
        };
    }

    protected FileContentResult GetPdfResponse(byte[] pdfAsByteArray)
    {
        return new FileContentResult(pdfAsByteArray, PdfContentType);
    }

    protected async Task<Result<UploadedFile>> GetFileFromRequestAsync(CancellationToken cancellationToken = default)
    {
        IFormFileCollection? files = HttpContext?.Request?.Form?.Files;

        if (files is null || files.Count == 0)
        {
            return Result<UploadedFile>.NotFound(ApiResultErrorCodes.FileNotFound, "No file was uploaded.");
        }

        IFormFile? postedFile = files.FirstOrDefault();

        if (postedFile is null || string.IsNullOrEmpty(postedFile.FileName))
        {
            return Result<UploadedFile>.NotFound(ApiResultErrorCodes.FileValidationName, "Invalid file name.");
        }

        var uploadedFile = new UploadedFile
        {
            FileName = postedFile.FileName,
            ContentType = string.IsNullOrEmpty(postedFile.ContentType)
                ? "application/octet-stream"
                : postedFile.ContentType,
        };

        await using var memoryStream = new MemoryStream();
        await postedFile.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);

        byte[] content = memoryStream.ToArray();

        using SHA256 sha = SHA256.Create();
        byte[] checksum = sha.ComputeHash(content);
        string sha256Hash = BitConverter.ToString(checksum).Replace("-", string.Empty, StringComparison.Ordinal);

        uploadedFile.Sha256Hash = sha256Hash;
        uploadedFile.Content = content;

        return Result<UploadedFile>.Success(uploadedFile);
    }

    protected IActionResult OkOrError<T>(Result<T> result)
    {
        IActionResult? errorResponse = GetErrorResponse(result);

        if (errorResponse is not null)
        {
            return errorResponse;
        }

        return Ok(result.Value);
    }

    protected IActionResult OkOrError(Result result)
    {
        IActionResult? errorResponse = GetErrorResponse(result);

        if (errorResponse is not null)
        {
            return errorResponse;
        }

        return Ok();
    }

    private IActionResult? GetErrorResponse(Result result)
    {
        if (result.IsFailure)
        {
            ResultError error = result.Error!;
            HttpStatusCode statusCode = GetStatusCode(error.Type);

            return new ObjectResult(new { code = error.Code, message = error.Message })
            {
                StatusCode = (int)statusCode,
            };
        }

        return null;
    }

    protected string? GetTokenFromRequest(HttpRequest request)
    {
        if (request.Cookies.TryGetValue("AccessToken", out string? cookieToken))
        {
            return cookieToken;
        }

        if (request.Headers.TryGetValue("Authorization", out StringValues bearerToken))
        {
            return bearerToken.ToString().Replace("Bearer", string.Empty, StringComparison.Ordinal).Trim();
        }

        if (request.Query.TryGetValue("access_token", out StringValues queryToken))
        {
            return queryToken;
        }

        return null;
    }
}

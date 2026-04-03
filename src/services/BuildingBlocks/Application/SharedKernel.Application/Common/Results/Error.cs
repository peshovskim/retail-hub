namespace RetailHub.SharedKernel.Application.Common.Results;

public sealed record Error
{
    public string Code { get; init; } = null!;
    public string Message { get; init; } = null!;

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error Validation(string message) => new(ErrorCodes.Validation, message);

    public static Error NotFound(string message) => new(ErrorCodes.NotFound, message);

    public static Error Conflict(string message) => new(ErrorCodes.Conflict, message);

    public static Error Failure(string code, string message) => new(code, message);
}

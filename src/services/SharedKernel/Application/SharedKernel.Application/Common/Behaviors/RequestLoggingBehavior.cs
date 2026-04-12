using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.SharedKernel.Application.Common.Behaviors;

/// <summary>
/// Structured logs for every MediatR request: timing, failures (<see cref="Result"/>), and
/// lighter logging for queries vs commands (queries at Debug to avoid noisy catalogs).
/// </summary>
public sealed class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<RequestLoggingBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        bool isCommand = IsCommand(typeof(TRequest));
        var sw = Stopwatch.StartNew();
        try
        {
            var response = await next().ConfigureAwait(false);
            sw.Stop();

            if (response is Result { IsFailure: true } failed)
            {
                _logger.LogWarning(
                    "Request {RequestName} failed after {ElapsedMs}ms: {ErrorType} {ErrorCode}",
                    requestName,
                    sw.ElapsedMilliseconds,
                    failed.Error?.Type,
                    failed.Error?.Code);
            }
            else if (isCommand)
            {
                _logger.LogInformation(
                    "Request {RequestName} completed in {ElapsedMs}ms",
                    requestName,
                    sw.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogDebug(
                    "Request {RequestName} completed in {ElapsedMs}ms",
                    requestName,
                    sw.ElapsedMilliseconds);
            }

            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, "Request {RequestName} threw after {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
            throw;
        }
    }

    private static bool IsCommand(Type requestType)
    {
        return requestType.GetInterfaces().Any(static i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>));
    }
}

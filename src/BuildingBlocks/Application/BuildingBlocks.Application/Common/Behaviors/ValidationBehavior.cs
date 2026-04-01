using FluentValidation;
using MediatR;
using RetailHub.BuildingBlocks.Application.Common.Results;

namespace RetailHub.BuildingBlocks.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators is null || !_validators.Any())
        {
            return await next().ConfigureAwait(false);
        }

        var context = new ValidationContext<TRequest>(request);
        var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)))
                .ConfigureAwait(false))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next().ConfigureAwait(false);
        }

        var message = string.Join("; ", failures.Select(f => f.ErrorMessage));
        return AdaptFailure(message);
    }

    private static TResponse AdaptFailure(string message)
    {
        if (typeof(TResponse).IsGenericType
            && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = typeof(TResponse).GenericTypeArguments[0];
            var failureMethod = typeof(Result<>).MakeGenericType(valueType).GetMethod(
                nameof(Result<object>.Failure),
                new[] { typeof(Error) });
            if (failureMethod is not null)
            {
                var failureResult = failureMethod.Invoke(null, new object[] { Error.Validation(message) });
                return (TResponse)(failureResult ?? throw new InvalidOperationException("Result failure could not be created."));
            }
        }

        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(Error.Validation(message));
        }

        throw new ValidationException(message);
    }
}

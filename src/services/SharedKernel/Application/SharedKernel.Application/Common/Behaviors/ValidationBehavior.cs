using System.Reflection;
using FluentValidation;
using MediatR;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.SharedKernel.Application.Common.Behaviors;

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
            var resultType = typeof(Result<>).MakeGenericType(valueType);
            var invalidMethod = resultType.GetMethod(
                nameof(Result<int>.Invalid),
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(string), typeof(string) },
                null);
            if (invalidMethod is not null)
            {
                var failureResult = invalidMethod.Invoke(null, new object[] { ResultCodes.Validation, message });
                return (TResponse)(failureResult ?? throw new InvalidOperationException("Result failure could not be created."));
            }
        }

        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Invalid(ResultCodes.Validation, message);
        }

        throw new ValidationException(message);
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Odex.AspNetCore.Clarc.Application.Configuration;
using Odex.AspNetCore.Clarc.Application.CQRS;
using Odex.AspNetCore.Clarc.Application.Exceptions;

namespace Odex.AspNetCore.Clarc.Application.Behaviors;

/// <summary>
/// MediatR pipeline step that runs all registered <see cref="IValidator{T}"/> instances for the request
/// and throws <see cref="Odex.AspNetCore.Clarc.Application.Exceptions.ValidationException"/> when any rule fails.
/// </summary>
/// <typeparam name="TRequest">MediatR request type.</typeparam>
/// <typeparam name="TResponse">MediatR response type.</typeparam>
public class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    IOptions<ClarcValidationPipelineOptions> options,
    ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is ISkipValidation)
        {
            return await next(cancellationToken);
        }

        var validatorList = validators as IValidator<TRequest>[] ?? validators.ToArray();
        if (validatorList.Length == 0)
        {
            return await next(cancellationToken);
        }

        var opt = options.Value;
        var ruleSets = ParseRuleSets(opt.RuleSet);

        List<FluentValidation.Results.ValidationFailure> failures;
        if (opt.ParallelizeValidators)
        {
            var tasks = validatorList.Select(v => ValidateAsync(v, request, ruleSets, cancellationToken));
            var results = await Task.WhenAll(tasks);
            failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToList()!;
        }
        else
        {
            failures = new List<FluentValidation.Results.ValidationFailure>();
            foreach (var v in validatorList)
            {
                var r = await ValidateAsync(v, request, ruleSets, cancellationToken);
                failures.AddRange(r.Errors.Where(e => e is not null)!);
            }
        }

        if (failures.Count != 0)
        {
            if (opt.LogFailures)
            {
                logger.LogWarning(
                    "Validation failed for {RequestType} with {FailureCount} errors",
                    typeof(TRequest).FullName ?? typeof(TRequest).Name,
                    failures.Count);
            }

            throw global::Odex.AspNetCore.Clarc.Application.Exceptions.ValidationException.FromFailures(failures);
        }

        return await next(cancellationToken);
    }

    private static string[]? ParseRuleSets(string? ruleSet)
    {
        if (string.IsNullOrWhiteSpace(ruleSet))
        {
            return null;
        }

        return ruleSet.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    private static async Task<FluentValidation.Results.ValidationResult> ValidateAsync(
        IValidator<TRequest> validator,
        TRequest request,
        string[]? ruleSets,
        CancellationToken cancellationToken)
    {
        if (ruleSets is { Length: > 0 })
        {
            return await validator.ValidateAsync(
                request,
                strategy => strategy.IncludeRuleSets(ruleSets),
                cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);
        return await validator.ValidateAsync(context, cancellationToken);
    }
}

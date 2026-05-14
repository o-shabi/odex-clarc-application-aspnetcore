using FluentValidation;
using FluentValidation.Results;
using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when FluentValidation rules fail in <see cref="Behaviors.ValidationPipelineBehavior{TRequest,TResponse}"/>.
/// </summary>
public class ValidationException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance from a grouped error dictionary (legacy shape).
    /// </summary>
    /// <param name="errors">Validation errors keyed by property name (or <c>_</c> when no property name is supplied).</param>
    public ValidationException(Dictionary<string, string[]> errors)
        : base("One or more validation errors occurred", ExceptionType.ValidationFailed, null, "validation_failed")
    {
        Errors = errors;
        FailureDetails = FlattenFromDictionary(errors);
    }

    /// <summary>
    /// Initializes a new instance from detailed failure rows.
    /// </summary>
    /// <param name="failureDetails">Individual failures (grouped <see cref="Errors"/> is derived).</param>
    public ValidationException(IReadOnlyList<ValidationFailureDetail> failureDetails)
        : base("One or more validation errors occurred", ExceptionType.ValidationFailed, null, "validation_failed")
    {
        FailureDetails = failureDetails;
        Errors = GroupToDictionary(failureDetails);
    }

    /// <summary>
    /// Gets the grouped validation messages (property name to messages).
    /// </summary>
    public Dictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Gets structured failure rows including error codes and severities when available.
    /// </summary>
    public IReadOnlyList<ValidationFailureDetail> FailureDetails { get; }

    /// <summary>
    /// Builds a <see cref="ValidationException"/> from FluentValidation <see cref="ValidationFailure"/> values.
    /// </summary>
    /// <param name="failures">Non-null failures (null entries are ignored).</param>
    /// <returns>A new exception instance.</returns>
    public static ValidationException FromFailures(IEnumerable<ValidationFailure?> failures)
    {
        ArgumentNullException.ThrowIfNull(failures);
        var details = failures
            .Where(f => f is not null)
            .Select(f => f!)
            .Select(f => new ValidationFailureDetail(
                string.IsNullOrEmpty(f.PropertyName) ? "_" : f.PropertyName,
                f.ErrorMessage,
                string.IsNullOrEmpty(f.ErrorCode) ? null : f.ErrorCode,
                f.AttemptedValue?.ToString(),
                f.Severity))
            .ToList();

        return new ValidationException(details);
    }

    private static IReadOnlyList<ValidationFailureDetail> FlattenFromDictionary(Dictionary<string, string[]> errors)
    {
        var list = new List<ValidationFailureDetail>();
        foreach (var pair in errors)
        {
            foreach (var msg in pair.Value)
            {
                list.Add(new ValidationFailureDetail(pair.Key, msg, null, null, Severity.Error));
            }
        }

        return list;
    }

    private static Dictionary<string, string[]> GroupToDictionary(IReadOnlyList<ValidationFailureDetail> details)
    {
        return details
            .GroupBy(d => d.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Message).ToArray());
    }
}

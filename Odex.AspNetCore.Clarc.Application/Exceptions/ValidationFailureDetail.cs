using FluentValidation;
using FluentValidation.Results;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Describes a single validation failure with optional metadata for Problem Details or logs.
/// </summary>
/// <param name="PropertyName">Property or field name (or <c>_</c> when unknown).</param>
/// <param name="Message">User-facing message.</param>
/// <param name="ErrorCode">Optional FluentValidation error code.</param>
/// <param name="AttemptedValue">Optional string representation of the attempted value.</param>
/// <param name="Severity">FluentValidation severity.</param>
public sealed record ValidationFailureDetail(
    string PropertyName,
    string Message,
    string? ErrorCode,
    string? AttemptedValue,
    Severity Severity);

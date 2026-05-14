using MediatR;

namespace Odex.AspNetCore.Clarc.Application.CQRS;

/// <summary>
/// Marker for commands (use with <see cref="BaseCommand"/> or <see cref="BaseCommand{TResponse}"/>).
/// </summary>
public interface ICommand
{
}

/// <summary>
/// Marker for queries (use with <see cref="BaseQuery{TResponse}"/>).
/// </summary>
public interface IQuery
{
}

/// <summary>
/// When implemented by a MediatR request, <see cref="Behaviors.ValidationPipelineBehavior{TRequest,TResponse}"/> skips validation for that request.
/// </summary>
public interface ISkipValidation
{
    /// <summary>
    /// Gets an optional reason for diagnostics (never log sensitive payloads).
    /// </summary>
    string? SkipValidationReason => null;
}

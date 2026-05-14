using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Base type for application-layer exceptions with a stable <see cref="ExceptionType"/> for API mapping.
/// </summary>
public abstract class ApplicationException : Exception
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="message">Human-readable message.</param>
    /// <param name="type">Stable classification for filters and HTTP mapping.</param>
    /// <param name="innerException">Optional inner exception.</param>
    /// <param name="errorCode">Optional stable machine-readable code (e.g. for Problem Details <c>type</c> extensions).</param>
    protected ApplicationException(
        string message,
        ExceptionType type,
        Exception? innerException = null,
        string? errorCode = null)
        : base(message, innerException)
    {
        Type = type;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Gets the exception classification.
    /// </summary>
    public ExceptionType Type { get; }

    /// <summary>
    /// Gets an optional machine-readable error code for APIs and logs.
    /// </summary>
    public string? ErrorCode { get; }
}

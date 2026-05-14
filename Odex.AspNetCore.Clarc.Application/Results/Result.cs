namespace Odex.AspNetCore.Clarc.Application.Results;

/// <summary>
/// Lightweight error payload for <see cref="Result{T}"/> without using exceptions for expected failures.
/// </summary>
/// <param name="Code">Stable machine-readable code.</param>
/// <param name="Message">Human-readable message.</param>
public sealed record Error(string Code, string Message)
{
    /// <summary>
    /// Creates an error for validation-style failures.
    /// </summary>
    public static Error Validation(string message, string code = "validation_failed") => new(code, message);

    /// <summary>
    /// Creates a generic failure.
    /// </summary>
    public static Error Failure(string code, string message) => new(code, message);
}

/// <summary>
/// Represents either a successful value or an error (optional pattern for handlers and APIs).
/// </summary>
/// <typeparam name="T">Success value type.</typeparam>
public readonly record struct Result<T>(bool IsSuccess, T? Value, Error? Error)
{
    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result<T> Ok(T value) => new(true, value, null);

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result<T> Fail(Error error) => new(false, default, error);

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool IsFailure => !IsSuccess;
}

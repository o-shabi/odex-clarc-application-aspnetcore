using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when the caller is not allowed to perform an action (maps to <see cref="ExceptionType.AccessDenied"/>).
/// </summary>
/// <param name="action">Human-readable action or resource name.</param>
/// <param name="reason">Optional explanation.</param>
public class UnauthorizedAccessException(string action, string reason = "No reason provided.")
    : ApplicationException($"Unauthorized to access '{action}' action: {reason}", ExceptionType.AccessDenied, null, "access_denied");

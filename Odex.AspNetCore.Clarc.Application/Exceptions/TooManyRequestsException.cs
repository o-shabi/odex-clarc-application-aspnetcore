using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when a rate limit or quota is exceeded (<see cref="ExceptionType.TooManyRequests"/>).
/// </summary>
/// <param name="message">Human-readable explanation.</param>
public class TooManyRequestsException(string message)
    : ApplicationException(message, ExceptionType.TooManyRequests, null, "too_many_requests");

using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when a resource is permanently unavailable (<see cref="ExceptionType.Gone"/>).
/// </summary>
/// <param name="message">Human-readable explanation.</param>
public class GoneException(string message)
    : ApplicationException(message, ExceptionType.Gone, null, "gone");

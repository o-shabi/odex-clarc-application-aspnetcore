using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when the request conflicts with the current state of the resource (<see cref="ExceptionType.Conflict"/>).
/// </summary>
/// <param name="message">Human-readable explanation.</param>
public class ConflictException(string message)
    : ApplicationException(message, ExceptionType.Conflict, null, "conflict");

using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when an operation or downstream dependency times out (<see cref="ExceptionType.Timeout"/>).
/// </summary>
/// <param name="operation">Logical operation or dependency name.</param>
/// <param name="reason">Optional explanation.</param>
public class ResourceTimeoutException(string operation, string reason = "The operation timed out.")
    : ApplicationException($"Timeout for '{operation}': {reason}", ExceptionType.Timeout, null, "timeout");

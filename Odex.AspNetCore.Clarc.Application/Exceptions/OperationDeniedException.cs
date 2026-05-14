using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when an operation is denied for security or policy reasons (<see cref="ExceptionType.OperationDenied"/>).
/// </summary>
/// <param name="operation">Operation name.</param>
/// <param name="reason">Optional explanation.</param>
public class OperationDeniedException(string operation, string reason = "No reason provided")
    : ApplicationException(
        $"Operation '{operation}' denied due to security reasons: {reason}",
        ExceptionType.OperationDenied,
        null,
        "operation_denied");

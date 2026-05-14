using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when an operation is not allowed by business rules (<see cref="ExceptionType.UnallowedOperation"/>).
/// </summary>
/// <param name="operation">Operation name.</param>
/// <param name="reason">Optional explanation.</param>
public class OperationNotAllowedException(string operation, string reason = "No reason provided")
    : ApplicationException($"Operation '{operation}' is not allowed: {reason}", ExceptionType.UnallowedOperation, null, "operation_not_allowed");

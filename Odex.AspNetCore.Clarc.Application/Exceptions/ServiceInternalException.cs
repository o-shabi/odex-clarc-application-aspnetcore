using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when an internal dependency fails after being invoked (<see cref="ExceptionType.ServiceFailed"/>).
/// </summary>
/// <param name="serviceName">Logical service name.</param>
/// <param name="reason">Failure description.</param>
public class ServiceInternalException(string serviceName, string reason)
    : ApplicationException($"Service '{serviceName}' threw an exception: {reason}", ExceptionType.ServiceFailed, null, "service_failed");

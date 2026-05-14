using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when a dependency is temporarily unavailable (<see cref="ExceptionType.ServiceUnavailable"/>).
/// </summary>
/// <param name="serviceName">Logical service name.</param>
public class ServiceUnavailableException(string serviceName)
    : ApplicationException($"Service '{serviceName}' is not available at the moment", ExceptionType.ServiceUnavailable, null, "service_unavailable");

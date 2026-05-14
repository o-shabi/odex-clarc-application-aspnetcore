using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when a requested resource does not exist (<see cref="ExceptionType.NotFound"/>).
/// </summary>
/// <param name="resource">Resource or entity name.</param>
/// <param name="identifier">Identifier that was not found.</param>
public class NotFoundException(string resource, string identifier)
    : ApplicationException($"{resource} with identifier '{identifier}' was not found", ExceptionType.NotFound, null, "not_found");

using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

/// <summary>
/// Thrown when a duplicate unique identifier is detected (<see cref="ExceptionType.Duplicate"/>).
/// </summary>
/// <param name="resource">Resource or entity name.</param>
/// <param name="identifier">Conflicting identifier value.</param>
public class DuplicateResourceException(string resource, string identifier)
    : ApplicationException($"{resource} with identifier '{identifier}' already exists", ExceptionType.Duplicate, null, "duplicate_resource");

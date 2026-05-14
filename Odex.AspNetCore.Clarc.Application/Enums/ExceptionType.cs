namespace Odex.AspNetCore.Clarc.Application.Enums;

/// <summary>
/// Application-layer exception classification for mapping to HTTP responses or logs.
/// This is unrelated to Domain or Infrastructure exception type enums shipped in other CLARC packages.
/// </summary>
public enum ExceptionType
{
    /// <summary>Unclassified failure.</summary>
    Unknown,

    /// <summary>Duplicate resource or identifier conflict.</summary>
    Duplicate,

    /// <summary>Internal service failure after invocation.</summary>
    ServiceFailed,

    /// <summary>Temporary unavailability.</summary>
    ServiceUnavailable,

    /// <summary>Security or policy denial.</summary>
    OperationDenied,

    /// <summary>Business rule disallowing the operation.</summary>
    UnallowedOperation,

    /// <summary>Authorization failure.</summary>
    AccessDenied,

    /// <summary>Input validation failure.</summary>
    ValidationFailed,

    /// <summary>Requested resource was not found.</summary>
    NotFound,

    /// <summary>State conflict with current server resource (HTTP 409-style).</summary>
    Conflict,

    /// <summary>Resource existed but is no longer available (HTTP 410-style).</summary>
    Gone,

    /// <summary>Operation or dependency timed out.</summary>
    Timeout,

    /// <summary>Rate limit or quota exceeded (HTTP 429-style).</summary>
    TooManyRequests
}

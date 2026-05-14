namespace Odex.AspNetCore.Clarc.Application.CQRS;

/// <summary>
/// Base record for CQRS response DTOs.
/// </summary>
public abstract record BaseResponse
{
    /// <summary>
    /// Gets the UTC timestamp when the response instance was created (default: creation time).
    /// </summary>
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
}

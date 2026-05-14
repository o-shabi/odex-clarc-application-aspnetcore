using MediatR;

namespace Odex.AspNetCore.Clarc.Application.CQRS;

/// <summary>
/// Base record for queries handled through MediatR.
/// </summary>
/// <typeparam name="TResponse">The handler response type.</typeparam>
public abstract record BaseQuery<TResponse> : IRequest<TResponse>, IQuery
{
    /// <summary>
    /// Gets the UTC timestamp when the query instance was created (default: creation time).
    /// </summary>
    public DateTime RequestedAt { get; init; } = DateTime.UtcNow;
}

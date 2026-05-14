using MediatR;

namespace Odex.AspNetCore.Clarc.Application.CQRS;

/// <summary>
/// Command with no meaningful response; maps to MediatR <see cref="Unit"/>.
/// </summary>
public abstract record BaseCommand : IRequest<Unit>, ICommand
{
    /// <summary>
    /// Gets the UTC timestamp when the command instance was created (default: creation time).
    /// </summary>
    public DateTime ExecutedAt { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Base record for commands handled through MediatR.
/// </summary>
/// <typeparam name="TResponse">The handler response type.</typeparam>
public abstract record BaseCommand<TResponse> : IRequest<TResponse>, ICommand
{
    /// <summary>
    /// Gets the UTC timestamp when the command instance was created (default: creation time).
    /// </summary>
    public DateTime ExecutedAt { get; init; } = DateTime.UtcNow;
}

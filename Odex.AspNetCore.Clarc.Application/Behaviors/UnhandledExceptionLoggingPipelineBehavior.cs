using MediatR;
using Microsoft.Extensions.Logging;

namespace Odex.AspNetCore.Clarc.Application.Behaviors;

/// <summary>
/// Logs unhandled exceptions from downstream pipeline steps and the handler, then rethrows.
/// </summary>
public sealed class UnhandledExceptionLoggingPipelineBehavior<TRequest, TResponse>(ILogger<UnhandledExceptionLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unhandled exception executing MediatR request {RequestType}",
                typeof(TRequest).FullName ?? typeof(TRequest).Name);
            throw;
        }
    }
}

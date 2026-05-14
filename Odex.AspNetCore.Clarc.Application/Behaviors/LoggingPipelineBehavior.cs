using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Odex.AspNetCore.Clarc.Application.Configuration;

namespace Odex.AspNetCore.Clarc.Application.Behaviors;

/// <summary>
/// Logs request pipeline timing at a configurable level (never logs request payloads).
/// </summary>
public sealed class LoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger,
    IOptions<ClarcLoggingPipelineOptions> options)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ClarcLoggingPipelineOptions _options = options.Value;

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var typeName = typeof(TRequest).FullName ?? typeof(TRequest).Name;
        if (_options.IncludeRequestTypeName)
        {
            logger.Log(_options.PipelineLogLevel, "MediatR pipeline start {RequestType}", typeName);
        }
        else
        {
            logger.Log(_options.PipelineLogLevel, "MediatR pipeline start");
        }

        try
        {
            var response = await next(cancellationToken);
            sw.Stop();
            if (_options.IncludeRequestTypeName)
            {
                logger.Log(
                    _options.PipelineLogLevel,
                    "MediatR pipeline complete {RequestType} in {ElapsedMs} ms",
                    typeName,
                    sw.ElapsedMilliseconds);
            }
            else
            {
                logger.Log(
                    _options.PipelineLogLevel,
                    "MediatR pipeline complete in {ElapsedMs} ms",
                    sw.ElapsedMilliseconds);
            }

            return response;
        }
        catch (Exception)
        {
            sw.Stop();
            logger.Log(
                LogLevel.Debug,
                "MediatR pipeline faulted {RequestType} after {ElapsedMs} ms",
                typeName,
                sw.ElapsedMilliseconds);
            throw;
        }
    }
}

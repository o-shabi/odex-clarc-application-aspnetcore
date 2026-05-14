using System.Diagnostics;
using System.Diagnostics.Metrics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Odex.AspNetCore.Clarc.Application.Behaviors;

/// <summary>
/// Emits lightweight <see cref="Meter"/> metrics for MediatR requests (no external OpenTelemetry dependency).
/// </summary>
public sealed class RequestMetricsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private static readonly Meter Meter = new("Odex.AspNetCore.Clarc.Application", "0.2.0");
    private static readonly Counter<long> RequestTotal = Meter.CreateCounter<long>("clarc_requests_total");
    private static readonly Histogram<double> RequestDurationMs = Meter.CreateHistogram<double>("clarc_request_duration_ms");

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var tags = new TagList { { "request_type", typeof(TRequest).FullName ?? typeof(TRequest).Name } };
        try
        {
            return await next(cancellationToken);
        }
        finally
        {
            sw.Stop();
            RequestTotal.Add(1, tags);
            RequestDurationMs.Record(sw.Elapsed.TotalMilliseconds, tags);
        }
    }
}

using Microsoft.Extensions.Logging;

namespace Odex.AspNetCore.Clarc.Application.Configuration;

/// <summary>
/// Options for <see cref="Behaviors.LoggingPipelineBehavior{TRequest,TResponse}"/>.
/// </summary>
public sealed class ClarcLoggingPipelineOptions
{
    /// <summary>
    /// Gets or sets the log level for pipeline start/complete messages. Default is <see cref="LogLevel.Debug"/>.
    /// </summary>
    public LogLevel PipelineLogLevel { get; set; } = LogLevel.Debug;

    /// <summary>
    /// Gets or sets a value indicating whether request type names are included in log messages (no payload is ever logged here).
    /// </summary>
    public bool IncludeRequestTypeName { get; set; } = true;
}

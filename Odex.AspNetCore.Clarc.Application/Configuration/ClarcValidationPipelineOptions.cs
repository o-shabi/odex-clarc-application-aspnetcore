namespace Odex.AspNetCore.Clarc.Application.Configuration;

/// <summary>
/// Options for <see cref="Behaviors.ValidationPipelineBehavior{TRequest,TResponse}"/>.
/// Configure via <see cref="ClarcApplicationBuilder.ConfigureValidation"/> or <c>services.Configure&lt;ClarcValidationPipelineOptions&gt;(...)</c>.
/// </summary>
public sealed class ClarcValidationPipelineOptions
{
    /// <summary>
    /// Gets or sets the optional FluentValidation rule set name(s) to include (comma-separated), or <c>null</c> to use default rules only.
    /// </summary>
    public string? RuleSet { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether validators for the same request run in parallel. Default is <see langword="true"/>.
    /// </summary>
    public bool ParallelizeValidators { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether failed validation should be logged at warning level when a logger is available.
    /// </summary>
    public bool LogFailures { get; set; }
}

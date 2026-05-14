using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Odex.AspNetCore.Clarc.Application.Configuration;

/// <summary>
/// Immutable registration model produced by <see cref="ClarcApplicationBuilder"/>.
/// </summary>
public sealed class ClarcApplicationDescriptor
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public ClarcApplicationDescriptor(
        IReadOnlyList<Assembly> assemblies,
        Action<MediatRServiceConfiguration>? configureMediatR,
        ServiceLifetime validatorLifetime,
        Action<ClarcValidationPipelineOptions>? configureValidation,
        Action<ClarcLoggingPipelineOptions>? configureLogging,
        bool enableValidationPipeline,
        bool enableLoggingPipeline,
        bool enableMetricsPipeline,
        bool enableUnhandledExceptionPipeline)
    {
        Assemblies = assemblies;
        ConfigureMediatR = configureMediatR;
        ValidatorLifetime = validatorLifetime;
        ConfigureValidation = configureValidation;
        ConfigureLogging = configureLogging;
        EnableValidationPipeline = enableValidationPipeline;
        EnableLoggingPipeline = enableLoggingPipeline;
        EnableMetricsPipeline = enableMetricsPipeline;
        EnableUnhandledExceptionPipeline = enableUnhandledExceptionPipeline;
    }

    /// <summary>
    /// Gets assemblies scanned for MediatR handlers and FluentValidation validators.
    /// </summary>
    public IReadOnlyList<Assembly> Assemblies { get; }

    /// <summary>
    /// Gets optional MediatR configuration callback.
    /// </summary>
    public Action<MediatRServiceConfiguration>? ConfigureMediatR { get; }

    /// <summary>
    /// Gets the DI lifetime for validators registered from the assemblies.
    /// </summary>
    public ServiceLifetime ValidatorLifetime { get; }

    /// <summary>
    /// Gets optional validation pipeline options configuration.
    /// </summary>
    public Action<ClarcValidationPipelineOptions>? ConfigureValidation { get; }

    /// <summary>
    /// Gets optional logging pipeline options configuration.
    /// </summary>
    public Action<ClarcLoggingPipelineOptions>? ConfigureLogging { get; }

    /// <summary>
    /// Gets a value indicating whether <see cref="Behaviors.ValidationPipelineBehavior{TRequest,TResponse}"/> is registered.
    /// </summary>
    public bool EnableValidationPipeline { get; }

    /// <summary>
    /// Gets a value indicating whether <see cref="Behaviors.LoggingPipelineBehavior{TRequest,TResponse}"/> is registered.
    /// </summary>
    public bool EnableLoggingPipeline { get; }

    /// <summary>
    /// Gets a value indicating whether <see cref="Behaviors.RequestMetricsPipelineBehavior{TRequest,TResponse}"/> is registered.
    /// </summary>
    public bool EnableMetricsPipeline { get; }

    /// <summary>
    /// Gets a value indicating whether <see cref="Behaviors.UnhandledExceptionLoggingPipelineBehavior{TRequest,TResponse}"/> is registered.
    /// </summary>
    public bool EnableUnhandledExceptionPipeline { get; }
}

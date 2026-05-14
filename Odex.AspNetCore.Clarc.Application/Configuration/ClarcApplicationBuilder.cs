using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Odex.AspNetCore.Clarc.Application.Configuration;

/// <summary>
/// Fluent builder for <see cref="ServiceCollectionExtensions.AddClarcApplication(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{ClarcApplicationBuilder})"/>.
/// </summary>
public sealed class ClarcApplicationBuilder
{
    private readonly List<Assembly> _assemblies = new();
    private Action<MediatRServiceConfiguration>? _configureMediatR;
    private ServiceLifetime _validatorLifetime = ServiceLifetime.Scoped;
    private Action<ClarcValidationPipelineOptions>? _configureValidation;
    private Action<ClarcLoggingPipelineOptions>? _configureLogging;
    private bool _enableValidationPipeline = true;
    private bool _enableLoggingPipeline = true;
    private bool _enableMetricsPipeline = true;
    private bool _enableUnhandledExceptionPipeline = true;

    /// <summary>
    /// Adds an assembly to scan for MediatR handlers and FluentValidation validators.
    /// </summary>
    public ClarcApplicationBuilder AddAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        if (!_assemblies.Contains(assembly))
        {
            _assemblies.Add(assembly);
        }

        return this;
    }

    /// <summary>
    /// Adds multiple assemblies to scan.
    /// </summary>
    public ClarcApplicationBuilder AddAssemblies(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);
        foreach (var assembly in assemblies)
        {
            AddAssembly(assembly);
        }

        return this;
    }

    /// <summary>
    /// Adds the assembly that contains <typeparamref name="T"/>.
    /// </summary>
    public ClarcApplicationBuilder AddAssemblyContaining<T>() where T : class =>
        AddAssembly(typeof(T).Assembly);

    /// <summary>
    /// Configures MediatR (notifications, behaviors, lifetime, etc.) in addition to handler registration from configured assemblies.
    /// </summary>
    public ClarcApplicationBuilder ConfigureMediatR(Action<MediatRServiceConfiguration> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        _configureMediatR = _configureMediatR is null ? configure : Chain(_configureMediatR, configure);
        return this;
    }

    /// <summary>
    /// Sets the DI lifetime for validators discovered in the configured assemblies.
    /// </summary>
    public ClarcApplicationBuilder UseValidatorLifetime(ServiceLifetime lifetime)
    {
        _validatorLifetime = lifetime;
        return this;
    }

    /// <summary>
    /// Configures validation pipeline options (rule sets, parallelism, logging of failures).
    /// </summary>
    public ClarcApplicationBuilder ConfigureValidation(Action<ClarcValidationPipelineOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        _configureValidation = _configureValidation is null ? configure : Chain(_configureValidation, configure);
        return this;
    }

    /// <summary>
    /// Configures logging pipeline options (levels, request type naming).
    /// </summary>
    public ClarcApplicationBuilder ConfigureLogging(Action<ClarcLoggingPipelineOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        _configureLogging = _configureLogging is null ? configure : Chain(_configureLogging, configure);
        return this;
    }

    /// <summary>
    /// Enables or disables registration of <see cref="Behaviors.ValidationPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    public ClarcApplicationBuilder EnableValidationPipeline(bool enable = true)
    {
        _enableValidationPipeline = enable;
        return this;
    }

    /// <summary>
    /// Enables or disables registration of <see cref="Behaviors.LoggingPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    public ClarcApplicationBuilder EnableLoggingPipeline(bool enable = true)
    {
        _enableLoggingPipeline = enable;
        return this;
    }

    /// <summary>
    /// Enables or disables registration of <see cref="Behaviors.RequestMetricsPipelineBehavior{TRequest,TResponse}"/> (<see cref="System.Diagnostics.Metrics"/>).
    /// </summary>
    public ClarcApplicationBuilder EnableMetricsPipeline(bool enable = true)
    {
        _enableMetricsPipeline = enable;
        return this;
    }

    /// <summary>
    /// Enables or disables registration of <see cref="Behaviors.UnhandledExceptionLoggingPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    public ClarcApplicationBuilder EnableUnhandledExceptionLoggingPipeline(bool enable = true)
    {
        _enableUnhandledExceptionPipeline = enable;
        return this;
    }

    /// <summary>
    /// Builds the immutable descriptor used by DI registration.
    /// </summary>
    /// <returns>The registration descriptor.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no assemblies were added.</exception>
    public ClarcApplicationDescriptor Build()
    {
        if (_assemblies.Count == 0)
        {
            throw new InvalidOperationException(
                "At least one assembly is required. Call AddAssembly, AddAssemblies, or AddAssemblyContaining<T>().");
        }

        return new ClarcApplicationDescriptor(
            _assemblies,
            _configureMediatR,
            _validatorLifetime,
            _configureValidation,
            _configureLogging,
            _enableValidationPipeline,
            _enableLoggingPipeline,
            _enableMetricsPipeline,
            _enableUnhandledExceptionPipeline);
    }

    private static Action<T> Chain<T>(Action<T> first, Action<T> second) =>
        cfg =>
        {
            first(cfg);
            second(cfg);
        };
}

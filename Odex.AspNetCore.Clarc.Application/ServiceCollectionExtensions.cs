using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Odex.AspNetCore.Clarc.Application.Behaviors;
using Odex.AspNetCore.Clarc.Application.Configuration;

namespace Odex.AspNetCore.Clarc.Application;

/// <summary>
/// Registers MediatR handlers, FluentValidation validators, pipeline behaviors, and options.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Clarc application services using a fluent builder.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Configures assemblies and optional behaviors.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    public static IServiceCollection AddClarcApplication(this IServiceCollection services, Action<ClarcApplicationBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var builder = new ClarcApplicationBuilder();
        configure(builder);
        return services.AddClarcApplication(builder.Build());
    }

    /// <summary>
    /// Adds MediatR registration for the assembly containing <typeparamref name="T"/>, FluentValidation validators from the same assembly,
    /// default pipeline behaviors, and options.
    /// </summary>
    /// <typeparam name="T">Any reference type defined in the assembly to scan for handlers and validators.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    public static IServiceCollection AddClarcApplication<T>(this IServiceCollection services) where T : class =>
        services.AddClarcApplication(b => b.AddAssemblyContaining<T>());

    /// <summary>
    /// Adds Clarc application services from a built <see cref="ClarcApplicationDescriptor"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="descriptor">Registration model produced by <see cref="ClarcApplicationBuilder"/>.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    public static IServiceCollection AddClarcApplication(this IServiceCollection services, ClarcApplicationDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        if (descriptor.Assemblies.Count == 0)
        {
            throw new ArgumentException("At least one assembly is required.", nameof(descriptor));
        }

        services.AddLogging();

        services.AddOptions<ClarcValidationPipelineOptions>();
        services.Configure<ClarcValidationPipelineOptions>(o => descriptor.ConfigureValidation?.Invoke(o));

        services.AddOptions<ClarcLoggingPipelineOptions>();
        services.Configure<ClarcLoggingPipelineOptions>(o => descriptor.ConfigureLogging?.Invoke(o));

        services.AddMediatR(cfg =>
        {
            foreach (var assembly in descriptor.Assemblies)
            {
                cfg.RegisterServicesFromAssembly(assembly);
            }

            descriptor.ConfigureMediatR?.Invoke(cfg);
        });

        services.AddValidatorsFromAssemblies(descriptor.Assemblies, descriptor.ValidatorLifetime);

        if (descriptor.EnableUnhandledExceptionPipeline)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionLoggingPipelineBehavior<,>));
        }

        if (descriptor.EnableLoggingPipeline)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        }

        if (descriptor.EnableMetricsPipeline)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestMetricsPipelineBehavior<,>));
        }

        if (descriptor.EnableValidationPipeline)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        }

        return services;
    }
}

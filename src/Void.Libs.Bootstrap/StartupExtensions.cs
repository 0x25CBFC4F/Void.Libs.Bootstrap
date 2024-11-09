using Microsoft.Extensions.DependencyInjection.Extensions;
using Void.Libs.Bootstrap.Base;
using Void.Libs.Bootstrap.Enums;
using Void.Libs.Bootstrap.Middlewares;
using Void.Libs.Bootstrap.Pipeline;
using Void.Libs.Bootstrap.Services;

namespace Void.Libs.Bootstrap;

/// <summary>
/// Startup extensions for <see cref="WebApplicationBuilder"/>.
/// </summary>
public static class StartupExtensions
{
    private static readonly Type StartupPipelineType = typeof(AbstractBootstrapPipeline);
    
    /// <summary>
    /// Registers startup pipeline services.
    /// </summary>
    public static void UseBootstrap(this IServiceCollection services, ErrorBootstrapBehavior bootstrapBehavior = ErrorBootstrapBehavior.ExitOnError)
    {
        // Checking if consumer registered a startup pipeline.
        if (services.All(service => service.ServiceType != StartupPipelineType))
            throw new InvalidOperationException($"Startup pipeline of type '{StartupPipelineType.FullName}' must be registered in DI before calling {nameof(UseBootstrap)}.");
        
        // Registering background service to handle bootstrap actions.
        services.AddSingleton<IBootstrapService>(sp => ActivatorUtilities.CreateInstance<BootstrapService>(sp, bootstrapBehavior));
        services.AddHostedService(sp => sp.GetRequiredService<IBootstrapService>());
        
        // Registering middleware to handle responses during bootstrap process.
        services.AddSingleton<BootstrapMiddleware>();
    }

    /// <summary>
    /// Registers a startup pipeline.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="startupPipeline">Instance of your pipeline.</param>
    /// <typeparam name="TStartupPipeline">Type of your pipeline.</typeparam>
    public static void AddBootstrapPipeline<TStartupPipeline>(this IServiceCollection services, TStartupPipeline startupPipeline)
        where TStartupPipeline : AbstractBootstrapPipeline
    {
        services.TryAddSingleton<AbstractBootstrapPipeline>(startupPipeline);
    }
    
    public static void AddBootstrapPipeline(this IServiceCollection services, Action<DefaultStartupPipelineBuilder> configure)
    {
        var builder = new DefaultStartupPipelineBuilder();
        configure(builder);
        
        services.TryAddSingleton<AbstractBootstrapPipeline>(builder.Build());
    }

    /// <summary>
    /// Adds <see cref="BootstrapMiddleware"/> to the request pipeline.
    /// </summary>
    public static void UseBootstrapMiddleware(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<BootstrapMiddleware>();
    }
}

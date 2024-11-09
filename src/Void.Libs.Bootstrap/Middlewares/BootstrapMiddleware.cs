using System.Net;
using Void.Libs.Bootstrap.Enums;
using Void.Libs.Bootstrap.Models;
using Void.Libs.Bootstrap.Services;

namespace Void.Libs.Bootstrap.Middlewares;

/// <summary>
/// Startup middleware.
/// </summary>
public class BootstrapMiddleware : IMiddleware
{
    private readonly IBootstrapService _bootstrapService;
    private BootstrapState _currentState = BootstrapState.InProgress;
    
    /// <summary>
    /// Startup middleware.
    /// </summary>
    public BootstrapMiddleware(IBootstrapService bootstrapService)
    {
        _bootstrapService = bootstrapService;
        _bootstrapService.Subscribe(FinalStateHasChanged);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // If current state is successful - passthrough all requests.
        if (_currentState == BootstrapState.Successful)
        {
            await next(context);
            return;
        }
        
        // Otherwise - return 'maintenance' error.
        context.Response.StatusCode = (int) HttpStatusCode.ServiceUnavailable;

        var response = new BootstrapResponse { State = _currentState };

        if (_currentState == BootstrapState.InProgress)
        {
            context.Response.Headers.Append("Retry-After", "1");
            response.CurrentTask = _bootstrapService.CurrentTaskName;
        }

        context.Response.Headers.Append("X-Maintenance", "true");
        context.Response.Headers.Append("Cache-Control", "no-cache");
        
        await context.Response.WriteAsJsonAsync(response);
    }

    private void FinalStateHasChanged(BootstrapState state)
    {
        _currentState = state;
    }
}
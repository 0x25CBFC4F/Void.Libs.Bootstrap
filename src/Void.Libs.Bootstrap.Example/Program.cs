using Void.Libs.Bootstrap;
using Void.Libs.Bootstrap.Base;
using Void.Libs.Bootstrap.Enums;
using Void.Libs.Bootstrap.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddBootstrapPipeline(x =>
{
    x.Use<ExampleBootstrapAction>();
    x.Use<SecondBootstrapAction>();
});

builder.Services.UseBootstrap(ErrorBootstrapBehavior.Continue);

var app = builder.Build();

app.Services.GetRequiredService<IBootstrapService>();

app.UseBootstrapMiddleware();
app.MapControllers();
app.Run();

internal class ExampleBootstrapAction : IBootstrapPipelineAction
{
    public string Name => "Example action";
    
    public async Task<bool> Invoke(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        return true;
    }
}

internal class SecondBootstrapAction : IBootstrapPipelineAction
{
    public string Name => "Second action";
    
    public async Task<bool> Invoke(CancellationToken cancellationToken)
    {
        return true;
    }
}
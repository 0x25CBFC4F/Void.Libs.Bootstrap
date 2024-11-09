# 🚀 Void.Libs.Bootstrap

**Lightweight package to execute bootstrap actions in your service.**

[![Documentation build](https://build.voids.site/app/rest/builds/buildType:(id:Libraries_VoidLibsBootstrap_BuildDocumentation),count:1,defaultFilter:false/statusIcon)](https://build.voids.site/buildConfiguration/Libraries_VoidLibsBootstrap_BuildDocumentation)
### [Documentation](https://bootstrapdocs.voids.site/)

### Install:
```bash
dotnet add Void.Libs.Bootstrap
```

### Integrates as easy as:

```csharp
builder.Services.AddBootstrapPipeline(x =>
{
    x.Use<ExampleBootstrapAction>();
});

builder.Services.UseBootstrap(ErrorBootstrapBehavior.Continue);

// < .. after build .. >

app.UseBootstrapMiddleware();
```


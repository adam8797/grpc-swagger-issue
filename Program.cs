using Grpc.Core;
using GrpcSwaggerIssue;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
});

builder.WebHost.UseKestrel(x =>
{
    x.ListenAnyIP(5000, b => b.Protocols = HttpProtocols.Http1);
    x.ListenAnyIP(5001, b => b.Protocols = HttpProtocols.Http2);
});

var app = builder.Build();

app.UseSwagger();

// Configure the HTTP request pipeline.
app.MapGrpcService<ConcreteThingService>();
app.MapGrpcService<ConcreteWidgetService>();

app.Run();

public class ConcreteThingService : ThingService.ThingServiceBase
{
    public override Task<ThingResponse> GetThing(ThingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new ThingResponse() { Name = "Thing: " + request.Name });
    }
}

public class ConcreteWidgetService : WidgetService.WidgetServiceBase
{
    public override Task<WidgetResponse> GetWidget(WidgetRequest request, ServerCallContext context)
    {
        return Task.FromResult(new WidgetResponse() { Name = "Widget: " + request.Name });
    }
}

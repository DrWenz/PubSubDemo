using Microsoft.AspNetCore.Mvc;
using PubSubServer;
using PubSubServer.Hub;
using PubSubServer.Node;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDevelopment",
        builder => builder
            .SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ConsumesAttribute("application/json"));
});
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddSignalR();

builder.Services.AddScoped<IAppHubService, AppHubService>();

builder.Services.AddHostedService<PumpSimulator>();
builder.Services.AddHostedService<TemperaturSimulator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("LocalDevelopment");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AppHub>("/api/v1/hub");

// Setup default notes
using var scope = app.Services.CreateScope();
var appHubService = scope.ServiceProvider.GetRequiredService<IAppHubService>();
NodeManager.Init(appHubService);
NodeManager.SetValue("Pump-Flow", 0);
NodeManager.SetValue("Pump-Heat", 0);
NodeManager.SetValue("Temperature", 0);


app.Run();
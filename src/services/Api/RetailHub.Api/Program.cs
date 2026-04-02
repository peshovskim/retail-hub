using System.Diagnostics;
using Catalog.Application;
using Catalog.Application.Category.Queries.GetCategories;
using Catalog.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailHub.SharedKernel.Application.Common.Abstractions.DomainEvents;
using RetailHub.SharedKernel.Application.Common.Behaviors;
using RetailHub.SharedKernel.Application.Common.DomainEvents;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RetailHubDatabase");
ArgumentException.ThrowIfNullOrEmpty(connectionString);

builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
builder.Services.AddCatalogApplication();
builder.Services.AddCatalogInfrastructure(connectionString);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetCategoriesQuery).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.Register(() =>
        TryOpenSwaggerInBrowser(app));
}

app.UseHttpsRedirection();
app.UseCors("AngularDev");
app.MapControllers();
app.Run();

static void TryOpenSwaggerInBrowser(WebApplication webApp)
{
    try
    {
        var server = webApp.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
        if (addresses is null || addresses.Count == 0)
        {
            return;
        }

        var baseUrl = addresses.FirstOrDefault(static a => a.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            ?? addresses.FirstOrDefault();
        if (baseUrl is null)
        {
            return;
        }

        var swaggerUrl = $"{baseUrl.TrimEnd('/')}/swagger";
        Process.Start(new ProcessStartInfo { FileName = swaggerUrl, UseShellExecute = true });
    }
    catch
    {
    }
}

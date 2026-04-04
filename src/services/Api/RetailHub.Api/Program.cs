using System.Diagnostics;
using Cart.Application;
using Cart.Application.Cart.Queries.GetCart;
using Catalog.Application;
using Catalog.Application.Category.Queries.GetCategories;
using Cart.Infrastructure;
using Catalog.Infrastructure;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using RetailHub.SharedKernel.Application.Common.Abstractions.DomainEvents;
using RetailHub.SharedKernel.Application.Common.Behaviors;
using RetailHub.SharedKernel.Application.Common.DomainEvents;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RetailHubDatabase");
ArgumentException.ThrowIfNullOrEmpty(connectionString);

builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
builder.Services.AddCatalogApplication();
builder.Services.AddCartApplication();
builder.Services.AddCatalogInfrastructure(connectionString);
builder.Services.AddCartInfrastructure(connectionString);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(GetCategoriesQuery).Assembly, typeof(GetCartQuery).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "http://127.0.0.1:4200",
                "https://127.0.0.1:4200")
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

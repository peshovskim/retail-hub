using System.Diagnostics;
using System.Text;
using Cart.Application;
using Cart.Application.Cart.Queries.GetCart;
using Catalog.Application;
using Catalog.Application.Category.Queries.GetCategories;
using Cart.Infrastructure;
using Catalog.Infrastructure;
using Identity.Application;
using Identity.Application.User.Commands.Login;
using Identity.Application.User.Commands.RegisterUser;
using Identity.Infrastructure;
using Orders.Application;
using Orders.Application.Order.Queries.GetOrderById;
using Orders.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RetailHub.Api.Options;
using RetailHub.Api.Services;
using Identity.Application.User.Interfaces;
using RetailHub.SharedKernel.Application.Common.Abstractions.DomainEvents;
using RetailHub.SharedKernel.Application.Common.Behaviors;
using RetailHub.SharedKernel.Application.Common.DomainEvents;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RetailHubDatabase");
ArgumentException.ThrowIfNullOrEmpty(connectionString);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddSingleton<ITokenIssuer, JwtTokenIssuer>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserAccessor, HttpContextCurrentUserAccessor>();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException($"Configuration section '{JwtOptions.SectionName}' is missing.");
ArgumentException.ThrowIfNullOrEmpty(jwtOptions.SigningKey, nameof(jwtOptions.SigningKey));
ArgumentException.ThrowIfNullOrEmpty(jwtOptions.Issuer, nameof(jwtOptions.Issuer));
ArgumentException.ThrowIfNullOrEmpty(jwtOptions.Audience, nameof(jwtOptions.Audience));

byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Keep JWT claim types as issued (e.g. `uid`, `sub`) so HttpContextCurrentUserAccessor can resolve the user id reliably.
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
builder.Services.AddCatalogApplication();
builder.Services.AddCartApplication();
builder.Services.AddOrdersApplication();
builder.Services.AddIdentityApplication();
builder.Services.AddCatalogInfrastructure(connectionString);
builder.Services.AddCartInfrastructure(connectionString);
builder.Services.AddOrdersInfrastructure(connectionString);
builder.Services.AddIdentityInfrastructure(connectionString);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(GetCategoriesQuery).Assembly,
        typeof(GetCartQuery).Assembly,
        typeof(GetOrderByIdQuery).Assembly,
        typeof(RegisterUserCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RetailHub API", Version = "v1" });
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Bearer token (include: Authorization: Bearer {token})",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            },
            Array.Empty<string>()
        },
    });
});

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
app.UseAuthentication();
app.UseAuthorization();
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

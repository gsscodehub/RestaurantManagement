using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantManagement.Application.Extensions;
using RestaurantManagement.Domain.Interfaces.Security;
using RestaurantManagement.Infrastructure.Extensions;
using RestaurantManagement.Infrastructure.Security;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Centralized registration
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

// JWT generator DI (Api references Infrastructure, so this is valid)
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Restaurant Management API",
        Version = "v1",
        Description = "API for managing restaurant operations"
    });

    // ??z Add JWT Bearer definition so Swagger shows Authorize button
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token.\nExample: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//  Add CORS policy

builder.Services.AddCors(options =>
{ 
    options.AddPolicy("MyReactPolicy", policy => 
    {
      policy.WithOrigins("http://localhost:5173" , "https://localhost:7252")
      // React dev server
       .AllowAnyHeader() 
       .AllowAnyMethod()
       .AllowCredentials(); // if you plan to send cookies/JWT via browser
    }); 
});

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", opt =>
{
    opt.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            ctx.Token = ctx.Request.Cookies["AuthToken"];
            return Task.CompletedTask;
        }
    };
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  Apply CORS before authentication/authorization
app.UseCors("MyReactPolicy");

app.UseAuthentication();
app.UseAuthorization();

//  Global RBAC enforcement — skip only Swagger endpoints
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    if (path != null && path.StartsWith("/swagger"))
    {
        await next(context);
        return;
    }

    var resolver = context.RequestServices.GetRequiredService<IRightResolver>();
    var middleware = new RbacAuthorizationMiddleware(next, resolver);
    await middleware.InvokeAsync(context);
});

app.MapControllers();

app.Run();

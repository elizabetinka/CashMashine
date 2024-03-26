using Lab5.Infrastructure.DataAccess.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using WebApplication6;
using AuthorizationMiddleware = WebApplication6.AuthorizationMiddleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// получаем строку подключения из файла конфигурации
string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new AggregateException();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<
    IErrorDescriber, ErrorDescriber>();
builder.Services.ExtentionInf(connection);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler, AuthorizationMiddleware>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Set Title and version
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cash machine service", Version = "v1", Description = "The API for my application" });

    options.EnableAnnotations();
    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Write your data.",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic",
                },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services.AddAuthentication("BasicAuthentication").Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler, AuthorizationMiddleware>();
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
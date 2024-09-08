using System.Text.Json;
using AutoMapper;
using GoldenCastle.Govhack2024.Api;
using GoldenCastle.Govhack2024.Middleware;
using Microsoft.OpenApi.Models;
using Refit;

const string allowAllOrigins = nameof(allowAllOrigins);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRefitClient<IHomesApi>((_) => new RefitSettings
    {
        ContentSerializer = new SystemTextJsonContentSerializer(
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            }
        )
    })
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.GetConnectionString("HomesApiUrl")))
    .AddHttpMessageHandler(serviceProvider 
        => new HttpLoggingHandler(serviceProvider.GetRequiredService<ILogger<HttpLoggingHandler>>()))
    .Services.AddSingleton<HttpLoggingHandler>();

builder.Services.AddRefitClient<IHomesGatewayApi>((_) => new RefitSettings
    {
        ContentSerializer = new SystemTextJsonContentSerializer(
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            }
        )
    })
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.GetConnectionString("HomesGatewayApiUrl")))
    .AddHttpMessageHandler(serviceProvider 
        => new HttpLoggingHandler(serviceProvider.GetRequiredService<ILogger<HttpLoggingHandler>>()))
    .Services.AddSingleton<HttpLoggingHandler>();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAllOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "2024GovhackBackend", Version = "v1" });
});

builder.Services.AddAutoMapper(typeof(PropertyProfile));

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<PropertyProfile>();
});
mapperConfig.AssertConfigurationIsValid();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(allowAllOrigins);

app.Run();

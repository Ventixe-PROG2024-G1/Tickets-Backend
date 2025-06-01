using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Presentation.Extensions.Middlewares;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddMemoryCache();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TicketsDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddScoped<ITicketRepository, TicketRepository>();

builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Lägg till detta:
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key måste anges i headern som 'X-API-KEY: {nyckel}'",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY", // eller t.ex. "Authorization"
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});



var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API"));
app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyHeader());

app.MapOpenApi();


app.UseHttpsRedirection();

app.UseMiddleware<DefaultApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

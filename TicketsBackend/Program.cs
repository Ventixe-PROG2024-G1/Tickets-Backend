using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
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

using AuthenticationSample;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("AmadeusApi",
    client => { client.BaseAddress = new Uri("https://test.api.amadeus.com"); });
builder.Services.AddScoped<AmadeusApi>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/GetBusiestTravelPeriodsOfYear", (
        [FromServices] IConfiguration config,
        [FromServices] IHttpClientFactory handler
    ) =>
    {
        var api = new AmadeusApi(config, handler);
        api.ConnectOAuth().Wait();
        var busiest = api.GetBusiestTravelPeriodsOfYear("BOS", 2017).GetAwaiter().GetResult();
        return busiest;
    })
    .WithName("Travel")
    .WithOpenApi();


app.Run();
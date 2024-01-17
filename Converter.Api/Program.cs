using Currency.BusinessLogic.Contract;
using Currency.BusinessLogic.Implement;
using Currency.Converter.Api;
using Currency.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dbConnection));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICurrencyConverter, CurrencyConverter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS
var allowedOrigin = builder.Configuration.GetValue<string>("AllowedOrigin");
app.UseCors(policyBuilder =>
{
    policyBuilder.SetIsOriginAllowed(x =>
            string.IsNullOrEmpty(allowedOrigin) || x.Equals(allowedOrigin, StringComparison.OrdinalIgnoreCase))
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromDays(30));
});

app.UseRouting();

app.MapControllers();

app.Run();
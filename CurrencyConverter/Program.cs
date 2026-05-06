using CurrencyConverter.DTO;
using CurrencyConverter.Models;
using CurrencyConverter.Services;

namespace CurrencyConverter;

public class Program
{
    static string _baseUri = "https://api.frankfurter.dev/v1/latest";
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddHttpClient<CurrencyService>(client => client.BaseAddress = new Uri(_baseUri));
        builder.Services.AddMemoryCache();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();

        app.MapGet("/api/currencies", async (CurrencyService service, string currency) =>
        {
            if(!currency.IsValidCurrencyCode())
                return Results.BadRequest(new {Message = "Неверный код валюты"});
            try
            {
                API_Obj? apiObj = await service.GetCurrency(currency);
                return Results.Json(apiObj);
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(new {Message = e.Message});
            }
            catch (Exception e)
            {
                return Results.NotFound(new { Message = e.Message });
            }
        });

        app.MapGet("/api/convert", async (CurrencyService service, string from, string to, decimal amount) =>
        {
            if(amount <= 0)
                return Results.BadRequest(new {Message = "Сумма должна быть положительной"});
            if(!from.IsValidCurrencyCode() || !to.IsValidCurrencyCode())
                return Results.BadRequest(new {Message = "Неверный код валюты"});
            try
            {
                var cur = await service.GetCurrency(from);
                
                var convertCurrency = amount * cur?.Rates?.GetRate(to) 
                                      ?? throw new InvalidOperationException("Тарифы не найдены");
                return Results.Json(new CurrencyResponse(to, convertCurrency));
                
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(new {Message = e.Message});
            }
            catch (Exception e)
            {
                return Results.NotFound(new  {e.Message});
            }
            
            
        });

        app.Run();
    }
}


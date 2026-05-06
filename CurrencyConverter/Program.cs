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
            try
            {
                API_Obj? apiObj = await service.GetCurrency(currency);
                return Results.Json(apiObj);
            }
            catch (Exception e)
            {
                return Results.NotFound(new { Message = e.Message });
            }
        });

        app.MapGet("/api/convert", async (CurrencyService service, string from, string to, double amount) =>
        {
            try
            {
                var cur = await service.GetCurrency(from);
                if (cur != null)
                {
                    var convertCurrency = amount * cur.Rates.GetRate(to);
                    return Results.Json(new CurrencyResponse() {Currency = to, Amount =  convertCurrency});
                }
                else
                    return Results.NotFound(new { Message = $"Could not find currency {to}" });
            }
            catch (Exception e)
            {
                return Results.NotFound(new  {e.Message});
            }
            
            
        });

        app.Run();
    }
}


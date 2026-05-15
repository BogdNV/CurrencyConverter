using CurrencyConverter.Services;

namespace CurrencyConverter;

public class Program
{
    static string _api = "https://api.frankfurter.dev";
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddHttpClient<CurrencyService>(r => r.BaseAddress = new Uri(_api));
        builder.Services.AddMemoryCache();

        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.MapControllers();
        app.Run();
    }
}


using CurrencyConverter.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyConverter.Services;

public class CurrencyService
{
    private readonly ILogger<CurrencyService> _logger;
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;

    public CurrencyService(ILogger<CurrencyService> logger,  IMemoryCache cache,  HttpClient httpClient)
    {
        _logger = logger;
        _cache = cache;
        _httpClient = httpClient;
    }

    public async Task<API_Obj?> GetCurrency(string symbol = "EUR", CancellationToken ct = default)
    {
        symbol = symbol.ToUpperInvariant();

        return await _cache.GetOrCreateAsync(symbol, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromHours(1);

            var response = await _httpClient.GetFromJsonAsync<API_Obj>($"?base={symbol}", ct);
            _logger.LogInformation("Кэш обновлен для {Base}", response?.Base);
            return response;
        });
    }
}
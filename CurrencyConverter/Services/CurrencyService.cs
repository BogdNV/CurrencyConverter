using CurrencyConverter.DTO;
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

    public async Task<IEnumerable<Currency?>?> GetCurrency(string path, string query, CancellationToken ct = default)
    {
        IEnumerable<Currency?>? rates = await _httpClient.GetFromJsonAsAsyncEnumerable<Currency>($"{path}?{query}").ToArrayAsync();
        foreach (var cur in rates)
        {
            string key = $"{cur?.Base}_{cur?.Quote}";
            await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                _logger.LogInformation("Кэш обновлен для {Base}", key);
                return cur;
            });
        }
        return rates;
    }

    public IEnumerable<CurrencyResponse> Convert(IEnumerable<Currency?> rates, decimal amount, CancellationToken ct = default)
    {
        return rates.Select(r => new CurrencyResponse(r.Base, amount, r.Quote, r.Rate * amount));
    }
}
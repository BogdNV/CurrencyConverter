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

    public async Task<API_Obj?> GetCurrency(string symbol = "EUR")
    {
        symbol = symbol.ToUpper();
        if (!_cache.TryGetValue(symbol, out API_Obj? currency))
        {
            API_Obj? cur = await _httpClient.GetFromJsonAsync<API_Obj?>(_httpClient.BaseAddress + $"?base={symbol}");
            if (cur != null)
            {
                
                _cache.Set(cur.Base, cur, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
                _logger.LogInformation("Данные успешно добавлены в кэш {Data}", cur);
                return cur;
            }
            else
            {
                _logger.LogError("Валюта не найдена {Symbol}", symbol);
            }
        }

        _logger.LogInformation("Получение данных из кэша");
        return currency;
    }
}
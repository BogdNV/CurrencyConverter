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

    public async Task<IReadOnlyList<Currency>> GetCurrencyByBaseAsync(string path, string @base, CancellationToken ct = default)
    {
        var cacheKey = $"base={@base.ToUpperInvariant()}";
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var response = await _httpClient.GetFromJsonAsync<Currency[]>($"{path}?{cacheKey}");
            if (response is null || response.Length == 0)
                throw new InvalidOperationException($"Нет данных для валюты {@base}");
            
            _logger.LogInformation("Кэш обновлен: {Key} ({Count} курсов)",  @base, response.Length);
            return response.ToList().AsReadOnly();
        }) ?? throw new InvalidOperationException("Неудалось получить данные");
    }

    public IEnumerable<Currency> FilterByQuotes(IEnumerable<Currency> currencies, string targetQuotes)
    {
        if(string.IsNullOrWhiteSpace(targetQuotes))
            return currencies;
        var quotes = targetQuotes.Split(",").Select(q => q.ToUpperInvariant()).ToArray();

        return currencies
            .Where(c => quotes.Contains(c.Quote));
    }

    public IEnumerable<CurrencyResponse> Convert(IEnumerable<Currency> rates, decimal amount, CancellationToken ct = default)
    {
        return rates.Select(r => new CurrencyResponse(r.Base, amount, r.Quote, r.Rate * amount));
    }
}
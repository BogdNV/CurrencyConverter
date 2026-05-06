namespace CurrencyConverter.Models;

public static class DictionaryExtensions
{
    public static decimal GetRate(this IDictionary<string, decimal> rates, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentNullException(nameof(currency));
        var key = currency.ToUpperInvariant();
        
        return rates.TryGetValue(key, out var rate) ? rate : throw new ArgumentException($"Неизвестная валюта {currency}", nameof(currency));
    }

    public static bool IsValidCurrencyCode(this string code) =>
        !string.IsNullOrWhiteSpace(code)
        && code.Length == 3;
}
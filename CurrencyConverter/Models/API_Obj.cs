namespace CurrencyConverter.Models;

public record API_Obj(
    string Base,
    DateTime Date,
    Dictionary<string, decimal> Rates
);
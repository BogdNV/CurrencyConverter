namespace CurrencyConverter.DTO;

public record CurrencyResponse(string Base, decimal BaseAmount, string Quote, decimal QuoteAmount);
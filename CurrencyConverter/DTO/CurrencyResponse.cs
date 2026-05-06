namespace CurrencyConverter.DTO;

public record CurrencyResponse
{
    public string Currency { get; set; }
    public double Amount { get; set; }
}
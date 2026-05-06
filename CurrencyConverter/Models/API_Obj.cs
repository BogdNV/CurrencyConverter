namespace CurrencyConverter.Models;

public record API_Obj
{
    public string Base {get; init;}
    public DateTime Date { get; init; }
    public ConversionRate Rates { get; init; }
}
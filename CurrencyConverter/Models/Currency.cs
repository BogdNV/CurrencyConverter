namespace CurrencyConverter.Models;

public record Currency(DateTime Date, string Base, string Quote, decimal Rate);
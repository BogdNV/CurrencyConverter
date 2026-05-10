using System.ComponentModel.DataAnnotations;
using CurrencyConverter.Models;
using CurrencyConverter.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ILogger<CurrencyController> _logger;
    private readonly CurrencyService _currencyService;
    public CurrencyController(ILogger<CurrencyController> logger, CurrencyService currencyService)
    {
        _logger = logger;
        _currencyService = currencyService;
    }
    
    [HttpGet("{currency}")]
    public async Task<IActionResult> GetAll(string currency, CancellationToken cancellationToken)
    {
        var rates = await _currencyService
            .GetCurrencyByBaseAsync("/v2/rates", currency, cancellationToken);
        return Ok(rates);
    }

    [HttpGet("convert/{from}")]
    public async Task<IActionResult> Convert(string from, [Required][FromQuery]string to, [Required][FromQuery]decimal amount, CancellationToken cancellationToken)
    {
        var rates = await _currencyService
            .GetCurrencyByBaseAsync("/v2/rates", from, cancellationToken);
        var filterRates = _currencyService.FilterByQuotes(rates, to);

        var result = _currencyService.Convert(filterRates, amount, cancellationToken).ToList();
        return Ok(result);
    }
}
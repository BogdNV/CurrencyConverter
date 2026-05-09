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
        IEnumerable<Currency?>? rates = await _currencyService
            .GetCurrency("/v2/rates", $"base={currency}", cancellationToken);
        return Ok(rates);
    }

    [HttpGet("convert/{from}")]
    public async Task<IActionResult> Convert(string from, [Required][FromQuery]string to, [Required][FromQuery]decimal amount, CancellationToken cancellationToken)
    {
        IEnumerable<Currency?>? rates = await _currencyService
            .GetCurrency("/v2/rates", $"base={from}&quotes={to}", cancellationToken);
        if (rates == null)
            return NotFound();
        if (amount <= 0)
            return BadRequest(new { message = "Amount must be greater than zero." });
        
        return Ok(_currencyService.Convert(rates, amount, cancellationToken));
    }
}
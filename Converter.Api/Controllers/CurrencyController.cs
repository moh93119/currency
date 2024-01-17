using Currency.BusinessLogic.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Currency.Converter.Api.Controllers;

[Route("currency")]
public class CurrencyController(ICurrencyConverter currencyConverter) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> GetRates(CancellationToken ct)
    {
        return Ok();
    }
}
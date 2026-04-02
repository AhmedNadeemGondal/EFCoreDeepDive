using EFCoreDeepDive.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDeepDive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController(AppDBContext appDBContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCurrencies()
        {
            // DB calls are not performed in the controller
            // a seperate layer is created for these calls
            // for the purpose of the tutorial we are sticking to
            // the controller for now.

            // This is the non-standard approach, a DTO is used
            // to pass the data back to client, preserving internal
            // schema and providing a layer of seperation offering flexibility.
            //var result = appDBContext.Currencies.ToList();  // Form 1 of LINQ
            var result = await (from currencies in appDBContext.Currencies // Form 2 of LINQ
                                select currencies).ToListAsync();
            return Ok(result);
        }

        // Endpoint for demonstrating .FindAsync() LINQ
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurrencyByID([FromRoute(Name = "id")] int id)
        {
            var currency = await appDBContext.Currencies.FindAsync(id);

            if (currency == null)
            {
                return NotFound();
            }

            return Ok(currency);
        }

    }
}

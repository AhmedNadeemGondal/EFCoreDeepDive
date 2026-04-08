using EFCoreDeepDive.Data;
using EFCoreDeepDive.Data.DTO;
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
                                select new CurrencyDTO() // This will query only the selected columns
                                {
                                    CurrencyID = currencies.Id,
                                    Name = currencies.Title,
                                }).ToListAsync();
            return Ok(result);
        }

        // Endpoint for demonstrating .FindAsync() LINQ
        [HttpGet("{id:int}")] // Addind type allows for the next query to be in a non-conflict as well
        public async Task<IActionResult> GetCurrencyByID([FromRoute(Name = "id")] int id)
        {
            var currency = await appDBContext.Currencies.FindAsync(id);

            if (currency == null)
            {
                return NotFound();
            }

            return Ok(currency);
        }
        [HttpGet("{name}")] // Can't do ':string' as routes are string by default, throws runtime errors
        // Not a recommended routing scheme but good enough for focusing on EF Core and LINQ
        public async Task<IActionResult> GetCurrencyByName([FromRoute(Name = "name")] string name)
        {
            //var currency = await appDBContext.Currencies.Where(x => x.Title == name).FirstOrDefaultAsync(); 
            var currency = await appDBContext.Currencies.FirstOrDefaultAsync(x => x.Title == name); // functionally identical to the above
            // FirstAsync will throw an error if nothing is returned by the DB
            // FIrstOrDefaultAsync will return Null if nothing is found.
            // SingleAsync and SingleOrDefault have the above behaviour along with the added benefit/disadvantage
            // of throwing an error if more than one record is returned by the DB.

            if (currency == null)
            {
                return NotFound();
            }

            return Ok(currency);
        }

        // Query using multiple parameters
        [HttpGet("multiple/{name}")]

        public async Task<IActionResult> GetCurrencyByName(
                                            [FromRoute(Name = "name")] string name,
                                            [FromQuery(Name = "description")] string? description
                                                          )
        {
            //var currency = await appDBContext.Currencies.FirstOrDefaultAsync(
            //    x => 
            //    x.Title == name &&  ( string.IsNullOrEmpty(description) ||x.Description == description));
            var currency = await appDBContext.Currencies
                .Where( // This creates the filter before the followng .ToListAsync
                x => 
                x.Title == name &&  ( string.IsNullOrEmpty(description) ||x.Description == description))
                .ToListAsync();

            if (currency == null)
            {
                return NotFound();
            }

            return Ok(currency);
        }

        [HttpPost("multipleids")]

        public async Task<IActionResult> GetCurrencyByMultipleIds([FromBody] List<int> ids)
        {
            var currencies = await appDBContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .Select(s => new CurrencyDTO() // This will query only the selected columns
                {
                    CurrencyID = s.Id,
                    Name = s.Title,
                })
                .ToListAsync();

            if (currencies is null)
            {
                return NotFound();
            }

            return Ok(currencies);

        }



    }
}

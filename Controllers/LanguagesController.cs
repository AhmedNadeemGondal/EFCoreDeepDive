using EFCoreDeepDive.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDeepDive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController(AppDBContext appDBContext) : ControllerBase
    {

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var language = await appDBContext.Languages
                .Include(x => x.Books) // This will generate the quey for the Books navigation join
                                       // This is eager loading
                .AsNoTracking()
                .ToListAsync();

            return Ok(language);
        }
    }
}

using EFCoreDeepDive.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDeepDive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDBContext appDBContext) : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] BookDTO bookDto)
        {
            // The Mapping Logic
            var bookEntity = new Book
            {
                Title = bookDto.Title,
                Description = bookDto.Description,
                NoOfPages = bookDto.NoOfPages,
                IsActive = bookDto.IsActive,
                LanguageId = bookDto.LanguageId,
                //Language = null, This is not needed as the Navigation logic 
                // kicks in and fills this part based on the LanguageId
                CreatedOn = DateTime.Now // Server-side logic
            };

            // Fetching the related string as you did before, !!! See note in front
            // of Language = null comment
            //bookEntity.Language = (await appDBContext.Languages
            //    .FirstOrDefaultAsync(x => x.Id == bookDto.LanguageId))!;

            appDBContext.Books.Add(bookEntity);// This only updates in the memory change tracker
            await appDBContext.SaveChangesAsync(); // This actually forwards the query to the DB
            return NoContent();
        }
    }
}

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
                CreatedOn = DateTime.UtcNow, // Server-side logic
                Author = bookDto.Author!
            };

            // Fetching the related string as you did before, !!! See note in front
            // of Language = null comment
            //bookEntity.Language = (await appDBContext.Languages
            //    .FirstOrDefaultAsync(x => x.Id == bookDto.LanguageId))!;

            appDBContext.Books.Add(bookEntity);// This only updates in the memory change tracker
            await appDBContext.SaveChangesAsync(); // This actually forwards the query to the DB
            return Ok($"{bookEntity.Title} added successfully.");
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddNewBooks([FromBody] List<BookDTO> bookDtos)
        {
            if (bookDtos == null || !bookDtos.Any())
            {
                return BadRequest("Book list cannot be empty.");
            }

            // Map the list of DTOs to a list of Entities
            // Should use some automapper
            var bookEntities = bookDtos.Select(dto => new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                NoOfPages = dto.NoOfPages,
                IsActive = dto.IsActive,
                LanguageId = dto.LanguageId,
                CreatedOn = DateTime.UtcNow // Using UtcNow is generally better practice
            }).ToList();

            // Add the entire collection to the Change Tracker
            appDBContext.Books.AddRange(bookEntities); // This will result in a single SQL operation
            await appDBContext.SaveChangesAsync(); 

            return Ok($"{bookEntities.Count} books added successfully.");
        }
    }
}

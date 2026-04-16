using EFCoreDeepDive.Data;
using EFCoreDeepDive.Data.DTO;
using EFCoreDeepDive.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDeepDive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDBContext appDBContext) : ControllerBase
    {
        [HttpGet("allUsingProjection")] // Projection based mapping using .Select
        public async Task<ActionResult<List<BookDTO>>> GetAllBooksAsync()
        {
            var books = await appDBContext.Books
                .Select(x => new BookDTO
                // Book is being mapped to BookDTO manually
                {
                    Title = x.Title,
                    LanguageId = x.LanguageId,
                    IsActive = x.IsActive,
                    Description = x.Description,
                    NoOfPages = x.NoOfPages,
                    Language = x.Language,
                    Author = x.Author != null ? x.Author: null // This is a class, so will add a complete object
                })
                
                .AsNoTracking().ToListAsync();
            return Ok(books);
        }

        [HttpGet("allEagerLoad")] // Eager loading based mapping using .Include
        public async Task<ActionResult<List<BookDTO>>> GetAllBooksEagerAsync()
        {
            // Eager Loading happens here
            var books = await appDBContext.Books
                .Include(x => x.Language) // This will introduce infinite nesting if books
                                          // is passed to the Ok() method which trigerr the
                                          // JSONserializer
                .Include(x => x.Author)
                .AsNoTracking()
                .ToListAsync();

            // Mapping happens in memory AFTER the data is fetched
            var bookDTOs = books.Select(x => new BookDTO
            {
                Title = x.Title,
                Description = x.Description,
                NoOfPages = x.NoOfPages,
                IsActive = x.IsActive,
                LanguageId = x.LanguageId,
                Language = x.Language, // Already loaded via .Include
                Author = x.Author     // Already loaded via .Include
            }).ToList();

            return Ok(bookDTOs);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddNewBookAsync([FromBody] BookDTO bookDto)
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
        public async Task<IActionResult> AddNewBooksAsync([FromBody] List<BookDTO> bookDtos)
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

        [HttpPut("{id}")] // getting the id seperate is a best practice
        public async Task<IActionResult> UpdateBook([FromRoute(Name = "id")] int bookId, [FromBody] UpdateBookDTO bookDto)
        {

            var bookEntity = await appDBContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if (bookEntity == null)
            {
                return NotFound();
            }
            // This is the 'Property-by-Property' update logic.
            if (bookDto.Title != null) bookEntity.Title = bookDto.Title;
            if (bookDto.Description != null) bookEntity.Description = bookDto.Description;
            if (bookDto.NoOfPages.HasValue) bookEntity.NoOfPages = bookDto.NoOfPages.Value;
            if (bookDto.IsActive.HasValue) bookEntity.IsActive = bookDto.IsActive.Value;
            if (bookDto.LanguageId.HasValue) bookEntity.LanguageId = bookDto.LanguageId.Value;

            //appDBContext.Books.Add(bookEntity);// This will break this flow as it is not required
            // we are not creating a record, only updating the in-memory entity
            // and the tracker
            await appDBContext.SaveChangesAsync(); // This will generate and forward required SQL to the DB
                                                   //await appDBContext.UpdateAsync(bookEntity); // There are scenarios where update can be used as well
                                                   // This will work as the entry state is modified and
                                                   // tracked by the change tracker !!! See example below !!!
                                                   //appDBContext.Entry(bookEntity).State = EntityState.Modified; // manually modifying the ENUM
            return Ok($"{bookEntity.Title} updated successfully.");
        }

        [HttpPut("bulk")] // getting the id seperate is a best practice
        public async Task<IActionResult> UpdateBooksInBulkAsync()
        {
            await appDBContext.Books
                .Where(x => x.NoOfPages > 100)
                .ExecuteUpdateAsync // This generates a query and sends directly to the DB
                                    // No entity exists in this method.
                (x => x
            .SetProperty(p => p.Description, p => p.Title + " More than 100 pages")
            .SetProperty(p => p.Title, p => p.Title + " updated more than 100 pages")
            );

            return Ok("Bulk update succeded.");
        }

        [HttpDelete("{id}")] // getting the id seperate is a best practice
        public async Task<IActionResult> DeleteBookById([FromRoute(Name = "id")] int id)
        {
            //var bookToDelete = await appDBContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            //if (bookToDelete != null)
            //{
            //    appDBContext.Books.Remove(bookToDelete);
            //    await appDBContext.SaveChangesAsync();
            //    return Ok("Book deleted succeded.");

            //}

            //return NotFound("Book not found");

            var book = new Book { Id = id };
            appDBContext.Entry(book).State = EntityState.Deleted;
            await appDBContext.SaveChangesAsync();

            return Ok("Book deleted succeded.");
        }

        [HttpDelete("bulk")] // getting the id seperate is a best practice
        public async Task<IActionResult> DeleteBookInBulkAsync([FromRoute(Name = "id")] int id)
        {

            var booksToDelete = await appDBContext.Books.Where(x => x.Id < 5).ToListAsync();
            if (booksToDelete != null)
            {
                appDBContext.Books.RemoveRange(booksToDelete); // This will generate a query for each record deletion
                await appDBContext.SaveChangesAsync();

                //var books = await appDBContext.Books.ExecuteDeleteAsync(); deletes everything with a single query
                return Ok("Book deleted succeded.");

            }



            return NotFound("Book not found");
        }

    }
}

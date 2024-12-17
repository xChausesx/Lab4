using Lab4.Context;
using Lab4.Dtos;
using Lab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Controllers
{
    [ApiController]
    [Route("book")]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BookController(AppDbContext appContext)
        {
            _db = appContext;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var books = await _db.Books
                .Include(b => b.Authors)
                .Include(b => b.Keywords)
                .Select(b => new BookDto(b))
                .ToListAsync();

            if (books.Count == 0)
            {
                return NotFound();
            }

            return Ok(books);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _db.Books
                .Include(b => b.Authors)
                .Include(b => b.Keywords)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var result = new BookDto(book);

            return Ok(result);
        }

        [HttpGet("getFile")]
        public async Task<IActionResult> GetFile(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null || book.File == null)
            {
                return NotFound();
            }

            var fileBytes = book.File;
            var fileName = $"{book.Title}.pdf";
            var contentType = "application/pdf";

            return File(fileBytes, contentType, fileName);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Book book = new (bookDto);

            await book.InitializeAsync(_db);

            await _db.AddAsync(book);

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("edit")]
        public async Task<IActionResult> Edit(BookDto bookDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _db.Books
                .Include(b => b.Authors)
                .Include(b => b.Keywords)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null || book.File == null)
            {
                return BadRequest();
            }

            book.Update(bookDto);

            await book.InitializeAsync(_db);

            _db.Update(book);

            await _db.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return BadRequest();
            }

            _db.Remove(book);

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}

using Lab4.Context;
using Lab4.Dtos;
using Lab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Controllers
{
    [ApiController]
    [Route("author")]
    public class AuthorController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AuthorController(AppDbContext appContext)
        {
            _db = appContext;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _db.Authors
                .Select(a => new AuthorDto(a))
                .ToListAsync();

            if (authors.Count == 0)
            {
                return NotFound();
            }

            return Ok(authors);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _db.Authors
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            var result = new AuthorDto(author);

            return Ok(result);
        }

        [HttpGet("getFile")]
        public async Task<IActionResult> GetFile(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _db.Authors.FirstOrDefaultAsync(b => b.Id == id);

            if (author == null || author.Photo == null)
            {
                return NotFound();
            }

            var fileBytes = author.Photo;
            var fileName = $"{author.FullName}.jpeg";
            var contentType = "image/jpeg";

            return File(fileBytes, contentType, fileName);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author author = new (authorDto);

            await _db.AddAsync(author);

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("edit")]
        public async Task<IActionResult> Edit(AuthorDto authorDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _db.Authors
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            author.Update(authorDto);

            _db.Update(author);
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

            var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            _db.Remove(author);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}

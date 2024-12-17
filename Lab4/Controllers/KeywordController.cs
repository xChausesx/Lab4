using Lab4.Context;
using Lab4.Dtos;
using Lab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Controllers
{
    [ApiController]
    [Route("keyword")]
    public class KeywordController : ControllerBase
    {
        private readonly AppDbContext _db;

        public KeywordController(AppDbContext appContext)
        {
            _db = appContext;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var keywords = await _db.Keywords
                .Select(k => k.Word)
                .ToListAsync();

            if (keywords.Count == 0)
            {
                return NotFound();
            }

            return Ok(keywords);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var keyword = await _db.Keywords
                .FirstOrDefaultAsync(k => k.Id == id);

            if (keyword == null)
            {
                return NotFound();
            }

            return Ok(keyword.Word);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(KeywordDto keywordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Keyword keyword = new(keywordDto);

            await _db.AddAsync(keyword);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("edit")]
        public async Task<IActionResult> Edit(KeywordDto keywordDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var keyword = await _db.Keywords
                .FirstOrDefaultAsync(k => k.Id == id);

            if (keyword == null)
            {
                return NotFound();
            }

            if(string.IsNullOrEmpty(keywordDto.Word))
            {
                return BadRequest();
            }

            keyword.Word = keywordDto.Word;

            _db.Update(keyword);
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

            var keyword = await _db.Keywords.FirstOrDefaultAsync(k => k.Id == id);

            if (keyword == null)
            {
                return NotFound();
            }

            _db.Remove(keyword);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
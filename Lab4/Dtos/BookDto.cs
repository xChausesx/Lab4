using Lab4.Models;
using System.Text.Json.Serialization;

namespace Lab4.Dtos
{
    public class BookDto
    {
        public string? Title { get; set; }
        public int Pages { get; set; }
        public List<AuthorDto>? Authors { get; set; }
        public List<string>? Keywords { get; set; }
        
        [JsonIgnore]
        public IFormFile? File { get; set; }
        [JsonIgnore]
        public int[]? AuthorsIds { get; set; }
        [JsonIgnore]
        public int[]? KeywordsIds { get; set; }

        public BookDto() 
        {
            
        }
        public BookDto(Book book)
        {
            Title = book.Title;
            Pages = book.Pages;
            Authors = book.Authors.Select(a => new AuthorDto(a)).ToList();
            Keywords = book.Keywords.Select(k => k.Word).ToList();
        }
    }
}

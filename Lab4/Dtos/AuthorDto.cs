using Lab4.Models;
using Lab4.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lab4.Dtos
{
    public class AuthorDto
    {
        public string? FullName { get; set; }

        public string? ShortName { get; set; }

        public string? Bio { get; set; }
        [JsonIgnore]
        public IFormFile? ImageFile { get; set; }
        public AuthorDto() { }
        public AuthorDto(Author author)
        {
            FullName = author.FullName;
            ShortName = author.ShortName;
            Bio = author.Bio;
        }
    }
}

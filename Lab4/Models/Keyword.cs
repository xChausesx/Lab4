using Lab4.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class Keyword
    {
        public int Id { get; set; }

        [MaxLength(48)]
        public string? Word { get; set; }
        public List<Book>? Books { get; set; }

        public Keyword()
        {

        }

        public Keyword(KeywordDto keywordDto)
        {
            Word = keywordDto.Word;
        }
    }
}

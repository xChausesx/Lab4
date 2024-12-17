using Lab4.Dtos;
using Lab4.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string? FullName { get; set; }

        public string? ShortName { get; set; }

        public string? Bio { get; set; }

        public byte[]? Photo { get; set; }

        public List<Book>? Books { get; set; }

        private IFormFile? _formFile;

        [NotMapped]
        public IFormFile? PhotoFile
        {
            get => _formFile;
            set
            {
                Photo = FileService.ConvertToByteArr(value);
                _formFile = value;
            }
        }

        public void Update(AuthorDto author)
        {
            FullName = string.IsNullOrEmpty(author.FullName) ? FullName : author.FullName;
            ShortName = string.IsNullOrEmpty(author.ShortName) ? ShortName : author.ShortName;
            Bio = string.IsNullOrEmpty(author.Bio) ? Bio : author.Bio;

            if (author.ImageFile != null)
            {
                PhotoFile = author.ImageFile;
            }
        }
        public Author()
        {

        }

        public Author(AuthorDto authorDto)
        {
            FullName = authorDto.FullName;
            ShortName = authorDto.ShortName;
            Bio = authorDto.Bio;
            PhotoFile = authorDto.ImageFile;
        }
    }
}
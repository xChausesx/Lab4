using Lab4.Context;
using Lab4.Dtos;
using Lab4.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }

        [Required]
        public int Pages { get; set; }

        public byte[]? File { get; set; }

        public List<Author>? Authors { get; set; }

        public List<Keyword>? Keywords { get; set; }
        
        [NotMapped]
        public int[] SelectedAuthors { get; set; }
        
        [NotMapped]
        public int[] SelectedKeywords { get; set; }

        private IFormFile? _formFile;

        [NotMapped]
        public IFormFile? FormFile
        {
            get => _formFile;
            set
            {
                File = FileService.ConvertToByteArr(value);
                _formFile = value;
            }
        }

        public void Update(BookDto bookDto)
        {
            Title = string.IsNullOrEmpty(bookDto.Title) ? Title : bookDto.Title;
            Pages = int.IsPositive(bookDto.Pages) ? bookDto.Pages : Pages;
            SelectedAuthors = bookDto.AuthorsIds != null ? bookDto.AuthorsIds : SelectedAuthors;
            SelectedKeywords = bookDto.KeywordsIds!= null ? bookDto.KeywordsIds : SelectedKeywords;
            
            if(bookDto.File != null)
            {
                FormFile = bookDto.File;
            }
        }
        public async Task InitializeAsync(AppDbContext appDbContext)
        {
            Authors = await appDbContext.Authors
                .Where(a => SelectedAuthors.Contains(a.Id))
                .ToListAsync();

            Keywords = await appDbContext.Keywords
                .Where(a => SelectedKeywords.Contains(a.Id))
                .ToListAsync();
        }

        public Book()
        {

        }

        public Book(BookDto bookDto)
        {
            Title = bookDto.Title;
            Pages = bookDto.Pages;
            FormFile = bookDto.File;
            SelectedAuthors = bookDto.AuthorsIds;
            SelectedKeywords = bookDto.KeywordsIds;
        }
    }
}

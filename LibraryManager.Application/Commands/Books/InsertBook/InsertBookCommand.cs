using LibraryManager.Application.Models;
using LibraryManager.Core.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibraryManager.Application.Commands.Books.InsertBook
{
    public class InsertBookCommand : IRequest<ResultViewModel<int>>
    {
        [Required(ErrorMessage = "O título do livro é obrigatório")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O autor do livro é obrigatório")]
        public string Author { get; set; }

        [Required(ErrorMessage = "O ISBN do livro é obrigatório")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "O ano de publicação é obrigatório")]
        [Range(1, 9999, ErrorMessage = "O ano de publicação deve ser maior que zero")]
        public int PublicationYear { get; set; }

        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB
        public IFormFile? ImageFile { get; set; }

        public Book ToEntity()
        {
            return new Book
            {
                Title = Title,
                Author = Author,
                ISBN = ISBN,
                PublicationYear = PublicationYear,
                IsAvailable = true,
                ImageUrl = string.Empty
            };
        }
    }
}

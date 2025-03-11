using LibraryManager.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Application.Models
{
    public class CreateBookInputModel
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

        public Book ToEntity()
        {
            return new Book
            {
                Title = Title,
                Author = Author,
                ISBN = ISBN,
                PublicationYear = PublicationYear,
                IsAvailable = true
            };
        }
    }
}

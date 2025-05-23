﻿using LibraryManager.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Application.Models
{
    public class UpdateBookInputModel
    {
        public int IdBook { get; set; }

        [Required(ErrorMessage = "O título do livro é obrigatório")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O autor do livro é obrigatório")]
        public string Author { get; set; }

        [Required(ErrorMessage = "O ISBN do livro é obrigatório")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "O ano de publicação é obrigatório")]
        [Range(1, 9999, ErrorMessage = "O ano de publicação deve ser maior que zero")]
        public int PublicationYear { get; set; }

        public void UpdateEntity(Book book)
        {
            book.Title = Title;
            book.Author = Author;
            book.ISBN = ISBN;
            book.PublicationYear = PublicationYear;
        }
    }
}

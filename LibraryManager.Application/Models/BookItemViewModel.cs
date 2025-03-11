using LibraryManager.Core.Entities;

namespace LibraryManager.Application.Models
{
    public class BookItemViewModel(int id, string title, string author, string iSBN, int publicationYear, bool isAvailable)
    {
        public int Id { get; set; } = id;
        public string Title { get; set; } = title;
        public string Author { get; set; } = author;
        public string ISBN { get; set; } = iSBN;
        public int PublicationYear { get; set; } = publicationYear;
        public bool IsAvailable { get; set; } = isAvailable;

        public static BookItemViewModel FromEntity(Book book) => new(book.Id, book.Title, book.Author, book.ISBN, book.PublicationYear, book.IsAvailable);
    }
}

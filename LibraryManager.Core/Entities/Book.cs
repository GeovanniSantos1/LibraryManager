namespace LibraryManager.Core.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublicationYear { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Book() : base()
        {
        }

        public Book(string title, string author, string isbn, int publicationYear) : base()
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            PublicationYear = publicationYear;
        }

        public override string ToString()
        {
            return $"ID: {Id} | Título: {Title} | Autor: {Author} | ISBN: {ISBN} | Ano: {PublicationYear} | Disponível: {(IsAvailable ? "Sim" : "Não")}";
        }
    }
}
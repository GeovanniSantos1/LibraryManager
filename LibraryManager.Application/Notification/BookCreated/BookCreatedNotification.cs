using LibraryManager.Core.Entities;
using MediatR;

namespace LibraryManager.Application.Notification.BookCreated
{
    public class BookCreatedNotification : INotification
    {
        public BookCreatedNotification(Book book)
        {
            Book = book;
        }

        public Book Book { get; set; }
    }
}

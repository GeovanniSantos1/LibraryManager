using MediatR;

namespace LibraryManager.Application.Notification.BookCreated
{
    public class GenerateBookBoardHandler : INotificationHandler<BookCreatedNotification>
    {
        public Task Handle(BookCreatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Gerando quadro para o livro: {notification.Book.Title}");

            return Task.CompletedTask;
        }
    }
}

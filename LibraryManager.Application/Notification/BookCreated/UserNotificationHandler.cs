using MediatR;

namespace LibraryManager.Application.Notification.BookCreated
{
    public class UserNotificationHandler : INotificationHandler<BookCreatedNotification>
    {
        public Task Handle(BookCreatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Enviando notificação para o usuário sobre o livro: {notification.Book.Title}");
            return Task.CompletedTask;
        }
    }
}

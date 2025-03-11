using FluentAssertions;
using LibraryManager.Application.Commands.Books.InsertBook;
using LibraryManager.Application.Notification.BookCreated;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.UnitTests.Fakes;
using MediatR;
using Moq;
using Xunit;

namespace LibraryManager.UnitTests.Application.Books
{
    public class InsertBookHandlerTests : BookFakeDataHelper
    {
        private readonly Mock<IBookRepository> _repositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly InsertBookHandler _handler;

        public InsertBookHandlerTests()
        {
            _repositoryMock = new Mock<IBookRepository>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new InsertBookHandler(_repositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_WhenISBNNotExists_ShouldInsertSuccessfully()
        {
            // Arrange
            var book = _bookFaker.Generate();
            var command = new InsertBookCommand
            {
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear
            };

            _repositoryMock.Setup(r => r.GetAll())
                .ReturnsAsync(new List<Book>());

            _repositoryMock.Setup(r => r.Add(It.IsAny<Book>()))
               .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(1);
            result.Message.Should().BeEmpty();
            _repositoryMock.Verify(r => r.Add(It.IsAny<Book>()), Times.Once);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<BookCreatedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenValidBook_ShouldInsertSuccessfully()
        {
            // Arrange
            var book = _bookFaker.Generate();
            var command = new InsertBookCommand
            {
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear
            };

            _repositoryMock.Setup(r => r.GetAll())
                .ReturnsAsync(new List<Book>());

            _repositoryMock.Setup(r => r.Add(It.IsAny<Book>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(1);
            result.Message.Should().BeEmpty();
            _repositoryMock.Verify(r => r.Add(It.IsAny<Book>()), Times.Once);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<BookCreatedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
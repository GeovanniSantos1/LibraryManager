using FluentAssertions;
using LibraryManager.Application.Commands.Books.UpdateBook;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.UnitTests.Fakes;
using Moq;

namespace LibraryManager.UnitTests.Application.Books
{
    public class UpdateBookHandlerTests : BookFakeDataHelper
    {
        private readonly Mock<IBookRepository> _repositoryMock;
        private readonly UpdateBookHandler _handler;

        public UpdateBookHandlerTests()
        {
            _repositoryMock = new Mock<IBookRepository>();
            _handler = new UpdateBookHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenBookExists_ShouldUpdateSuccessfully()
        {
            // Arrange
            var book = _bookFaker.Generate();
            var command = new UpdateBookCommand
            {
                IdBook = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear
            };

            _repositoryMock.Setup(r => r.GetById(book.Id))
                .ReturnsAsync(book);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Book>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeEmpty(); 
            _repositoryMock.Verify(r => r.Update(It.Is<Book>(b => b.Id == book.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUpdateFails_ShouldReturnError()
        {
            // Arrange
            var book = _bookFaker.Generate();
            var command = new UpdateBookCommand
            {
                IdBook = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear
            };

            _repositoryMock.Setup(r => r.GetById(book.Id))
                .ReturnsAsync(book);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Book>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Não foi possível atualizar o livro.");
            _repositoryMock.Verify(r => r.Update(It.Is<Book>(b => b.Id == book.Id)), Times.Once);
        }
    }
}
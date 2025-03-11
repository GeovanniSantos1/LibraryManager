using FluentAssertions;
using LibraryManager.Application.Commands.Books.DeleteBook;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.UnitTests.Fakes;
using Moq;
using Xunit;

namespace LibraryManager.UnitTests.Application.Books
{
    public class DeleteBookHandlerTests : BookFakeDataHelper
    {
        private readonly Mock<IBookRepository> _repositoryMock;
        private readonly DeleteBookHandler _handler;

        public DeleteBookHandlerTests()
        {
            _repositoryMock = new Mock<IBookRepository>();
            _handler = new DeleteBookHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenBookExists_ShouldDeleteSuccessfully()
        {
            // Arrange
            var book = _bookFaker.Generate();
            var command = new DeleteBookCommand(book.Id);

            _repositoryMock.Setup(r => r.GetById(book.Id))
                .ReturnsAsync(book);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Book>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeEmpty();  // Alterado de BeNull para BeEmpty
            _repositoryMock.Verify(r => r.Update(It.Is<Book>(b => b.Id == book.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenBookNotFound_ShouldReturnError()
        {
            // Arrange
            var command = new DeleteBookCommand(1);

            _repositoryMock.Setup(r => r.GetById(1))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Livro não encontrado.");
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ShouldReturnError()
        {
            // Arrange
            var book = _bookFaker.Generate();
            var command = new DeleteBookCommand(book.Id);

            _repositoryMock.Setup(r => r.GetById(book.Id))
                .ReturnsAsync(book);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Book>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Não foi possível deletar o livro.");
        }
    }
}
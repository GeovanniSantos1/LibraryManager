using FluentAssertions;
using LibraryManager.Application.Commands.Loans.InsertLoan;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.UnitTests.Fakes;
using Moq;
using Xunit;

namespace LibraryManager.UnitTests.Application.Loans
{
    public class InsertLoanHandlerTests : LoanFakeDataHelper
    {
        private readonly Mock<IloanRepository> _loanRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly InsertLoanHandler _handler;

        public InsertLoanHandlerTests()
        {
            _loanRepositoryMock = new Mock<IloanRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new InsertLoanHandler(_loanRepositoryMock.Object, _bookRepositoryMock.Object, _userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldReturnError()
        {
            // Arrange
            var command = new InsertLoanCommand { UserId = 1, BookId = 1 };

            _userRepositoryMock.Setup(r => r.GetById(1))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Usuário não encontrado.");
            _bookRepositoryMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
            _loanRepositoryMock.Verify(r => r.Add(It.IsAny<Loan>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenBookNotFound_ShouldReturnError()
        {
            // Arrange
            var user = _userFaker.Generate();
            var command = new InsertLoanCommand { UserId = user.Id, BookId = 1 };

            _userRepositoryMock.Setup(r => r.GetById(user.Id))
                .ReturnsAsync(user);
            _bookRepositoryMock.Setup(r => r.GetById(1))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Livro não encontrado.");
            _loanRepositoryMock.Verify(r => r.Add(It.IsAny<Loan>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenBookNotAvailable_ShouldReturnError()
        {
            // Arrange
            var user = _userFaker.Generate();
            var book = _bookFaker.Generate();
            book.IsAvailable = false;

            var command = new InsertLoanCommand { UserId = user.Id, BookId = book.Id };

            _userRepositoryMock.Setup(r => r.GetById(user.Id))
                .ReturnsAsync(user);
            _bookRepositoryMock.Setup(r => r.GetById(book.Id))
                .ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Livro não está disponível para empréstimo.");
            _loanRepositoryMock.Verify(r => r.Add(It.IsAny<Loan>()), Times.Never);
        }
    }
}
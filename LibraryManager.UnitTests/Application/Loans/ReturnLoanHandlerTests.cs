using FluentAssertions;
using LibraryManager.Application.Commands.Loans.LoanReturn;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.UnitTests.Fakes;
using Moq;

namespace LibraryManager.UnitTests.Application.Loans
{
    public class LoanReturnHandlerTests : LoanFakeDataHelper
    {
        private readonly Mock<IloanRepository> _loanRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly LoanReturnHandler _handler;

        public LoanReturnHandlerTests()
        {
            _loanRepositoryMock = new Mock<IloanRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _handler = new LoanReturnHandler(_loanRepositoryMock.Object, _bookRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenLoanNotFound_ShouldReturnError()
        {
            // Arrange
            var command = new LoanReturnCommand(1);

            _loanRepositoryMock.Setup(r => r.GetById(1))
                .ReturnsAsync((Loan)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Empréstimo não encontrado.");
            _bookRepositoryMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
            _loanRepositoryMock.Verify(r => r.Update(It.IsAny<Loan>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenBookAlreadyReturned_ShouldReturnError()
        {
            // Arrange
            var loan = new Loan(1, 1) { ActualReturnDate = DateTime.Now };
            var command = new LoanReturnCommand(loan.Id);

            _loanRepositoryMock.Setup(r => r.GetById(loan.Id))
                .ReturnsAsync(loan);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Este livro já foi devolvido.");
            _bookRepositoryMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
            _loanRepositoryMock.Verify(r => r.Update(It.IsAny<Loan>()), Times.Never);
        }
        [Fact]
        public async Task Handle_WhenBookNotFound_ShouldStillProcessReturn()
        {
            // Arrange
            var loan = new Loan(1, 1);
            var command = new LoanReturnCommand(loan.Id);

            _loanRepositoryMock.Setup(r => r.GetById(loan.Id))
                .ReturnsAsync(loan);
            _bookRepositoryMock.Setup(r => r.GetById(loan.BookId))
                .ReturnsAsync((Book)null);
            _bookRepositoryMock.Setup(r => r.Update(It.IsAny<Book>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            _bookRepositoryMock.Verify(r => r.Update(It.IsAny<Book>()), Times.Never);
            _loanRepositoryMock.Verify(r => r.Update(It.Is<Loan>(l => l.ActualReturnDate.HasValue)), Times.Once);
        }
    }
}
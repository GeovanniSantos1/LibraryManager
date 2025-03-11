using FluentAssertions;
using LibraryManager.Application.Commands.Loans.DeleteLoan;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.UnitTests.Fakes;
using Moq;
using Xunit;

namespace LibraryManager.UnitTests.Application.Loans
{
    public class DeleteLoanHandlerTests : LoanFakeDataHelper
    {
        private readonly Mock<IloanRepository> _repositoryMock;
        private readonly DeleteLoanHandler _handler;

        public DeleteLoanHandlerTests()
        {
            _repositoryMock = new Mock<IloanRepository>();
            _handler = new DeleteLoanHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenLoanNotFound_ShouldReturnError()
        {
            // Arrange
            var command = new DeleteLoanCommand(1);

            _repositoryMock.Setup(r => r.GetById(1))
                .ReturnsAsync((Loan)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Empréstimo não encontrado.");
            _repositoryMock.Verify(r => r.GetById(1), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Loan>()), Times.Never);
        }
    }
}
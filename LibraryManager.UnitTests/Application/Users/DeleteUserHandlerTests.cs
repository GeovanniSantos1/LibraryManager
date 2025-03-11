using FluentAssertions;
using LibraryManager.Application.Commands.Users.DeleteUser;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.UnitTests.Fakes;
using Moq;
using Xunit;

namespace LibraryManager.UnitTests.Application.Users
{
    public class DeleteUserHandlerTests : UserFakeDataHelper
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _handler = new DeleteUserHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenUserExists_ShouldDeleteSuccessfully()
        {
            // Arrange
            var user = _userFaker.Generate();
            var command = new DeleteUserCommand(user.Id);

            _repositoryMock.Setup(r => r.GetById(user.Id))
                .ReturnsAsync(user);
            _repositoryMock.Setup(r => r.HasActiveLoans(user.Id))
                .ReturnsAsync(false);
            _repositoryMock.Setup(r => r.Update(It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(user.Id);
            _repositoryMock.Verify(r => r.Update(It.Is<User>(u => u.Id == user.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldReturnError()
        {
            // Arrange
            var command = new DeleteUserCommand(1);

            _repositoryMock.Setup(r => r.GetById(1))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Usuário não encontrado.");
            _repositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenUserHasActiveLoans_ShouldReturnError()
        {
            // Arrange
            var user = _userFaker.Generate();
            var command = new DeleteUserCommand(user.Id);

            _repositoryMock.Setup(r => r.GetById(user.Id))
                .ReturnsAsync(user);
            _repositoryMock.Setup(r => r.HasActiveLoans(user.Id))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Não é possível excluir um usuário com empréstimos ativos.");
            _repositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        }
    }
}
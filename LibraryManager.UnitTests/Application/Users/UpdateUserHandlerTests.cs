using FluentAssertions;
using LibraryManager.Application.Commands.Users.UpdateUser;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.UnitTests.Fakes;
using Moq;
using Xunit;

namespace LibraryManager.UnitTests.Application.Users
{
    public class UpdateUserHandlerTests : UserFakeDataHelper
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly UpdateUserHandler _handler;

        public UpdateUserHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _handler = new UpdateUserHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenUserExists_ShouldUpdateSuccessfully()
        {
            // Arrange
            var user = _userFaker.Generate();
            var command = new UpdateUserCommand(user.Id, "Updated Name", "updated@email.com");

            _repositoryMock.Setup(r => r.GetById(user.Id))
                .ReturnsAsync(user);
            _repositoryMock.Setup(r => r.Update(It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(user.Id);
            _repositoryMock.Verify(r => r.Update(It.Is<User>(u =>
                u.Id == user.Id &&
                u.Name == command.Name &&
                u.Email == command.Email)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldReturnError()
        {
            // Arrange
            var command = new UpdateUserCommand(1, "Test", "test@email.com");

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

        [Theory]
        [InlineData("", "email@test.com")]
        [InlineData(null, "email@test.com")]
        public async Task Handle_WhenNameIsInvalid_ShouldReturnError(string name, string email)
        {
            // Arrange
            var command = new UpdateUserCommand(1, name, email);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("O nome do usuário é obrigatório.");
            _repositoryMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData("John Doe", "")]
        [InlineData("John Doe", null)]
        public async Task Handle_WhenEmailIsInvalid_ShouldReturnError(string name, string email)
        {
            // Arrange
            var command = new UpdateUserCommand(1, name, email);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("O email do usuário é obrigatório.");
            _repositoryMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
        }
    }
}
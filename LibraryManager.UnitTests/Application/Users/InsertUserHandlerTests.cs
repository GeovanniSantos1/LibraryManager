using FluentAssertions;
using LibraryManager.Application.Commands.Users.InsertUser;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.Infrastructure.Auth;
using LibraryManager.UnitTests.Fakes;
using Moq;

namespace LibraryManager.UnitTests.Application.Users
{
    public class InsertUserHandlerTests : UserFakeDataHelper
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly InsertUserHandler _handler;

        public InsertUserHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>(); 
            _handler = new InsertUserHandler(_repositoryMock.Object, authServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidUser_ShouldInsertSuccessfully()
        {
            // Arrange
            var user = _userFaker.Generate();
            var command = new InsertUserCommand
            {
                Name = user.Name,
                Email = user.Email
            };

            _repositoryMock.Setup(r => r.Add(It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(user.Id);
            _repositoryMock.Verify(r => r.Add(It.Is<User>(u =>
                u.Name == user.Name &&
                u.Email == user.Email)), Times.Once);
        }

        [Theory]
        [InlineData("", "email@test.com")]
        [InlineData(null, "email@test.com")]
        public async Task Handle_WhenNameIsInvalid_ShouldReturnError(string name, string email)
        {
            // Arrange
            var command = new InsertUserCommand { Name = name, Email = email };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("O nome do usuário é obrigatório.");
            _repositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
        }

        [Theory]
        [InlineData("John Doe", "")]
        [InlineData("John Doe", null)]
        public async Task Handle_WhenEmailIsInvalid_ShouldReturnError(string name, string email)
        {
            // Arrange
            var command = new InsertUserCommand { Name = name, Email = email };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("O email do usuário é obrigatório.");
            _repositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
        }
    }
}
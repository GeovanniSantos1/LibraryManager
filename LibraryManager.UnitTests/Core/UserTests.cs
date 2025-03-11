using Bogus;
using FluentAssertions;
using LibraryManager.Core.Entities;
using LibraryManager.UnitTests.Core.Fakes;
using Xunit;

namespace LibraryManager.UnitTests.Core
{
    public class UserTests
    {
        private readonly Faker<User> _userFaker;

        public UserTests()
        {
            _userFaker = UserFakeBuilder.UserFaker();
        }

        [Fact]
        public void Constructor_WhenCalled_ShouldInitializeProperties()
        {
            // Arrange & Act
            var user = _userFaker.Generate();

            // Assert
            user.Name.Should().NotBeNullOrEmpty();
            user.Email.Should().NotBeNullOrEmpty();
            user.IsDeleted.Should().BeFalse();
            user.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void ToString_WhenCalled_ShouldReturnFormattedString()
        {
            // Arrange
            var user = _userFaker.Generate();

            // Act
            var result = user.ToString();

            // Assert
            result.Should().Contain(user.Name);
            result.Should().Contain(user.Email);
        }

        [Fact]
        public void SetAsDeletedAsync_WhenCalled_ShouldMarkAsDeleted()
        {
            // Arrange
            var user = _userFaker.Generate();

            // Act
            user.SetAsDeletedAsync();

            // Assert
            user.IsDeleted.Should().BeTrue();
        }
    }
}
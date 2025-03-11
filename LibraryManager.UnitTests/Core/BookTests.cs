using Bogus;
using FluentAssertions;
using LibraryManager.Core.Entities;
using LibraryManager.UnitTests.Core.Fakes;
using Xunit;

namespace LibraryManager.UnitTests.Core
{
    public class BookTests
    {
        private readonly Faker<Book> _bookFaker;

        public BookTests()
        {
            _bookFaker = BookFakeBuilder.BookFaker();
        }

        [Fact]
        public void Constructor_WhenCalled_ShouldInitializeProperties()
        {
            // Arrange & Act
            var book = _bookFaker.Generate();

            // Assert
            book.Title.Should().NotBeNullOrEmpty();
            book.Author.Should().NotBeNullOrEmpty();
            book.ISBN.Should().NotBeNullOrEmpty();
            book.PublicationYear.Should().BeInRange(1900, 2024);
            book.IsAvailable.Should().BeTrue();
            book.IsDeleted.Should().BeFalse();
            book.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void ToString_WhenCalled_ShouldReturnFormattedString()
        {
            // Arrange
            var book = _bookFaker.Generate();

            // Act
            var result = book.ToString();

            // Assert
            result.Should().Contain(book.Title);
            result.Should().Contain(book.Author);
            result.Should().Contain(book.ISBN);
            result.Should().Contain(book.PublicationYear.ToString());
            result.Should().Contain("Sim"); // IsAvailable = true
        }

        [Fact]
        public void SetAsDeletedAsync_WhenCalled_ShouldMarkAsDeleted()
        {
            // Arrange
            var book = _bookFaker.Generate();

            // Act
            book.SetAsDeletedAsync();

            // Assert
            book.IsDeleted.Should().BeTrue();
        }
    }
}
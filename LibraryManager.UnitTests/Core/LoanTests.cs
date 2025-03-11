using Bogus;
using FluentAssertions;
using LibraryManager.Core.Entities;
using LibraryManager.UnitTests.Core.Fakes;
using Xunit;

namespace LibraryManager.UnitTests.Core
{
    public class LoanTests
    {
        private readonly Faker<Loan> _loanFaker;

        public LoanTests()
        {
            _loanFaker = LoanFakeBuilder.LoanFaker();
        }

        [Fact]
        public void Constructor_WhenCalled_ShouldInitializeProperties()
        {
            // Arrange & Act
            var loan = _loanFaker.Generate();

            // Assert
            loan.UserId.Should().BeInRange(1, 100);
            loan.BookId.Should().BeInRange(1, 100);
            loan.LoanDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            loan.ReturnDate.Should().BeCloseTo(DateTime.Now.AddDays(14), TimeSpan.FromSeconds(1));
            loan.ActualReturnDate.Should().BeNull();
            loan.IsDeleted.Should().BeFalse();
            loan.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void GetDelayedDays_WhenNotReturnedAndPastDue_ShouldReturnDelayedDays()
        {
            // Arrange
            var loan = _loanFaker.Generate();
            loan.ReturnDate = DateTime.Now.AddDays(-5);

            // Act
            var delayedDays = loan.GetDelayedDays();

            // Assert
            delayedDays.Should().Be(5);
        }

        [Fact]
        public void GetDelayedDays_WhenReturnedLate_ShouldReturnDelayedDays()
        {
            // Arrange
            var loan = _loanFaker.Generate();
            loan.ReturnDate = DateTime.Now.AddDays(-5);
            loan.ActualReturnDate = DateTime.Now;

            // Act
            var delayedDays = loan.GetDelayedDays();

            // Assert
            delayedDays.Should().Be(5);
        }

        [Fact]
        public void GetDelayedDays_WhenReturnedOnTime_ShouldReturnZero()
        {
            // Arrange
            var loan = _loanFaker.Generate();
            loan.ReturnDate = DateTime.Now.AddDays(5);
            loan.ActualReturnDate = DateTime.Now;

            // Act
            var delayedDays = loan.GetDelayedDays();

            // Assert
            delayedDays.Should().Be(0);
        }

        [Fact]
        public void SetAsDeletedAsync_WhenCalled_ShouldMarkAsDeleted()
        {
            // Arrange
            var loan = _loanFaker.Generate();

            // Act
            loan.SetAsDeletedAsync();

            // Assert
            loan.IsDeleted.Should().BeTrue();
        }
    }
}
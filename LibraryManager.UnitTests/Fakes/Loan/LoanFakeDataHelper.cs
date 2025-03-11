using Bogus;
using LibraryManager.Core.Entities;

namespace LibraryManager.UnitTests.Fakes
{
    public abstract class LoanFakeDataHelper
    {
        protected readonly Faker<Loan> _loanFaker;
        protected readonly Faker<Book> _bookFaker;
        protected readonly Faker<User> _userFaker;

        protected LoanFakeDataHelper()
        {
            _bookFaker = new Faker<Book>()
                .CustomInstantiator(f => new Book(
                    f.Commerce.ProductName(),
                    f.Name.FullName(),
                    f.Commerce.Ean13(),
                    f.Random.Int(1900, 2024)
                ));

            _userFaker = new Faker<User>()
                .CustomInstantiator(f => new User(
                    f.Name.FullName(),
                    f.Internet.Email(),
                    f.Rant.Random.Words(),
                    f.PickRandom("Admin", "User")
                ));

            _loanFaker = new Faker<Loan>()
                .CustomInstantiator(f => new Loan(
                    f.Random.Int(1, 100),  // UserId
                    f.Random.Int(1, 100)   // BookId
                ));
        }
    }
}
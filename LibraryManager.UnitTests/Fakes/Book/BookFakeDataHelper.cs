using Bogus;
using LibraryManager.Core.Entities;

namespace LibraryManager.UnitTests.Fakes
{
    public abstract class BookFakeDataHelper
    {
        protected readonly Faker<Book> _bookFaker;

        protected BookFakeDataHelper()
        {
            _bookFaker = new Faker<Book>()
                .CustomInstantiator(f => new Book(
                    f.Commerce.ProductName(),     
                    f.Name.FullName(),          
                    f.Commerce.Ean13(),          
                    f.Random.Int(1900, 2025)  
                ));
        }
    }
}
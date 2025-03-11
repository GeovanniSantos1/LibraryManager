using Bogus;
using LibraryManager.Core.Entities;

namespace LibraryManager.UnitTests.Core.Fakes
{
    public static class BookFakeBuilder
    {
        public static Faker<Book> BookFaker()
        {
            return new Faker<Book>()
                .CustomInstantiator(f => new Book(
                    f.Commerce.ProductName(),
                    f.Name.FullName(),
                    f.Commerce.Ean13(),
                    f.Random.Int(1900, 2024)
                ));
        }
    }
}
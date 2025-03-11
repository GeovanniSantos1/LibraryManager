using Bogus;
using LibraryManager.Core.Entities;

namespace LibraryManager.UnitTests.Core.Fakes
{
    public static class LoanFakeBuilder
    {
        public static Faker<Loan> LoanFaker()
        {
            return new Faker<Loan>()
                .CustomInstantiator(f => new Loan(
                    f.Random.Int(1, 100),
                    f.Random.Int(1, 100)
                ));
        }
    }
}
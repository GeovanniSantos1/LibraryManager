using Bogus;
using LibraryManager.Core.Entities;

namespace LibraryManager.UnitTests.Fakes
{
    public abstract class UserFakeDataHelper
    {
        protected readonly Faker<User> _userFaker;

        protected UserFakeDataHelper()
        {
            _userFaker = new Faker<User>()
                .CustomInstantiator(f => new User(
                    f.Name.FullName(),
                    f.Internet.Email(),
                    f.Rant.Random.Words(),
                    f.PickRandom("Admin", "User")
                ));
        }
    }
}
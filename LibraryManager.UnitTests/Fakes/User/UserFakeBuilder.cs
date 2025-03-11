using Bogus;
using LibraryManager.Core.Entities;

namespace LibraryManager.UnitTests.Core.Fakes
{
    public static class UserFakeBuilder
    {
        public static Faker<User> UserFaker()
        {
            return new Faker<User>()
                .CustomInstantiator(f => new User(
                    f.Name.FullName(),
                    f.Internet.Email(),
                    f.Rant.Random.Words(),
                    f.PickRandom("Admin", "User")
                ));
        }
    }
}
using Bogus;
using CommonTestUtilities.Cryptography;
using Habbits.Domain.Entities;
using Habbits.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static User Build()
        {
            var passwordEncripter = new PasswordEncripterBuilder().Build();

            var user = new Faker<User>()
                .RuleFor(u => u.Id, _ => Guid.NewGuid())
                .RuleFor(u => u.Name, faker => faker.Name.FullName())
                .RuleFor(u => u.Email, faker => faker.Internet.Email())
                .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
                .RuleFor(u => u.Habits, _ => new List<Habit>());

            return user;
        }
    }
}

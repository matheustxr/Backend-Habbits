using Bogus;
using Habbits.Communication.Requests.User;

namespace CommonTestUtilities.Requests.User
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build()
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(user => user.Password, faker => faker.Internet.Password())
                .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
        }
    }
}

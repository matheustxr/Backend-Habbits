using FluentAssertions;
using FluentValidation;
using Habbits.Communication.Requests.Users;

namespace Validator.Test.Users
{
    public class PasswordValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("aaa")]
        [InlineData("aaaa")]
        [InlineData("aaaaa")]
        [InlineData("aaaaaa")]
        [InlineData("aaaaaaa")]
        [InlineData("aaaaaaaa")]
        [InlineData("Aaaaaaaa")]
        [InlineData("Aaaaaaa1")]
        public void Error_Password_Invalid(string password)
        {
            var validator = new Habbits.Application.UseCases.Users.PasswordValidator<RequestRegisterUserJson>();
            var result = validator
                .IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

            result.Should().BeFalse();
        }
    }
}

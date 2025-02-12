using Habbits.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        private readonly Mock<IPasswordEncripter> _mock;

        public PasswordEncripterBuilder()
        {
            _mock = new Mock<IPasswordEncripter>();

            _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!% dlfjkd545");
        }

        public PasswordEncripterBuilder Verify(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);
            }

            return this;
        }

        public IPasswordEncripter Build() => _mock.Object;
    }
}

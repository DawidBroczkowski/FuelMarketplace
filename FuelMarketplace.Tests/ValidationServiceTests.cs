using FluentAssertions;
using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Application.Services;

namespace FuelMarketplace.Tests
{
    public class ValidationServiceTests
    {
        private readonly IValidationService _validationService;
        public ValidationServiceTests()
        {
            _validationService = new ValidationService();
        }

        [Theory]
        [InlineData("email@gmail.com", true)]
        [InlineData("email@gmail", false)]
        [InlineData("email", false)]
        [InlineData("email@", false)]
        [InlineData("email@.com", false)]
        [InlineData(" email@gmail.com", true)]
        [InlineData("@gmail.com", false)]
        [InlineData("email@example.com", true)]
        [InlineData("firstname.lastname@example.com", true)]
        [InlineData("email@subdomain.example.com", true)]
        [InlineData("firstname+lastname@example.com", true)]
        [InlineData("email@123.123.123.123", true)]
        [InlineData("email@[123.123.123.123]", true)]
        [InlineData("\"email\"@example.com", true)]
        [InlineData("1234567890@example.com", true)]
        [InlineData("email@example-one.com", true)]
        [InlineData("_______@example.com", true)]
        [InlineData("email@example.name", true)]
        [InlineData("email@example.museum", true)]
        [InlineData("email@example.co.jp", true)]
        [InlineData("firstname-lastname@example.com", true)]
        [InlineData("plainaddress", false)]
        [InlineData("#@%^%#$@#$@#.com", false)]
        [InlineData("@example.com", false)]
        [InlineData("Joe Smith <email@example.com>", false)]
        [InlineData("email.example.com", false)]
        [InlineData("email@example@example.com", false)]
        [InlineData(".email@example.com", false)]
        [InlineData("email.@example.com", false)]
        [InlineData("email..email@example.com", false)]
        [InlineData("あいうえお@example.com", false)]
        [InlineData("email@example.com (Joe Smith)", false)]
        [InlineData("email@example", false)]
        [InlineData("email@-example.com", false)]
        [InlineData("email@example..com", false)]
        [InlineData("Abc..123@example.com", false)]
        [InlineData("", false)]
        public void ValidationService_EmailValidation_ReturnBool(string email, bool result)
        {
            var isValid = _validationService.ValidateEmail(email);

            isValid.Should().Be(result);
        }
    }
}

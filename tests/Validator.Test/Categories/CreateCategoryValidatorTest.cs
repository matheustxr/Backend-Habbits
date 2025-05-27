using FluentValidation.TestHelper;
using Habits.Application.UseCases.Categories;
using Habits.Communication.Requests.Categories;
using Habits.Exception;

namespace Validator.Test.Categories
{
    public class CreateCategoryValidatorTest
    {
        private readonly CategoryValidator _validator;

        public CreateCategoryValidatorTest()
        {
            _validator = new CategoryValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Error_Category_EmptyOrWhitespace(string category)
        {
            var request = new RequestCategoryJson { Category = category };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Category)
                  .WithErrorMessage(ResourceErrorMessages.TITLE_EMPTY);
        }

        [Fact]
        public void Success_Valid_Category()
        {
            var request = new RequestCategoryJson { Category = "Health" };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

using FluentValidation;
using Habits.Communication.Requests.Categories;
using Habits.Exception;

namespace Habits.Application.UseCases.Categories
{
    public class CategoryValidator : AbstractValidator<RequestCategoryJson>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Category).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_EMPTY);
        }
    }
}

using FluentValidation;
using Habits.Communication.Requests.Habits;
using Habits.Exception;

namespace Habits.Application.UseCases.Habits.Create
{
    public class CreateHabitValidator : AbstractValidator<RequestCreateHabitJson>
    {
        public CreateHabitValidator()
        {
            RuleFor(habit => habit.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_EMPTY);
            RuleFor(habit => habit.WeekDays).NotEmpty().WithMessage(ResourceErrorMessages.WEEKDAYS_EMPTY);
            RuleFor(habit => habit.Description).MaximumLength(500).WithMessage(ResourceErrorMessages.DESCRIPTION_TOO_LONG);
        }
    }
}

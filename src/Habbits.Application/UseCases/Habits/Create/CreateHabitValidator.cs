using FluentValidation;
using Habbits.Communication.Requests.Habits;
using Habbits.Communication.Requests.Users;
using Habbits.Exception;

namespace Habbits.Application.UseCases.Habit.Create
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

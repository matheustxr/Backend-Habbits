using FluentValidation.TestHelper;
using Habbits.Application.UseCases.Habit.Create;
using Habbits.Communication.Enums;
using Habbits.Communication.Requests.Habits;

namespace Validator.Test.Habits.Create
{
    public class CreateHabitValidatorTest
    {
        private readonly CreateHabitValidator _validator;

        public CreateHabitValidatorTest()
        {
            _validator = new CreateHabitValidator();
        }

        [Fact]
        public void Should_HaveError_When_TitleIsEmpty()
        {
            var request = new RequestCreateHabitJson { Title = "" };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(h => h.Title);
        }

        [Fact]
        public void Should_HaveError_When_WeekDaysAreEmpty()
        {
            var request = new RequestCreateHabitJson { Title = "Test", WeekDays = new List<WeekDays>() };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(h => h.WeekDays);
        }

        [Fact]
        public void Should_NotHaveError_When_DataIsValid()
        {
            var request = new RequestCreateHabitJson
            {
                Title = "Workout",
                WeekDays = new List<WeekDays> { WeekDays.Monday, WeekDays.Wednesday }
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

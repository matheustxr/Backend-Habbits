﻿using FluentValidation.TestHelper;
using Habits.Communication.Requests.Habits;
using Habits.Communication.Enums;
using Habits.Application.UseCases.Habits;

namespace Validator.Test.Habits
{
    public class CreateHabitValidatorTest
    {
        private readonly HabitValidator _validator;

        public CreateHabitValidatorTest()
        {
            _validator = new HabitValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Should_HaveError_When_TitleIsEmpty(string title)
        {
            var request = new RequestHabitJson
            {
                Title = title,
                WeekDays = new List<WeekDays> { WeekDays.Monday }
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Title);
        }

        [Fact]
        public void Should_HaveError_When_WeekDaysAreEmpty()
        {
            var request = new RequestHabitJson { Title = "Test", WeekDays = new List<WeekDays>() };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(h => h.WeekDays);
        }

        [Fact]
        public void Should_NotHaveError_When_DataIsValid()
        {
            var request = new RequestHabitJson
            {
                Title = "Workout",
                WeekDays = new List<WeekDays> { WeekDays.Monday, WeekDays.Wednesday }
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Habits;
using CommonTestUtilities.Requests.Habits;
using FluentAssertions;
using Habbits.Application.UseCases.Habit.Create;
using Habbits.Exception;
using Habbits.Exception.ExceptionBase;

namespace UseCases.Habits.Create
{
    public class CreateHabitUseCaseTest
    {
        [Fact]
        public async Task Sucess()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestCreateHabitJsonHabitBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Title.Should().Be(request.Title);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestCreateHabitJsonHabitBuilder.Build();

            request.Title = string.Empty;

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Which.GetErrors().Should().HaveCount(1);
            result.Which.GetErrors().Should().Contain(ResourceErrorMessages.TITLE_REQUIRED);
        }

        private CreateHabitUseCase CreateUseCase(Habbits.Domain.Entities.User user)
        {
            var habitWriteOnlyRepository = HabitsWriteOnlyRepositoryBuilder.Build();

            var habitReadOnlyRepository = HabitsReadOnlyRepositoryBuilder.Build();

            var unityOfWork = UnityOfWorkBuilder.Build();

            var mapper = MapperBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new CreateHabitUseCase(mapper, habitReadOnlyRepository, habitWriteOnlyRepository, unityOfWork);
        }
    }
}

using AutoMapper;
using FluentAssertions;
using Habbits.Application.UseCases.Habit.Create;
using Habbits.Communication.Enums;
using Habbits.Communication.Requests.Habits;
using Habbits.Domain.Entities;
using Habbits.Domain.Repositories;
using Habbits.Domain.Repositories.Habits;
using Habbits.Exception;
using Habbits.Exception.ExceptionBase;
using Moq;

namespace UseCases.Test.Habits.Create
{
    public class CreateHabitUseCaseTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHabitReadOnlyRepository> _habitReadOnlyRepositoryMock;
        private readonly Mock<IHabitWriteOnlyRepository> _habitWriteOnlyRepositoryMock;
        private readonly Mock<IUnityOfWork> _unitOfWorkMock;
        private readonly CreateHabitUseCase _useCase;

        public CreateHabitUseCaseTest()
        {
            _mapperMock = new Mock<IMapper>();
            _habitReadOnlyRepositoryMock = new Mock<IHabitReadOnlyRepository>();
            _habitWriteOnlyRepositoryMock = new Mock<IHabitWriteOnlyRepository>();
            _unitOfWorkMock = new Mock<IUnityOfWork>();

            _useCase = new CreateHabitUseCase(
                _mapperMock.Object,
                _habitReadOnlyRepositoryMock.Object,
                _habitWriteOnlyRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task Should_CreateHabit_When_DataIsValid()
        {
            var request = new RequestCreateHabitJson
            {
                Title = "Exercise",
                Description = "Go to the gym",
                WeekDays = new List<WeekDays> { WeekDays.Monday, WeekDays.Wednesday },
                IsActive = true,
                UserId = Guid.NewGuid()
            };

            var habit = new Habit { Title = request.Title };

            _habitReadOnlyRepositoryMock
                .Setup(repo => repo.ExistActiveHabitWithTitle(request.Title))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(mapper => mapper.Map<Habit>(request))
                .Returns(habit);

            var response = await _useCase.Execute(request);

            _habitWriteOnlyRepositoryMock.Verify(r => r.Add(It.IsAny<Habit>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);

            response.Title.Should().Be(request.Title);
        }

        [Fact]
        public async Task Should_ThrowError_When_TitleAlreadyExists()
        {
            var request = new RequestCreateHabitJson { Title = "Exercise" };

            _habitReadOnlyRepositoryMock
                .Setup(repo => repo.ExistActiveHabitWithTitle(request.Title))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => _useCase.Execute(request));

            exception.GetErrors().Should().Contain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
        }

        /*
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
            result.Which.GetErrors().Should().Contain(ResourceErrorMessages.TITLE_EMPTY);
        }

        [Fact]
        public async Task Error_WeekDays_Empty()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestCreateHabitJsonHabitBuilder.Build();
            request.WeekDays.Clear(); // Simulando lista vazia

            var useCase = CreateUseCase(loggedUser);
            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Which.GetErrors().Should().Contain(ResourceErrorMessages.WEEKDAYS_EMPTY);
        }

        [Fact]
        public async Task Error_Description_TooLong()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestCreateHabitJsonHabitBuilder.Build();
            request.Description = new string('A', 501); // Descrição com 501 caracteres

            var useCase = CreateUseCase(loggedUser);
            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Which.GetErrors().Should().Contain(ResourceErrorMessages.DESCRIPTION_TOO_LONG);
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
        */
    }
}

using AutoMapper;
using FluentAssertions;
using Habits.Communication.Enums;
using Habits.Communication.Requests.Habits;
using Habits.Domain.Entities;
using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Habits;
using Habits.Exception;
using Habits.Exception.ExceptionBase;
using Habits.Application.UseCases.Habits.Create;
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
            var request = new RequestHabitJson
            {
                Title = "Exercise",
                Description = "Go to the gym",
                WeekDays = new List<WeekDays> { WeekDays.Monday, WeekDays.Wednesday },
                IsActive = true,
                UserId = Guid.NewGuid()
            };

            var habit = new Habit { Title = request.Title };

            _habitReadOnlyRepositoryMock
                .Setup(repo => repo.ExistHabitWithTitle(request.Title, null))
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
            var request = new RequestHabitJson
            {
                Title = "Exercise",
                Description = "Any description",
                WeekDays = new List<WeekDays> { WeekDays.Monday },
                IsActive = true,
                UserId = Guid.NewGuid()
            };

            _habitReadOnlyRepositoryMock
                .Setup(repo => repo.ExistHabitWithTitle(request.Title, null))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => _useCase.Execute(request));

            exception.GetErrors().Should().Contain(ResourceErrorMessages.TITLE_ALREADY_REGISTERED);
        }
    }
}

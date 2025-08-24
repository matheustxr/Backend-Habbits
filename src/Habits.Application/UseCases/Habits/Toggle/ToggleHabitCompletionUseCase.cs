using Habits.Domain.Repositories;
using Habits.Domain.Repositories.DayHabits;
using Habits.Domain.Repositories.Habits;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Habits.ToggleCompletion
{
    public class ToggleHabitCompletionUseCase : IToggleHabitCompletionUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IHabitReadOnlyRepository _habitReadOnlyRepository;
        private readonly IDayHabitWriteOnlyRepository _dayHabitWriteOnlyRepository;
        private readonly IUnityOfWork _unityOfWork;

        public ToggleHabitCompletionUseCase(
            ILoggedUser loggedUser,
            IHabitReadOnlyRepository habitReadOnlyRepository,
            IDayHabitWriteOnlyRepository dayHabitWriteOnlyRepository,
            IUnityOfWork unityOfWork)
        {
            _loggedUser = loggedUser;
            _habitReadOnlyRepository = habitReadOnlyRepository;
            _dayHabitWriteOnlyRepository = dayHabitWriteOnlyRepository;
            _unityOfWork = unityOfWork;
        }

        public async Task Execute(long habitId, DateOnly date)
        {
            var loggedUser = await _loggedUser.Get();

            var habit = await _habitReadOnlyRepository.GetById(loggedUser, habitId);

            if (habit is null)
            {
                throw new NotFoundException(ResourceErrorMessages.HABIT_NOT_FOUND);
            }

            if (!habit.IsActive)
            {
                throw new ErrorOnValidationException(new List<string> { ResourceErrorMessages.HABIT_NOT_ACTIVE });
            }

            var dayOfWeek = date.DayOfWeek;
            var isHabitScheduledForDay = habit.WeekDays.Any(wd => (int)wd == (int)dayOfWeek);
            if (!isHabitScheduledForDay)
            {
                throw new ErrorOnValidationException(new List<string> { ResourceErrorMessages.HABIT_NOT_TODAY});
            }

            await _dayHabitWriteOnlyRepository.ToggleCompletionStatusAsync(habitId, date);

            await _unityOfWork.Commit();
        }
    }
}

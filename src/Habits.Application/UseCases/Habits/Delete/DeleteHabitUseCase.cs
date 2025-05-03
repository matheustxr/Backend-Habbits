
using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Habits;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Habits.Delete
{
    public class DeleteHabitUseCase : IDeleteHabitUseCase
    {
        private readonly IHabitReadOnlyRepository _habitReadOnly;
        private readonly IHabitWriteOnlyRepository _habitWriteOnly;
        private readonly IUnityOfWork _unityOfWork;
        private readonly ILoggedUser _loggedUser;

        public DeleteHabitUseCase(
            IHabitReadOnlyRepository habitReadOnly,
            IHabitWriteOnlyRepository habitWriteOnly,
            IUnityOfWork unityOfWork,
            ILoggedUser loggedUser)
        {
            _habitReadOnly = habitReadOnly;
            _habitWriteOnly = habitWriteOnly;
            _unityOfWork = unityOfWork;
            _loggedUser = loggedUser;

        }

        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.Get();

            var habit = await _habitReadOnly.GetById(loggedUser, id);

            if (habit == null)
            {
                throw new NotFoundException(ResourceErrorMessages.HABIT_NOT_FOUND);
            }

            await _habitWriteOnly.Delete(loggedUser, id);

            await _unityOfWork.Commit();
        }
    }
}

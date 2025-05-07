using AutoMapper;
using FluentValidation.Results;
using Habits.Communication.Requests.Habits;
using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Habits;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Habits.Update
{
    public class UpdateHabitUseCase : IUpdateHabitUseCase
    {
        private readonly IMapper _mapper;
        private readonly IHabitReadOnlyRepository _habitReadOnlyRepository;
        private readonly IHabitUpdateOnlyRepository _habitUpdateRepository;
        private readonly IUnityOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;

        public UpdateHabitUseCase(
            IMapper mapper,
            IHabitReadOnlyRepository habitReadOnlyRepository,
            IHabitUpdateOnlyRepository habitUpdateRepository,
            IUnityOfWork unitOfWork,
            ILoggedUser loggedUser)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _habitReadOnlyRepository = habitReadOnlyRepository;
            _habitUpdateRepository = habitUpdateRepository;
            _loggedUser = loggedUser;
        }

        public async Task Execute(long id, RequestHabitJson request)
        {
            await Validate(request, id);

            var loggedUser = await _loggedUser.Get();

            var habit = await _habitUpdateRepository.GetById(loggedUser, id);

            if (habit is null)
            {
                throw new NotFoundException(ResourceErrorMessages.HABIT_NOT_FOUND);
            }

            _mapper.Map(request, habit);

            _habitUpdateRepository.Update(habit);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestHabitJson request, long id)
        {
            var result = new HabitValidator().Validate(request);

            var titleExist = await _habitReadOnlyRepository.ExistHabitWithTitle(request.Title, id);

            if (titleExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.TITLE_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

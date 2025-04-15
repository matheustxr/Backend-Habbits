using AutoMapper;
using FluentValidation.Results;
using Habits.Communication.Requests.Habits;
using Habits.Communication.Responses.Habits;
using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Habits;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Habits.Create
{
    public class CreateHabitUseCase : ICreateHabitUseCase
    {
        private readonly IMapper _mapper;
        private readonly IHabitReadOnlyRepository _habitReadOnlyRepository;
        private readonly IHabitWriteOnlyRepository _habitWriteOnlyRepository;
        private readonly IUnityOfWork _unitOfWork;

        public CreateHabitUseCase(
            IMapper mapper,
            IHabitReadOnlyRepository habitReadOnlyRepository,
            IHabitWriteOnlyRepository habitWriteOnlyRepository,
            IUnityOfWork unitOfWork)
        {
            _mapper = mapper;
            _habitReadOnlyRepository = habitReadOnlyRepository;
            _habitWriteOnlyRepository = habitWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseCreateHabitJson> Execute(RequestCreateHabitJson request)
        {
            await Validate(request);

            var habit = _mapper.Map<Domain.Entities.Habit>(request);

            await _habitWriteOnlyRepository.Add(habit);

            await _unitOfWork.Commit();

            return new ResponseCreateHabitJson
            {
                Title = habit.Title
            };
        }

        private async Task Validate(RequestCreateHabitJson request)
        {
            var result = new CreateHabitValidator().Validate(request);

            var titleExist = await _habitReadOnlyRepository.ExistActiveHabitWithTitle(request.Title);

            if (titleExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

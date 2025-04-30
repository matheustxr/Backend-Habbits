using AutoMapper;
using Habits.Communication.Responses.Habits;
using Habits.Domain.Repositories.Habits;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Habits.GetById
{
    public class GetHabitByIdUseCase : IGetHabitByIdUseCase
    {
        private readonly IHabitReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetHabitByIdUseCase(IHabitReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseHabitJson> Execute(long id)
        {
            var loggedUser = await _loggedUser.Get();

            var result = await _repository.GetById(loggedUser, id);

            if (result is null)
            {
                throw new NotFoundException(ResourceErrorMessages.HABIT_NOT_FOUND);
            }

            return _mapper.Map<ResponseHabitJson>(result);
        }
    }
}

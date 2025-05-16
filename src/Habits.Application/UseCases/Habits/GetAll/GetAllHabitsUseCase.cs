using AutoMapper;
using Habits.Communication.Responses.Habits;
using Habits.Domain.Repositories.Habits;
using Habits.Domain.Services.LoggedUser;

namespace Habits.Application.UseCases.Habits.GetAll
{
    public class GetAllHabitsUseCase : IGetAllHabitsUseCase
    {
            private readonly IHabitReadOnlyRepository _repository;
            private readonly IMapper _mapper;
            private readonly ILoggedUser _loggedUser;

        public GetAllHabitsUseCase(IHabitReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseHabitsJson> Execute()
        {
            var loggedUser = await _loggedUser.Get();

            var result = await _repository.GetAll(loggedUser);

            return new ResponseHabitsJson
            {
                Habits = _mapper.Map<List<ResponseShortHabitJson>>(result)
            };
        }
    }
}
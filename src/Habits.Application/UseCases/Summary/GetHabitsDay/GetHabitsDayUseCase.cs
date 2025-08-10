using AutoMapper;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Repositories.DayHabits;
using Habits.Domain.Services.LoggedUser;

namespace Habits.Application.UseCases.Summary.GetHabitsDay
{
    public class GetHabitsDayUseCase : IGetHabitsDayUseCase
    {
        private readonly IDayHabitReadOnlyRepository _dayHabitRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetHabitsDayUseCase(
            IDayHabitReadOnlyRepository dayHabitRepository,
            IMapper mapper,
            ILoggedUser loggedUse)
        {
            _dayHabitRepository = dayHabitRepository;
            _mapper = mapper;
            _loggedUser = loggedUse;
        }

        public async Task<List<ResponseSummaryHabitJson>> Execute(DateOnly date)
        {
            var loggedUser = await _loggedUser.Get();

            var startDateUtc = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var endDateUtc = date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

            var habistDay = await _dayHabitRepository.GetHabitsForDateAsync(loggedUser.Id, date);

            return _mapper.Map<List<ResponseSummaryHabitJson>>(habistDay);
        }
    }
}

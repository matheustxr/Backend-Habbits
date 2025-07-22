using AutoMapper;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Repositories.DayHabits;
using Habits.Domain.Services.LoggedUser;

namespace Habits.Application.UseCases.Summary
{
    public class GetMonthlySummaryUseCase : IGetMonthlySummaryUseCase
    {
        private readonly IDayHabitReadOnlyRepository _dayHabitRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetMonthlySummaryUseCase(
            IDayHabitReadOnlyRepository dayHabitRepository, 
            IMapper mapper, 
            ILoggedUser loggedUse)
        {
            _dayHabitRepository = dayHabitRepository;
            _mapper = mapper;
            _loggedUser = loggedUse;
        }

        public async Task<List<ResponseSummaryJson>> Execute(Guid userId, DateOnly startDate, DateOnly endDate)
        {
            var loggedUser = await _loggedUser.Get();

            var summaryData = await _dayHabitRepository.GetMonthlySummaryAsync(loggedUser.Id, startDate, endDate);

            var result = summaryData
                .Select(kvp => new ResponseSummaryJson
                {
                    Date = kvp.Key,
                    Completed = kvp.Value.completed,
                    Amount = kvp.Value.possible
                }).ToList();

            return _mapper.Map<List<ResponseSummaryJson>>(result);
        }
    }
}

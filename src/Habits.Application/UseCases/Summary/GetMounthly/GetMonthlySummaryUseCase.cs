using AutoMapper;
using Habits.Communication.Requests.Summary;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Repositories.DayHabits;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Summary.GetMounthly
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

        public async Task<List<ResponseSummaryJson>> Execute(DateOnly startDate, DateOnly endDate)
        {
            await Validate(startDate, endDate);

            var loggedUser = await _loggedUser.Get();

            var summaryData = await _dayHabitRepository.GetMonthlySummaryAsync(loggedUser.Id, startDate, endDate);

            return _mapper.Map<List<ResponseSummaryJson>>(summaryData);
        }

        private async Task Validate(DateOnly startDate, DateOnly endDate)
        {
            var validator = new SummaryDateRangeValidator();

            var result = await validator.ValidateAsync(new RequestSummaryDateRange
            {
                StartDate = startDate,
                EndDate = endDate
            });

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}

using AutoMapper;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Repositories.DayHabits;
using Habits.Domain.Services.LoggedUser;

namespace Habits.Application.UseCases.Summary.GetHabitsDay
{
    public class GetHabitsDayUseCase : IGetHabitsDayUseCase
    {
        private readonly IDayHabitReadOnlyRepository _dayHabitRepository;
        //private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetHabitsDayUseCase(
            IDayHabitReadOnlyRepository dayHabitRepository,
            //IMapper mapper,
            ILoggedUser loggedUser)
        {
            _dayHabitRepository = dayHabitRepository;
            //_mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<List<ResponseSummaryHabitJson>> Execute(DateOnly date)
        {
            var loggedUser = await _loggedUser.Get();

            var habistDay = await _dayHabitRepository.GetHabitsForDateAsync(loggedUser.Id, date);

            return habistDay.Select(habit => new ResponseSummaryHabitJson
            {
                Id = habit.habitId,
                Title = habit.title,
                Completed = habit.isCompleted,
                CategoryName = habit.categoryName,
                CreatedAt = habit.createdAt ,
                UpdatedAt = habit.updatedAt  // <-- Agora disponível
            }).ToList();
        }
    }
}

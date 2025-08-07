using AutoMapper;
using Habits.Communication.Requests.Categories;
using Habits.Communication.Requests.Habits;
using Habits.Communication.Requests.Users;
using Habits.Communication.Responses.Categories;
using Habits.Communication.Responses.Habits;
using Habits.Communication.Responses.Summary;
using Habits.Communication.Responses.Users;
using Habits.Domain.Entities;

namespace Habits.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestEntity();
        EntityResponse();
    }

    private void RequestEntity()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());

        CreateMap<RequestHabitJson, Habit>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => (DateTime?)null))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<RequestCategoryJson, HabitCategory>();
    }

    private void EntityResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();

        CreateMap<Habit, ResponseCreateHabitJson>();
        CreateMap<Habit, ResponseHabitsJson>();
        CreateMap<Habit, ResponseShortHabitJson>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.HabitCategory != null ? src.HabitCategory.Category : null));
        CreateMap<Habit, ResponseHabitJson>();

        CreateMap<HabitCategory, ResponseCategoryJson>();

        CreateMap<(long habitId, string title, string? categoryName, bool isCompleted), ResponseSummaryHabitJson>()
        .ConvertUsing(src => new ResponseSummaryHabitJson
        {
            Id = src.habitId,
            Title = src.title,
            CategoryName = src.categoryName,
            Completed = src.isCompleted
        });
        CreateMap<DayHabit, ResponseSummaryHabitJson>();
    }
}

using AutoMapper;
using FluentValidation.Results;
using Habits.Communication.Requests.Habits;
using Habits.Domain.Entities;
using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Categories;
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
        private readonly ICategoriesReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnityOfWork _unitOfWork;

        public UpdateHabitUseCase(
            IMapper mapper,
            IHabitReadOnlyRepository habitReadOnlyRepository,
            IHabitUpdateOnlyRepository habitUpdateRepository,
            ICategoriesReadOnlyRepository categoryReadOnlyRepository,
            ILoggedUser loggedUser,
            IUnityOfWork unitOfWork)
        {
            _mapper = mapper;
            _habitReadOnlyRepository = habitReadOnlyRepository;
            _habitUpdateRepository = habitUpdateRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestHabitJson request, long id)
        {
            var user = await _loggedUser.Get();

            await Validate(request, user, id);

            var habit = await _habitUpdateRepository.GetById(user, id);

            if (habit is null)
            {
                throw new NotFoundException(ResourceErrorMessages.HABIT_NOT_FOUND);
            }

            _mapper.Map(request, habit);

            habit.UpdatedAt = DateTime.UtcNow;

            _habitUpdateRepository.Update(habit);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestHabitJson request, User user,long id)
        {
            var result = new HabitValidator().Validate(request);

            var titleExist = await _habitReadOnlyRepository.ExistHabitWithTitle(request.Title, user, id);

            if (titleExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.TITLE_ALREADY_REGISTERED));
            }
            else if (request.CategoryId != null)
            {
                var categoryExist = await _categoryReadOnlyRepository.GetById(user, request.CategoryId.Value);

                if (categoryExist == null)
                {
                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.CATEGORY_NOT_FOUND));
                }
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

using AutoMapper;
using FluentValidation.Results;
using Habits.Communication.Requests.Habits;
using Habits.Communication.Responses.Habits;
using Habits.Domain.Entities;
using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Categories;
using Habits.Domain.Repositories.Habits;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Habits.Create
{
    public class CreateHabitUseCase : ICreateHabitUseCase
    {
        private readonly IMapper _mapper;
        private readonly IHabitReadOnlyRepository _habitReadOnlyRepository;
        private readonly IHabitWriteOnlyRepository _habitWriteOnlyRepository;
        private readonly ICategoriesReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnityOfWork _unitOfWork;

        public CreateHabitUseCase(
            IMapper mapper,
            IHabitReadOnlyRepository habitReadOnlyRepository,
            IHabitWriteOnlyRepository habitWriteOnlyRepository,
            ICategoriesReadOnlyRepository categoryReadOnlyRepository,
            ILoggedUser loggedUser,
            IUnityOfWork unitOfWork)
        {
            _mapper = mapper;
            _habitReadOnlyRepository = habitReadOnlyRepository;
            _habitWriteOnlyRepository = habitWriteOnlyRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseCreateHabitJson> Execute(RequestHabitJson request)
        {
            var user = await _loggedUser.Get();

            await Validate(request, user);

            var habit = _mapper.Map<Habit>(request);
            habit.UserId = user.Id;

            await _habitWriteOnlyRepository.Add(habit);

            await _unitOfWork.Commit();

            return new ResponseCreateHabitJson
            {
                Title = habit.Title
            };
        }

        private async Task Validate(RequestHabitJson request, User user)
        {
            var result = new HabitValidator().Validate(request);

            var titleExist = await _habitReadOnlyRepository.ExistHabitWithTitle(request.Title, user);

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

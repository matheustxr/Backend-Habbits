using AutoMapper;
using FluentValidation.Results;
using Habits.Communication.Requests.Categories;
using Habits.Communication.Responses.Categories;
using Habits.Domain.Entities;
using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Categories;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Categories.Create
{
    public class CreateCategoryUseCase : ICreateCategoryUseCase
    {
        private readonly IMapper _mapper;
        private readonly ICategoriesReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ICategoriesWriteOnlyRepository _categoryWriteOnlyRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnityOfWork _unitOfWork;

        public CreateCategoryUseCase(
            IMapper mapper,
            ICategoriesReadOnlyRepository categoryReadOnlyRepository,
            ICategoriesWriteOnlyRepository categoryWriteOnlyRepository,
            ILoggedUser loggedUser,
            IUnityOfWork unitOfWork)
        {
            _mapper = mapper;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseCategoryJson> Execute(RequestCategoryJson request)
        {
            var user = await _loggedUser.Get();

            await Validate(request, user);

            var category = _mapper.Map<HabitCategory>(request);

            category.UserId = user.Id;

            await _categoryWriteOnlyRepository.Add(category);

            await _unitOfWork.Commit();

            return new ResponseCategoryJson
            {
                Category = category.Category,
                HexColor = category.HexColor
            };
        }

        private async Task Validate(RequestCategoryJson request, User user)
        {
            var result = new CategoryValidator().Validate(request);

            var titleExist = await _categoryReadOnlyRepository.AlreadyCategoryExist(request.Category, user.Id);

            if (titleExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.TITLE_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

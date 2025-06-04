using AutoMapper;
using FluentValidation.Results;
using Habits.Communication.Requests.Categories;
using Habits.Domain.Entities;
using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Categories;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Categories.Update
{
    public class UpdateCategoryUseCase : IUpdateCategoryUseCase
    {
        private readonly IMapper _mapper;
        private readonly ICategoriesReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ICategoriesUpdateOnlyRepository _categoryUpdateRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnityOfWork _unitOfWork;

        public UpdateCategoryUseCase(
            IMapper mapper,
            ICategoriesReadOnlyRepository categoryReadOnlyRepository,
            ICategoriesUpdateOnlyRepository categoryUpdateRepository,
            ILoggedUser loggedUser,
            IUnityOfWork unitOfWork)
        {
            _mapper = mapper;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _categoryUpdateRepository = categoryUpdateRepository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestCategoryJson request, long id)
        {
            var user = await _loggedUser.Get();

            await Validate(request, user, id);

            var category = await _categoryReadOnlyRepository.GetById(user, id);

            if (category is null)
            {
                throw new NotFoundException(ResourceErrorMessages.CATEGORY_NOT_FOUND);
            }

            _mapper.Map(request, category);

            _categoryUpdateRepository.Update(category);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestCategoryJson request, User user, long id)
        {
            var result = new CategoryValidator().Validate(request);

            var titleExist = await _categoryReadOnlyRepository.AlreadyCategoryExist(request.Category, user.Id, id);

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

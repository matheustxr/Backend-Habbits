using AutoMapper;
using Habits.Communication.Responses.Categories;
using Habits.Domain.Repositories.Categories;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Categories.GetById
{
    public class GetCategoriesByIdUseCase : IGetCategoriesByIdUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ICategoriesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetCategoriesByIdUseCase(
            ILoggedUser loggedUser,
            ICategoriesReadOnlyRepository repository,
            IMapper mapper)
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseCategoryJson> Execute(long id)
        {
            var user = await _loggedUser.Get();

            var result = await _repository.GetById(user, id);

            if (result == null)
            {
                throw new NotFoundException(ResourceErrorMessages.CATEGORY_NOT_FOUND);
            }

            return _mapper.Map<ResponseCategoryJson>(result);
        }
    }
}

using AutoMapper;
using Habits.Communication.Responses.Categories;
using Habits.Domain.Repositories.Categories;
using Habits.Domain.Services.LoggedUser;

namespace Habits.Application.UseCases.Categories.GetAll
{
    public class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ICategoriesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCategoriesUseCase(
            ILoggedUser loggedUser,
            ICategoriesReadOnlyRepository repository,
            IMapper mapper)
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseListCategoriesJson> Execute()
        {
            var user = await _loggedUser.Get();

            var result = await _repository.GetAll(user);

            return new ResponseListCategoriesJson
            {
                Categories = _mapper.Map<List<ResponseCategoryJson>>(result)
            };
        }
    }
}

using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Categories;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace Habits.Application.UseCases.Categories.Delete
{
    public class DeleteCategoryUseCase : IDeleteCategoryUseCase
    {
        private readonly ICategoriesReadOnlyRepository _categoryReadOnly;
        private readonly ICategoriesWriteOnlyRepository _categoryWriteOnly;
        private readonly IUnityOfWork _unityOfWork;
        private readonly ILoggedUser _loggedUser;

        public DeleteCategoryUseCase(
            ICategoriesReadOnlyRepository categoryReadOnly,
            ICategoriesWriteOnlyRepository categoryWriteOnly,
            IUnityOfWork unityOfWork,
            ILoggedUser loggedUser)
        {
            _categoryReadOnly = categoryReadOnly;
            _categoryWriteOnly = categoryWriteOnly;
            _unityOfWork = unityOfWork;
            _loggedUser = loggedUser;
        }

        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.Get();

            var catgeory = await _categoryReadOnly.GetById(loggedUser, id);

            if (catgeory == null)
            {
                throw new NotFoundException(ResourceErrorMessages.CATEGORY_NOT_FOUND);
            }

            await _categoryWriteOnly.Delete(loggedUser, id);

            await _unityOfWork.Commit();
        }
    }
}

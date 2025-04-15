namespace Habits.Domain.Repositories;
public interface IUnityOfWork
{
    Task Commit();
}

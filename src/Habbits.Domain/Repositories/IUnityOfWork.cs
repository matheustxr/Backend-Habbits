namespace Habbits.Domain.Repositories;
public interface IUnityOfWork
{
    Task Commit();
}

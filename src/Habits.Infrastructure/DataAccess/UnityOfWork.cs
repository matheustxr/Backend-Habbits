using Habits.Domain.Repositories;

namespace Habits.Infrastructure.DataAccess;

public class UnityOfWork : IUnityOfWork
{
    private readonly HabbitsDbContext _dbContext;
    public UnityOfWork(HabbitsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}

using Habbits.Domain.Repositories;

namespace Habbits.Infrastructure.DataAccess;

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

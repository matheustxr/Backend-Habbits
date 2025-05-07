using Habits.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Erro ao salvar no banco: " + ex.InnerException?.Message);
            throw;
        }
    }
}

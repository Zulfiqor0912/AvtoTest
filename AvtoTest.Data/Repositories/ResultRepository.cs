using AvtoTest.Data.Context;
using AvtoTest.Data.Entities;
using AvtoTest.Data.Repositories.Interfaces;

namespace AvtoTest.Data.Repositories;

public class ResultRepository : IResultRepository
{
    public AppDbContext appDbContext;

    public ResultRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }
    public Task AddResult()
    {
        throw new NotImplementedException();
    }

    public Task DeleteResult()
    {
        throw new NotImplementedException();
    }

    public List<Result> GetAllResults()
    {
    }
}

using AvtoTest.Data.Entities;

namespace AvtoTest.Data.Repositories.Interfaces;

public interface IResultRepository
{
    public List<Result> GetAllResults();
    public Task AddResult();
    public Task DeleteResult();
}

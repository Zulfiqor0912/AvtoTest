using AvtoTest.Data.Entities;

namespace AvtoTest.Data.Repositories.Interfaces;

public interface IResultRepository
{
    public Task<List<Result>> GetAllResults();
    public Task AddResult(byte ticketId, string userId, int correctAnswerCount);
    public Task DeleteResult(Result result);
    public Task<Result?> GetResultById(byte ticketId, string userId);

}

using AvtoTest.Data.Context;
using AvtoTest.Data.Entities;
using AvtoTest.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AvtoTest.Data.Repositories;

public class ResultRepository : IResultRepository
{
    public AppDbContext appDbContext;
    private readonly IMemoryCache _memoryCache;
    private const string ResultKey = "results";

    public ResultRepository(AppDbContext appDbContext, IMemoryCache memoryCache)
    {
        this.appDbContext = appDbContext;
        _memoryCache = memoryCache;
    }
    public async Task AddResult(byte ticketId, string userId, int correctAnswerCount)
    {
        var result = new Result()
        {
            TicketId = ticketId,
            CorrectAnswerCount = (byte)correctAnswerCount,
            UserId = userId
        };

        appDbContext.Results.Add(result);
        await appDbContext.SaveChangesAsync();
        await GetOrUpdateResults();
    }

    public async Task DeleteResult(Result result)
    {
        appDbContext.Results
            .Where(r => r.Id == result.Id)
            .ExecuteDeleteAsync();
        await appDbContext.SaveChangesAsync();
        await GetOrUpdateResults();
    }

    public async Task<List<Result>> GetAllResults()
    {
        var results = await GetResults();
        return results;
    }

    private async Task<List<Result>> GetOrUpdateResults()
    {
        var results = await appDbContext.Results.ToListAsync();
        _memoryCache.Set(ResultKey, results);
        return results;
    }

    private async Task<List<Result>> GetResults()
    {
        if (_memoryCache.TryGetValue(ResultKey, out object value))
        {
            var results = (List<Result>) value;
            return results;
        }

        return await GetOrUpdateResults();
    }

    public async Task<Result?> GetResultById(byte ticketId, string userId)
    {
        var results = await GetResults();
        var result = results.FirstOrDefault(r => r.TicketId == ticketId && r.UserId == userId);
        return result;
    }
}

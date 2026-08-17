using AvtoTest.Data.Context;
using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AvtoTest.Data.Repositories;

public class HomeRepository : IHomeRepasitory
{
    public AppDbContext dbContext;

    public HomeRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task AddUser(AnonymousUser anonymousUser)
    {
        throw new NotImplementedException();
    }

    public Task<List<AnonymousUser>> GetAllUser()
    {
        throw new NotImplementedException();
    }

    public Task<AnonymousUser> GetUserById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUser(AnonymousUser anonymousUser)
    {
        throw new NotImplementedException();
    }

    //public async Task<AnonymousUser> GetUserById(Guid id)
    //{
    //    var user =  await dbContext.AnonymousUsers.FirstOrDefaultAsync(u => u.Id == id);
    //    return user;
    //}

    //public async Task UpdateUser(AnonymousUser anonymousUser)
    //{
    //    dbContext.AnonymousUsers.Update(anonymousUser);
    //    await dbContext.SaveChangesAsync();
    //}
}

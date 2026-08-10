using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Repositories.Interfaces;

namespace AvtoTest.Data.Repositories;

public class VisitorUserRepository : IVisitorUserRepository
{
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
}

using AvtoTest.Data.Entities.TestEntities;

namespace AvtoTest.Data.Repositories.Interfaces;

public interface IHomeRepasitory
{
    public Task AddUser(AnonymousUser anonymousUser);
    public Task UpdateUser(AnonymousUser anonymousUser);
    public Task<AnonymousUser> GetUserById(Guid id);
    public Task<List<AnonymousUser>> GetAllUser();
}

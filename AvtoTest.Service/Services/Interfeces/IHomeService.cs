using AvtoTest.Data.Entities.TestEntities;

namespace AvtoTest.Service.Services.Interfeces;

public interface IHomeService
{
    public Task<AnonymousUser> GetUserById(Guid id);
    //public Task<AnonymousUser> UpdateUser(AnonymousUser user);
}

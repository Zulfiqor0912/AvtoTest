using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Repositories.Interfaces;
using AvtoTest.Service.Services.Interfeces;

namespace AvtoTest.Service.Services;

public class HomeService : IHomeService
{

    public IHomeRepasitory visitorUserRepository;
    public HomeService(IHomeRepasitory visitorUserRepository)
    {
        this.visitorUserRepository = visitorUserRepository;
    }
    
    public async Task<AnonymousUser> GetUserById(Guid id)
    {
        var user = await visitorUserRepository.GetUserById(id);
        return user;
    }

    //public async Task UpdateUser(AnonymousUser user)
    //{
    //    await visitorUserRepository.UpdateUser(user);
    //}
}

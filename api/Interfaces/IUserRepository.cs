using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllAsync(UserQueryObject query);
    }
}
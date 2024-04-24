using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class UserRepository: IUserRepository
    {
         private readonly ApplicationDBContext _context;
        // application db context is a dependency provided in this contructor only which is a dependency injection design pattern
        // the advantage of this approach is that the instantiation of the applicationdbcontext is now abstracted away and decoupled from this class
        public UserRepository(ApplicationDBContext context){
            _context = context;
        }
        /// <summary>
        /// get all users
        /// </summary>
        /// <param name="query">query params passed in by usl</param>
        /// <returns></returns>
        public async Task<List<AppUser>> GetAllAsync(UserQueryObject query)
        {

            var users = _context.Users.AsQueryable();
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            
            return await users.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }
    }
}
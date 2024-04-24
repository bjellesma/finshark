using api.Dtos.Account;
using api.Models;

namespace api.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this AppUser userModel){
            return new UserDto{
                UserName = userModel.UserName,
                Email = userModel.Email,
            };
        }
    }
}
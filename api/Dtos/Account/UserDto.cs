namespace api.Dtos.Account
{
    /// <summary>
    /// DTO meant to return data after we create a new user
    /// </summary>
    public class UserDto
    {
        public string UserName {get;set;}
        public string Email {get;set;}
        public string Token {get;set;}
    }
}
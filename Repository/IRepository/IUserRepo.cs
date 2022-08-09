using AuthService.Models.DTOs;

namespace AuthService.Repository.IRepository
{
    public interface IUserRepo
    {
        public bool userExist(string username);
        public int addUser(UserDto UserData);
        public UserDto authenticate(string username, string password);                
    }
}

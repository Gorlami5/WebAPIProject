using WebAPIProject.Models;

namespace WebAPIProject.Data
{
    public interface IAuthRepository
    {
        User Register(User user, string password);

        User Login (string username, string password);

        bool UserExist(string user);
    }
}

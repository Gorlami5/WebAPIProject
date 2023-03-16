using WebAPIProject.Helpers;
using WebAPIProject.Models;

namespace WebAPIProject.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        
        public User Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName.Equals(username));
            if (user == null)
            {
                return null;
            }
            if (!HashHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public User Register(User user, string password)
        {         
            byte[] passwordhash, passwordsalt;
            
            HashHelper.CreatePasswordHash(password, out passwordhash, out passwordsalt);
            user.PasswordSalt = passwordsalt;
            user.PasswordHash = passwordhash;
            _context.Add(user);
            _context.SaveChanges();
            return user;
        }

       

        public bool UserExist(string userName)
        {
            if (_context.Users.Any(x => x.UserName.Equals(userName)))
                {
                return true;
                }

            return false;
        }
    }
}

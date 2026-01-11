using Microsoft.EntityFrameworkCore;
using WebPortal.DbStuff.Repositories.Interfaces;
using WebPortal.DbStuff.DataModels;
using WebPortal.DbStuff.Models;

namespace WebPortal.DbStuff.Repositories
{
    public class UserRepositrory : BaseRepository<User>, IUserRepositrory
    {
        public UserRepositrory(TourismPortalContext portalContext) : base(portalContext)
        {
        }

        public override User Add(User model)
        {
            throw new Exception("DO NOT USER Add. User Registration method");
        }

        public override List<User> AddRange(List<User> models)
        {
            throw new Exception("DO NOT USER Add. User Registration method");
        }

        public User? GetByName(string name)
        {
            return _dbSet.Where(x => x.UserName == name).FirstOrDefault();
        }

        public User? Login(string userName, string password)
        {
            var hashPasswod = HashPassword(password);
            return _dbSet.FirstOrDefault(x => x.UserName == userName && x.Password == hashPasswod);
        }

        public void Registration(string userName, string password)
        {
            if (_dbSet.Any(x => x.UserName == userName))
            {
                throw new Exception($"{userName} already exist");
            }

            var user = new User
            {
                UserName = userName,
                Password = HashPassword(password), // broke Password
                Language = Enum.Language.English,
                Role = Enum.Role.User
            };

            _dbSet.Add(user);
            _portalContext.SaveChanges();
        }

        private string HashPassword(string password)
        {
            return password.Replace("a", "") + password.Length;
        }

        public User? GetFirstByName(string userName)
        {
            return _dbSet.FirstOrDefault(x => x.UserName == userName);
        }
    }
}

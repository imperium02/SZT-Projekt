using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SZT_Projekt.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public void AddUsers(IEnumerable<User> users)
        {
            _appDbContext.Users.AddRange(users);
            _appDbContext.SaveChanges();
        }

        public User GetUserByName(string name)
        {
            return _appDbContext.Users.SingleOrDefault(x => x.Name == name);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _appDbContext.Users.AsNoTracking().ToList();
        }

        public void UpdateUsers(IEnumerable<User> usersToModify)
        {
            foreach (var user in usersToModify)
            {
                var userToUpdate = _appDbContext.Users.SingleOrDefault(x => x.Name == user.Name);
                if (userToUpdate != null)
                {
                    if (user.Paid != null)
                    {
                        userToUpdate.Paid += user.Paid;
                    }

                    if (user.Loaned != null)
                    {
                        userToUpdate.Loaned += user.Loaned;
                    }

                    _appDbContext.SaveChanges();
                }
            }
        }
    }
}

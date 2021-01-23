using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZT_Projekt.Models
{
    public interface IUserRepository
    {
        public void AddUsers(IEnumerable<User> users);
        public User GetUserByName(string name);
        public IEnumerable<User> GetAllUsers();
        public void UpdateUsers(IEnumerable<User> usersToModify);
    }
}

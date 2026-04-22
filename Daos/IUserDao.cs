using YTeAspMVC.Models;
using System.Collections.Generic; // Cần dòng này để dùng List<User>

namespace YTeAspMVC.Daos
{
    public interface IUserDao
    {
        bool checkLogin(string email, string password);
        User getUserByEmail(string email);
        List<User> getUser();
        void Add(User user);
        User getById(int id);
        void Update(User user);
        void Delete(int id);
        bool checkExistEmail(string email);
    }
}
using YTeAspMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using YTeAspMVC.Daos;
using YTeAspMVC.Utils;



namespace YTeAspMVC.Daos
{
    public class UserDao : IUserDao
    {
        YTeDBContext myDb = new YTeDBContext();
        public bool checkLogin(string email, string password)
        {
            string hashed = SecurityUtils.HashSHA256(password);
            var obj = myDb.Users.FirstOrDefault(x => x.Email == email && x.Password == hashed);
            if (obj == null) 
            { 
                // Fallback for unmigrated accounts
                obj = myDb.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
                if (obj == null) return false;
                
                // Migrate on the fly
                obj.Password = hashed;
                myDb.SaveChanges();
            }
            if (obj.TrangThai == false) { return false; } // TrangThai = 0 => block login
            return true;
        }

        public User getUserByEmail(string email)
        {
            return myDb.Users.FirstOrDefault(x => x.Email.Equals(email));
        }


        public List<User> getUser()
        {
            return myDb.Users.Where(x => x.IdRole == 1).ToList();
        }

        public void Add(User user)
        {
            myDb.Users.Add(user);
            myDb.SaveChanges();
        }
        public User getById(int id)
        {
            return myDb.Users.FirstOrDefault(x => x.IdUser == id);
        }
        public void Update(User user)
        {
            var obj = myDb.Users.FirstOrDefault(x => x.IdUser == user.IdUser);
            obj.Email = user.Email;
            obj.FullName = user.FullName;
            if (!string.IsNullOrEmpty(user.Password))
            {
                obj.Password = SecurityUtils.HashSHA256(user.Password);
            }
            obj.PhoneNumber = user.PhoneNumber;
            obj.Address = user.Address;
            obj.Gender = user.Gender;
            obj.TrangThai = user.TrangThai;
            myDb.SaveChanges();
        }
        public bool Delete(int id)
        {
            try
            {
                var obj = myDb.Users.FirstOrDefault(x => x.IdUser == id);
                if (obj != null)
                {
                    myDb.Users.Remove(obj);
                    myDb.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool checkExistEmail(string email)
        {
            var user = myDb.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public int MigrateOldPasswords()
        {
            var users = myDb.Users.ToList();
            int count = 0;
            foreach (var u in users)
            {
                if (!string.IsNullOrEmpty(u.Password) && u.Password.Length != 64)
                {
                    u.Password = SecurityUtils.HashSHA256(u.Password);
                    count++;
                }
            }
            if (count > 0)
            {
                myDb.SaveChanges();
            }
            return count;
        }
    }
}
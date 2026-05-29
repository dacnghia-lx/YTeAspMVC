using YTeAspMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YTeAspMVC.Utils;

namespace YTeAspMVC.Daos
{
    public class DoctorDao
    {
        YTeDBContext myDb = new YTeDBContext();

        public List<Doctor> GetAll()
        {
            return myDb.Doctors.ToList();
        }

        public List<Doctor> Search(string keyword)
        {
            return myDb.Doctors.Where(x => x.FullName.Contains(keyword)).ToList();
        }

        public void Add(Doctor doctor)
        {
            myDb.Doctors.Add(doctor);
            myDb.SaveChanges();
        }

        public void Update(Doctor doctor)
        {
            var obj = myDb.Doctors.FirstOrDefault(x => x.IdDoctor == doctor.IdDoctor);
            obj.Email = doctor.Email;
            obj.FullName = doctor.FullName;
            if (!string.IsNullOrEmpty(doctor.Password))
            {
                obj.Password = SecurityUtils.HashSHA256(doctor.Password);
            }
            obj.Specialist = doctor.Specialist;
            obj.Describe = doctor.Describe;
            obj.Image = doctor.Image;
            myDb.SaveChanges();
        }
        public void Delete(int id)
        {
            var obj = myDb.Doctors.FirstOrDefault(x => x.IdDoctor == id);
            myDb.Doctors.Remove(obj);
            myDb.SaveChanges();
        }

        public bool checkLogin(string email, string password)
        {
            string hashed = SecurityUtils.HashSHA256(password);
            var obj = myDb.Doctors.FirstOrDefault(x => x.Email == email && x.Password == hashed);
            if (obj == null) 
            { 
                // Fallback for unmigrated accounts
                obj = myDb.Doctors.FirstOrDefault(x => x.Email == email && x.Password == password);
                if (obj == null) return false;
                
                // Migrate on the fly
                obj.Password = hashed;
                myDb.SaveChanges();
            }
            return true;
        }

        public Doctor getUserByEmail(string email)
        {
            return myDb.Doctors.FirstOrDefault(x => x.Email.Equals(email));
        }

        public Doctor getDoctor(int id)
        {
            return myDb.Doctors.FirstOrDefault(x => x.IdDoctor == id);
        }

        public List<Doctor> GetTop3()
        {
            return myDb.Doctors.OrderByDescending(x => x.IdDoctor).Take(3).ToList();
        }

        public int MigrateOldPasswords()
        {
            var doctors = myDb.Doctors.ToList();
            int count = 0;
            foreach (var d in doctors)
            {
                if (!string.IsNullOrEmpty(d.Password) && d.Password.Length != 64)
                {
                    d.Password = SecurityUtils.HashSHA256(d.Password);
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
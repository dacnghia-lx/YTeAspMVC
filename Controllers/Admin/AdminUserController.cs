using YTeAspMVC.Daos;
using YTeAspMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YTeAspMVC.Controllers.Admin
{
    public class AdminUserController : Controller
    {
        UserDao userDao = new UserDao();
        
        // GET: AdminUser
        public ActionResult Index(string msg)
        {
            ViewBag.Msg = msg;
            ViewBag.List = userDao.getUser();
            return View();
        }

        [HttpPost]
        public ActionResult Add(User user)
        {
            if (userDao.checkExistEmail(user.Email))
            {
                return RedirectToAction("Index", new { msg = "2" });
            }
            user.IdRole = 1;
            userDao.Add(user);
            return RedirectToAction("Index", new { msg = "1" });
        }

        [HttpPost]
        public ActionResult Update(User user)
        {
            userDao.Update(user);
            return RedirectToAction("Index", new { msg = "1" });
        }

        [HttpPost]
        public ActionResult Delete(User user)
        {
            userDao.Delete(user.IdUser);
            return RedirectToAction("Index", new { msg = "1" });
        }
    }
}
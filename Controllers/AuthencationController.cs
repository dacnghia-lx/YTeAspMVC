using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTeAspMVC.Daos;
using YTeAspMVC.Models;

namespace YTeAspMVC.Controllers
{
    public class AuthencationController : Controller
    {
        private readonly IUserDao userDao;

        public AuthencationController()
        {
            userDao = new UserDao();
        }

        public AuthencationController(IUserDao _userDao)
        {
            this.userDao = _userDao;
        }
        //UserDao userDao = new UserDao();
        BookingDao bookingDao = new BookingDao();
        // GET: Authencation
        public ActionResult Login()
        {
            Session.Add("Active", "Login");
            return View();
        }

        public ActionResult Singup()
        {
            Session.Add("Active", "Singup");
            return View();
        }

        public ActionResult Information(string msg)
        {
            User user = (User)Session["USER"];
            ViewBag.mess = msg;
            ViewBag.User = userDao.getById(user.IdUser);
            return View();
        }

        public ActionResult HistoryBooking(string mess)
        {
            User user = (User)Session["USER"];
            ViewBag.Msg = mess;
            ViewBag.List = bookingDao.GetBookingByUser(user.IdUser);
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            bool checkLogin = userDao.checkLogin(user.Email, user.Password);
            if (checkLogin)
            {
                var userInformation = userDao.getUserByEmail(user.Email);
                Session.Add("USER", userInformation);
                return Redirect("/Home/Index");
            }
            else
            {
                ViewBag.mess = "Error";
                return View("Login");
            }

        }


        [HttpPost]
        public ActionResult Singup(User user)
        {
            // 1. Kiểm tra mật khẩu trống
            if (string.IsNullOrEmpty(user.Password))
            {
                ViewBag.mess = "PasswordEmpty";
                return View("Singup");
            }

            // 2. Kiểm tra độ dài mật khẩu
            if (user.Password.Length < 6 || user.Password.Length > 100)
            {
                ViewBag.mess = "PasswordLengthError";
                return View("Singup");
            }

            // 3. Validate Unicode
            bool hasUnicodeinPassword = user.Password.Any(c => c > 127);
            bool hasUnicodeinEmail = user.Email.Any(c => c > 127);
            if (hasUnicodeinPassword || hasUnicodeinEmail)
            {
                ViewBag.mess = "UnicodeError";
                return View("Singup");
            }

            // 4. Các logic cũ của bạn giữ nguyên
            bool checkExistUserName = userDao.checkExistEmail(user.Email);
            if (checkExistUserName)
            {
                ViewBag.mess = "ErrorExist";
                return View("Singup");
            }
            else
            {
                user.IdRole = 1;
                user.Status = 1;
                user.Created = DateTime.Now.ToString();
                userDao.Add(user);
                ViewBag.mess = "Success";
                return View("Singup");
            }
        }

        [HttpPost]
        public ActionResult Update(User user)
        {
            userDao.Update(user);
            return RedirectToAction("Information", new { msg = "Success" });
        }
        public ActionResult Logout()
        {
            Session.Remove("User");
            return Redirect("/Home/Index");
        }
    }
}
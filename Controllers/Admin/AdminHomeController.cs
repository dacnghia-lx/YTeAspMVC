using YTeAspMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YTeAspMVC.Controllers.Admin
{
    public class AdminHomeController : Controller
    {
        YTeDBContext myDb = new YTeDBContext();

        // GET: AdminHome
        public ActionResult Index()
        {
            User user = (User)Session["ADMIN"];
            Doctor doctor = (Doctor)Session["DOCTOR"];
            if (user == null && doctor == null)
            {
                return RedirectToAction("Login", "AdminAuthentication");
            }
            else
            {
                ViewBag.TotalUsers = myDb.Users.Count(x => x.IdRole == 1);
                ViewBag.PendingBookings = myDb.Bookings.Count(x => x.Status == 0);
                ViewBag.ApprovedBookings = myDb.Bookings.Count(x => x.Status == 1);
                ViewBag.TotalDoctors = myDb.Doctors.Count();
                ViewBag.TotalPosts = myDb.Posts.Count();

                return View();
            }
        }


    }
}
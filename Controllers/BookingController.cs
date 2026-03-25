using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTeAspMVC.Daos;
using YTeAspMVC.Models;
using YTeAspMVC.Request;

namespace YTeAspMVC.Controllers
{
    public class BookingController : Controller
    {

        BookingDao bookingDao = new BookingDao();
        // GET: Booking
        public ActionResult Index(string mess)
        {                
            return View();
        }

        [HttpPost]
        public JsonResult ValidateInDay(AjaxRequest request)
        {
            DateTime aDateTime = DateTime.Now;
            string dateStr = aDateTime.Year + "-" + aDateTime.Month + "-" + aDateTime.Day;
            if ("InDay".Equals(request.type))
            {
                var listHour = bookingDao.GetHourInDay(request.day, aDateTime.Hour);
                return Json(new { Hours = listHour, Status = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listHour = bookingDao.GetHourInDay(request.day, 7);
                return Json(new { Hours = listHour, Status = true }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult Add(Booking booking)
        {
            User user = (User)Session["USER"];
            bool check = bookingDao.CheckExistScheduleInDay(booking.Day, booking.Time, user.IdUser,booking.IdDoctor);
            if (check) {
                booking.IdUser = user.IdUser;
                bookingDao.Add(booking);
                return RedirectToAction("HistoryBooking", "Authencation", new { mess = "1" });
            }
            else
            {
                return RedirectToAction("HistoryBooking", "Authencation", new { mess = "2"});
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            bookingDao.delete(id);
            return RedirectToAction("HistoryBooking", "Authencation", new { mess = "3" });
        }
       
    }
}
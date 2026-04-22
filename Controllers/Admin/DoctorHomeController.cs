using YTeAspMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTeAspMVC.Daos;

namespace YTeAspMVC.Controllers.Admin
{
    public class DoctorHomeController : Controller
    {
        YTeDBContext myDb = new YTeDBContext();

        // GET: DoctorHome
        public ActionResult Index(int? month, int? year)
        {
            Doctor doctor = (Doctor)Session["DOCTOR"];
            if (doctor == null)
            {
                return RedirectToAction("LoginDoctor", "AdminAuthentication");
            }

            int m = month ?? DateTime.Now.Month;
            int y = year ?? DateTime.Now.Year;

            ViewBag.CurrentMonth = m;
            ViewBag.CurrentYear = y;

            // Lấy danh sách lịch khám của bác sĩ
            var allBookings = myDb.Bookings.Where(x => x.IdDoctor == doctor.IdDoctor).ToList();
            
            Dictionary<int, int> bookingCounts = new Dictionary<int, int>();
            
            foreach(var b in allBookings) {
                // Cố gắng parse ngày "yyyy-MM-dd"
                if(DateTime.TryParse(b.Day, out DateTime bDate)) {
                    if(bDate.Month == m && bDate.Year == y) {
                        if(!bookingCounts.ContainsKey(bDate.Day)) {
                            bookingCounts[bDate.Day] = 0;
                        }
                        bookingCounts[bDate.Day]++;
                    }
                }
            }
            
            ViewBag.BookingCounts = bookingCounts;
            return View();
        }

        [HttpPost]
        public JsonResult GetBookingsByDay(int day, int month, int year)
        {
            Doctor doctor = (Doctor)Session["DOCTOR"];
            if (doctor == null)
            {
                return Json(new { success = false, message = "Phiên đăng nhập đã hết hạn" });
            }

            var allBookings = myDb.Bookings.Include("User").Where(x => x.IdDoctor == doctor.IdDoctor).ToList();
            var dailyBookings = new List<object>();

            foreach (var b in allBookings)
            {
                if (DateTime.TryParse(b.Day, out DateTime bDate))
                {
                    if (bDate.Day == day && bDate.Month == month && bDate.Year == year)
                    {
                        string statusStr = "Chưa rõ";
                        if (b.Status == 0) statusStr = "Chờ duyệt";
                        else if (b.Status == 1) statusStr = "Đã duyệt / Chờ khám";
                        else if (b.Status == 2) statusStr = "Đã khám xong";

                        dailyBookings.Add(new
                        {
                            CustomerName = b.User != null ? b.User.FullName : "Khách hàng",
                            BookingDay = b.Day,
                            Time = b.Time + ":00",
                            Reason = string.IsNullOrEmpty(b.Reason) ? "Không có" : b.Reason,
                            Status = statusStr
                        });
                    }
                }
            }

            return Json(new { success = true, data = dailyBookings });
        }
    }
}

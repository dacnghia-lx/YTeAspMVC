using System.Collections.Generic;
using YTeAspMVC.Models;

namespace Website.Daos
{
    public interface IBookingDao
    {
        // Lưu ý: Đổi kiểu dữ liệu string/DateTime của Day, Time cho khớp với Model của bạn nhé
        bool CheckExistScheduleInDay(string day, int time, int idUser, int idDoctor);
        void Add(Booking booking);
        void delete(int id);
        // Nếu có hàm GetHourInDay ở trên, bạn cũng nên khai báo luôn vào đây
        List<int> GetHourInDay(string day, int hour);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public class DateHelper
    {
        public static bool IsToday(DateTime dateTime)
        {
            return dateTime.Date == DateTime.Today;
        }

        public static bool IsTomorrow(DateTime dateTime)
        {
            return dateTime.Date == (DateTime.Today.AddDays(1).Date);
        }

        public static bool DateInThisWeek(DateTime dateTime)
        {
            DateTime beginning, end;
            GetWeek(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture, out beginning, out end);
            return dateTime.Date <= end.Date;
        }

        public static void GetWeek(DateTime now, System.Globalization.CultureInfo cultureInfo, out DateTime begining, out DateTime end)
        {
            if (now == null)
                throw new ArgumentNullException("now");
            if (cultureInfo == null)
                throw new ArgumentNullException("cultureInfo");

            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            int offset = firstDayOfWeek - now.DayOfWeek;
            if (offset != 1)
            {
                DateTime weekStart = now.AddDays(offset);
                DateTime endOfWeek = weekStart.AddDays(6);
                begining = weekStart;
                end = endOfWeek;
            }
            else
            {
                begining = now.AddDays(-6);
                end = now;
            }
        }

        public static bool DateInThisMonth(DateTime dateTime)
        {
            return (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month);
        }

        public static bool DateInNextMonth(DateTime dateTime)
        {
            DateTime currentNextMonth = DateTime.Now.AddMonths(1);
            return dateTime.Month == currentNextMonth.Month && dateTime.Year == currentNextMonth.Year;
        }

        public static string GetTimePartString(DateTime dateTime)
        {
            return dateTime.ToShortTimeString();
        }

        public static string GetWeekDayString(DateTime dateTime)
        {
            return dateTime.ToString("ddd");
        }

        public static string GetDayOfMonthString(DateTime dateTime)
        {
            return dateTime.ToString("dd-MMM");
        }

        public static DateTime ToDateTime(string dateTimeString)
        {
            DateTime dateTime;
            DateTime.TryParse(dateTimeString, out dateTime);
            return dateTime;
        }

        public static string ToString(DateTime dateTime)
        {
            return dateTime.ToString();
        }
    }
}

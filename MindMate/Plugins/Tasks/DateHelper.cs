using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public class DateHelper
    {
        public const int DEFAULT_HOUR = 7;

        public static bool IsOverdue(DateTime dateTime)
        {
            return dateTime.Date < DateTime.Today;
        }

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
            return dateTime.Date >= beginning.Date && dateTime.Date <= end.Date;
        }

        public static void GetWeek(DateTime now, System.Globalization.CultureInfo cultureInfo, out DateTime begining, out DateTime end)
        {
            if (now == null)
                throw new ArgumentNullException(nameof(now));
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            int offset = firstDayOfWeek - now.DayOfWeek;
            if (offset != 1)
            {
                DateTime weekStart = now.AddDays(offset);
                DateTime endOfWeek = weekStart.AddDays(6);
                begining = weekStart.Date;
                end = endOfWeek.Date;
            }
            else
            {
                begining = now.AddDays(-6).Date;
                end = now.Date;
            }
        }

        public static DateTime GetFirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);            
        }

        public static DateTime GetLastDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
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
            DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return dateTime;
        }

        public static string ToString(DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.InvariantCulture);
        }

        public static DateTime GetDefaultDueDate()
        {
            return DateTime.Today.AddHours(DEFAULT_HOUR);
        }

        public static DateTime GetDefaultDueDateToday()
        {
            DateTime dateTime = DateTime.Today.AddHours(DateTime.Now.Hour + 1.5);
            return IsToday(dateTime) ? dateTime : DateTime.Now;
        }

        public static DateTime GetDefaultDueDateTomorrow()
        {
            return DateTime.Today.AddDays(1).AddHours(DEFAULT_HOUR);
        }

        public static DateTime GetDefaultDueDateNextWeek()
        {
            return DateTime.Today.AddDays(3 + (7 - (int)DateTime.Today.DayOfWeek)).AddHours(DEFAULT_HOUR);
        }

        public static DateTime GetDefaultDueDateNextMonth()
        {
            return DateTime.Today.AddDays(10 + (DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) - (int)DateTime.Today.Day)).AddHours(DEFAULT_HOUR);
        }

        public static int GetQuarter(DateTime dateTime)
        {
            if (dateTime.Month < 4)
                return 1;
            else if (dateTime.Month < 7)
                return 2;
            else if (dateTime.Month < 10)
                return 3;
            else
                return 4;
        }

        public static DateTime GetDefaultDueDateNextQuarter()
        {
            int currentQuarter = GetQuarter(DateTime.Today);
            switch(currentQuarter)
            {
                case 1:
                    return new DateTime(DateTime.Today.Year, 5, 1, DEFAULT_HOUR, 0, 0);
                case 2:
                    return new DateTime(DateTime.Today.Year, 8, 1, DEFAULT_HOUR, 0, 0);
                case 3:
                    return new DateTime(DateTime.Today.Year, 11, 1, DEFAULT_HOUR, 0, 0);
                case 4:
                default:
                    return new DateTime(DateTime.Today.Year + 1, 2, 1, DEFAULT_HOUR, 0, 0);
            }
        }

        /// <summary>
        /// Returns a value indicating if two date ranges intersect
        /// </summary>
        /// <param name="startA"></param>
        /// <param name="endA"></param>
        /// <param name="startB"></param>
        /// <param name="endB"></param>
        /// <returns></returns>
        public static bool DateIntersects(DateTime startA, DateTime endA, DateTime startB, DateTime endB)
        {
            return startB < endA && startA < endB;
        }
    }
}

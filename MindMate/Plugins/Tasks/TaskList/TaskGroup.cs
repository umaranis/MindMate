using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public interface ITaskGroup
    {
        bool CanContain(DateTime dateTime);

        string ShortDueDateString(DateTime dateTime);

        DateTime StartTime { get; }

        DateTime EndTime{ get; }
    }
    
    public class TaskGroupOverdue : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) { return dateTime.Date < DateTime.Today; }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToString("dd-MMM"); }
        
        public DateTime StartTime { get { return DateTime.MinValue; } }

        public DateTime EndTime { get { return DateTime.Today.Subtract(TimeSpan.FromSeconds(1)); } }
    }


    public class TaskGroupToday : ITaskGroup
    {
        public bool CanContain(DateTime dateTime)  { return dateTime.Date == DateTime.Today;   }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToShortTimeString(); }

        public DateTime StartTime { get { return DateTime.Today; } }

        public DateTime EndTime { get { return DateTime.Today.Add(new TimeSpan(23, 59, 59)); } }
    }

    public class TaskGroupTomorrow : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) { return dateTime.Date == (DateTime.Today.AddDays(1).Date); }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToShortTimeString(); }

        public DateTime StartTime { get { return DateTime.Today.AddDays(1).Date; } }

        public DateTime EndTime { get { return StartTime.Add(new TimeSpan(23, 59, 59)); } }
    }

    public class TaskGroupThisWeek : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) 
        {
            DateTime beginning, end;
            DateHelper.GetWeek(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture, out beginning, out end);
            return dateTime.Date >= beginning.Date && dateTime.Date <= end.Date;
        }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToString("ddd"); }

        public DateTime StartTime
        { 
            get 
            {
                DateTime beginning, end;
                DateHelper.GetWeek(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture, out beginning, out end);
                return beginning; 
            } 
        }

        public DateTime EndTime 
        { 
            get 
            {
                DateTime beginning, end;
                DateHelper.GetWeek(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture, out beginning, out end);
                return end.Add(new TimeSpan(23, 59, 59));
            } 
        }
    }

    public class TaskGroupThisMonth : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) { 
            return (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month); 
        }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToString("dd-MMM"); }

        public DateTime StartTime { get { return DateHelper.GetFirstDayOfMonth(DateTime.Now); } }

        public DateTime EndTime { get { return DateHelper.GetLastDayOfMonth(DateTime.Now).Add(new TimeSpan(23, 59, 59)); } }
    }

    public class TaskGroupNextMonth : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) {
            DateTime currentNextMonth = DateTime.Now.AddMonths(1);
            return dateTime.Month == currentNextMonth.Month && dateTime.Year == currentNextMonth.Year;
        }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToString("dd-MMM"); }

        public DateTime StartTime { get { return DateHelper.GetFirstDayOfMonth(DateTime.Now.AddMonths(1)); } }

        public DateTime EndTime { get { return DateHelper.GetLastDayOfMonth(DateTime.Now.AddMonths(1)).Add(new TimeSpan(23, 59, 59)); } }
    }

}

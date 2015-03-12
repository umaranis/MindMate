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
    }
    
    public class TaskGroupOverdue : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) { return dateTime.Date < DateTime.Today; }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToString("dd-MMM"); }
    }


    public class TaskGroupToday : ITaskGroup
    {
        public bool CanContain(DateTime dateTime)  { return dateTime.Date == DateTime.Today;   }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToShortTimeString(); }
    }

    public class TaskGroupTomorrow : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) { return dateTime.Date == (DateTime.Today.AddDays(1).Date); }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToShortTimeString(); }
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
    }

    public class TaskGroupThisMonth : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) { 
            return (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month); 
        }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToString("dd-MMM"); }
    }

    public class TaskGroupNextMonth : ITaskGroup
    {
        public bool CanContain(DateTime dateTime) {
            DateTime currentNextMonth = DateTime.Now.AddMonths(1);
            return dateTime.Month == currentNextMonth.Month && dateTime.Year == currentNextMonth.Year;
        }

        public string ShortDueDateString(DateTime dateTime) { return dateTime.ToString("dd-MMM"); }
    }

}

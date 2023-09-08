using System;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public partial class DateTimePicker : Form
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }

        public DateTime Value
        {
            get => new DateTime(datePicker.SelectionStart.Year, datePicker.SelectionStart.Month, datePicker.SelectionStart.Day,
                    timePicker.Value.Hour, timePicker.Value.Minute, timePicker.Value.Second);
            set
            {
                datePicker.SelectionStart = value;
                timePicker.Value = value;
            }
        }
    }
}

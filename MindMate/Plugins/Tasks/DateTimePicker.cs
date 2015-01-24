using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            get
            {
                return new DateTime(datePicker.SelectionStart.Year, datePicker.SelectionStart.Month, datePicker.SelectionStart.Day,
                    timePicker.Value.Hour, timePicker.Value.Minute, timePicker.Value.Second);
            }
            set
            {
                datePicker.SelectionStart = value;
                timePicker.Value = value;
            }
        }
    }
}

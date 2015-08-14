using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks.Calender
{
    public class CalendarContextMenu : ContextMenuStrip
    {

        public CalendarContextMenu()
        {
            LoadItems();
        }

        private void LoadItems()
        {
            MenuCompleteTask = (ToolStripMenuItem)Items.Add("Complete Task", TaskRes.tick);
            MenuRemoveTask = (ToolStripMenuItem)Items.Add("Remove Task", TaskRes.date_delete);
            MenuEditDueDate = (ToolStripMenuItem)Items.Add("Edit Due Date ...", TaskRes.date_edit);

            MenuQuickDates = new ToolStripMenuItem("Quick Due Dates");
            Items.Add(MenuQuickDates);

            MenuDueDateToday = new ToolStripMenuItem("Today");
            MenuDueDateTomorrow = new ToolStripMenuItem("Tomorrow");
            MenuDueDateNextWeek = new ToolStripMenuItem("Next Week");
            MenuDueDateNextMonth = new ToolStripMenuItem("Next Month");
            MenuDueDateNextQuarter = new ToolStripMenuItem("Next Quarter");

            MenuQuickDates.DropDownItems.Add(MenuDueDateToday);
            MenuQuickDates.DropDownItems.Add(MenuDueDateTomorrow);
            MenuQuickDates.DropDownItems.Add(MenuDueDateNextWeek);
            MenuQuickDates.DropDownItems.Add(MenuDueDateNextMonth);
            MenuQuickDates.DropDownItems.Add(MenuDueDateNextQuarter);

            Items.Add(new ToolStripSeparator());

            MenuEditText = (ToolStripMenuItem)Items.Add("Edit text");

            this.MenuTimescale = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHourTImescale = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu30MinsTimeScale = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu15MinsTimescale = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu10MinsTimescale = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu6MinsTimescale = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu5MinsTimeScale = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // MenuTimescale
            // 
            this.MenuTimescale.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHourTImescale,
            this.Menu30MinsTimeScale,
            this.Menu15MinsTimescale,
            this.Menu10MinsTimescale,
            this.Menu6MinsTimescale,
            this.Menu5MinsTimeScale});
            this.MenuTimescale.Name = "MenuTimescale";
            this.MenuTimescale.Size = new System.Drawing.Size(181, 26);
            this.MenuTimescale.Text = "Timescale";
            // 
            // MenuHourTImescale
            // 
            this.MenuHourTImescale.Name = "MenuHourTImescale";
            this.MenuHourTImescale.Size = new System.Drawing.Size(181, 26);
            this.MenuHourTImescale.Text = "1 hour";
            
            // 
            // Menu30MinsTimeScale
            // 
            this.Menu30MinsTimeScale.Name = "Menu30MinsTimeScale";
            this.Menu30MinsTimeScale.Size = new System.Drawing.Size(181, 26);
            this.Menu30MinsTimeScale.Text = "30 minutes";
            
            // 
            // Menu15MinsTimescale
            // 
            this.Menu15MinsTimescale.Name = "Menu15MinsTimescale";
            this.Menu15MinsTimescale.Size = new System.Drawing.Size(181, 26);
            this.Menu15MinsTimescale.Text = "15 minutes";
            
            // 
            // Menu10MinsTimescale
            // 
            this.Menu10MinsTimescale.Name = "Menu10MinsTimescale";
            this.Menu10MinsTimescale.Size = new System.Drawing.Size(181, 26);
            this.Menu10MinsTimescale.Text = "10 minutes";
            
            // 
            // Menu6MinsTimescale
            // 
            this.Menu6MinsTimescale.Name = "Menu6MinsTimescale";
            this.Menu6MinsTimescale.Size = new System.Drawing.Size(181, 26);
            this.Menu6MinsTimescale.Text = "6 minutes";
            
            // 
            // Menu5MinsTimeScale
            // 
            this.Menu5MinsTimeScale.Name = "Menu5MinsTimeScale";
            this.Menu5MinsTimeScale.Size = new System.Drawing.Size(181, 26);
            this.Menu5MinsTimeScale.Text = "5 minutes";

            Items.Add(MenuTimescale);


        }

        public ToolStripMenuItem MenuCompleteTask { get; private set; }
        public ToolStripMenuItem MenuRemoveTask { get; private set; }
        public ToolStripMenuItem MenuEditDueDate { get; private set; }
        public ToolStripMenuItem MenuQuickDates { get; private set; }
        public ToolStripMenuItem MenuDueDateToday { get; private set; }
        public ToolStripMenuItem MenuDueDateTomorrow { get; private set; }
        public ToolStripMenuItem MenuDueDateNextWeek { get; private set; }
        public ToolStripMenuItem MenuDueDateNextMonth { get; private set; }
        public ToolStripMenuItem MenuDueDateNextQuarter { get; private set; }
        public ToolStripMenuItem MenuEditText { get; private set; }

        public System.Windows.Forms.ToolStripMenuItem MenuTimescale { get; private set; }
        public System.Windows.Forms.ToolStripMenuItem MenuHourTImescale {get; private set;}
        public System.Windows.Forms.ToolStripMenuItem Menu30MinsTimeScale  {get; private set;}
        public System.Windows.Forms.ToolStripMenuItem Menu15MinsTimescale  {get; private set;}
        public System.Windows.Forms.ToolStripMenuItem Menu10MinsTimescale  {get; private set;}
        public System.Windows.Forms.ToolStripMenuItem Menu6MinsTimescale   {get; private set;}
        public System.Windows.Forms.ToolStripMenuItem Menu5MinsTimeScale   {get; private set;}
    }
}

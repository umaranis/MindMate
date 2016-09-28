using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    public partial class HtmlSourceDialog : Form
    {
        public HtmlSourceDialog()
        {
            InitializeComponent();
            Debugging.FormDebugHooks.Instance.ApplyHook(this);
        }

        public string HtmlSource
        {
            get
            {
                return txtSource.Text;
            }
            set
            {
                txtSource.Text = value;
            }
        }
    }    
}

using System;
using System.Windows.Forms;
using MindMate.Controller;
using MindMate.View.MapControls;

namespace MindMate.Tests.TestDouble
{
    class MainCtrlStub : IMainCtrl
    {
        private Form form;
        public MainCtrlStub(Form form)
        {
            MindMate.MetaModel.MetaModel.Initialize();
            this.form = form;
        }

        public void AddMainPanel(MindMate.View.MapControls.MapViewPanel mapViewPanel)
        {
            form.Controls.Add(mapViewPanel);
        }

        public System.Drawing.Color ShowColorPicker(System.Drawing.Color currentColor)
        {
            return System.Drawing.Color.Red;
        }

        public System.Drawing.Font ShowFontDialog(System.Drawing.Font currentFont)
        {
            return new System.Drawing.Font(System.Drawing.FontFamily.GenericSerif, 16);
        }        

        public void ShowStatusNotification(string msg)
        {
            
        }

        public void ShowMessageBox(string title, string msg, MessageBoxIcon icon)
        {
            throw new NotImplementedException();
        }        

        public NodeContextMenu NodeContextMenu { get; }
    }
}

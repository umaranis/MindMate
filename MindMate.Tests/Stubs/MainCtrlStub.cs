using System.Windows.Forms;
using MindMate.Controller;
using MindMate.View.MapControls;

namespace MindMate.Tests.Stubs
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

        public bool SeekDeleteConfirmation(string msg)
        {
            return true;
        }

        public void ShowStatusNotification(string msg)
        {
            
        }

        public NodeContextMenu NodeContextMenu { get; }
    }
}

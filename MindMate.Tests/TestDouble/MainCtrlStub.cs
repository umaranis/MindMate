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

        public void ShowStatusNotification(string msg)
        {
            
        }              

        public NodeContextMenu NodeContextMenu { get; }
    }
}

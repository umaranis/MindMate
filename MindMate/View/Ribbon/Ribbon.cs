using RibbonLib.Controls;
using RibbonLib.Controls.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.Ribbon
{
    public class Ribbon
    {
        private Controller.MainCtrl mainCtrl;

        private RibbonApplicationMenu _applicationMenu;
        private RibbonButton _buttonNew;
        private RibbonButton _buttonOpen;
        private RibbonButton _buttonSave;
        private RibbonButton _buttonExit;

        public Ribbon(RibbonLib.Ribbon ribbon, Controller.MainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;

            _applicationMenu = new RibbonApplicationMenu(ribbon, (uint)RibbonMarkupCommands.cmdApplicationMenu);
            _buttonNew = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdButtonNew);
            _buttonOpen = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdButtonOpen);
            _buttonSave = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdButtonSave);
            _buttonExit = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdButtonExit);

            _applicationMenu.TooltipTitle = "Menu";
            _applicationMenu.TooltipDescription = "Application main menu";

            _buttonNew.ExecuteEvent += _buttonNew_ExecuteEvent;
            _buttonExit.ExecuteEvent += _buttonExit_ExecuteEvent;
            _buttonOpen.ExecuteEvent += _buttonOpen_ExecuteEvent;
            _buttonSave.ExecuteEvent += _buttonSave_ExecuteEvent;
        }

        private void _buttonSave_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.SaveMap();
        }

        private void _buttonOpen_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.OpenMap();
        }

        private void _buttonExit_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        void _buttonNew_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.NewMap();
        }

    }
}

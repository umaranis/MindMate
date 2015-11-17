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

        private RibbonGroup _groupNewNode;
        private RibbonButton _btnNewChildNode;
        private RibbonButton _btnNewLongNode;
        private RibbonButton _btnNewNodeAbove;

        private RibbonButton _btnNewNodeBelow;
        private RibbonButton _btnNewParent;

        private RibbonGroup _groupEdit;
        private RibbonButton _btnEditText;
        private RibbonButton _btnEditLong;
        private RibbonButton _btnDeleteNode;

        public Ribbon(RibbonLib.Ribbon ribbon, Controller.MainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;

            //Application Menu
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

            //Home Tab : New Node group
            _groupNewNode = new RibbonGroup(ribbon, (uint)RibbonMarkupCommands.cmdNewNode);
            _btnNewChildNode = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdNewChildNode);
            _btnNewLongNode = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdNewLongNode);
            _btnNewNodeAbove = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdNewNodeAbove);
            _btnNewNodeBelow = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdNewNodeBelow);
            _btnNewParent = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdNewNodeParent);

            _btnNewChildNode.ExecuteEvent += _btnNewChildNode_ExecuteEvent;
            _btnNewLongNode.ExecuteEvent += _btnNewLongNode_ExecuteEvent;
            _btnNewNodeAbove.ExecuteEvent += _btnNewNodeAbove_ExecuteEvent;
            _btnNewNodeBelow.ExecuteEvent += _btnNewNodeBelow_ExecuteEvent;
            _btnNewParent.ExecuteEvent += _btnNewParent_ExecuteEvent;

            //Home Tab: Edit Group
            _groupEdit = new RibbonGroup(ribbon, (uint)RibbonMarkupCommands.cmdEdit);
            _btnEditText = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdEditText);
            _btnEditLong = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdEditLong);
            _btnDeleteNode = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.cmdDeleteNode);

            _btnEditText.ExecuteEvent += _btnEditText_ExecuteEvent;
            _btnEditLong.ExecuteEvent += _btnEditLong_ExecuteEvent;
            _btnDeleteNode.ExecuteEvent += _btnDeleteNode_ExecuteEvent;

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

        #region Home Tab

        private void _btnNewChildNode_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.mapCtrl.AppendChildNodeAndEdit();
        }

        private void _btnNewLongNode_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.mapCtrl.AppendMultiLineNodeAndEdit();
        }

        private void _btnNewNodeAbove_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.mapCtrl.AppendSiblingAboveAndEdit();
        }

        private void _btnNewNodeBelow_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.mapCtrl.AppendSiblingNodeAndEdit();
        }

        private void _btnNewParent_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.mapCtrl.InsertParentAndEdit();
        }

        private void _btnEditText_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.mapCtrl.BeginCurrentNodeEdit(MapControls.TextCursorPosition.Undefined);
        }

        private void _btnEditLong_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.mapCtrl.MultiLineNodeEdit();
        }

        private void _btnDeleteNode_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.mapCtrl.DeleteSelectedNodes();
        }

        #endregion Home Tab

    }
}

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

        private RibbonGroup _grpNewNode;
        private RibbonButton _btnNewChildNode;
        private RibbonButton _btnNewLongNode;
        private RibbonButton _btnNewNodeAbove;

        private RibbonButton _btnNewNodeBelow;
        private RibbonButton _btnNewParent;

        private RibbonGroup _grpEdit;
        private RibbonButton _btnEditText;
        private RibbonButton _btnEditLong;
        private RibbonButton _btnDeleteNode;

        private RibbonGroup _grpClipboard;
        private RibbonButton _btnPaste;
        private RibbonButton _btnPasteAsText;
        private RibbonButton _btnCut;
        private RibbonButton _btnCopy;
        private RibbonToggleButton _btnFormatPainter;

        private RibbonGroup _grpFont;
        private RibbonFontControl _RichFont;

        public Ribbon(RibbonLib.Ribbon ribbon, Controller.MainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;

            //Application Menu
            _applicationMenu = new RibbonApplicationMenu(ribbon, (uint)RibbonMarkupCommands.ApplicationMenu);
            _buttonNew = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.ButtonNew);
            _buttonOpen = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.ButtonOpen);
            _buttonSave = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.ButtonSave);
            _buttonExit = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.ButtonExit);

            _applicationMenu.TooltipTitle = "Menu";
            _applicationMenu.TooltipDescription = "Application main menu";

            _buttonNew.ExecuteEvent += _buttonNew_ExecuteEvent;
            _buttonExit.ExecuteEvent += _buttonExit_ExecuteEvent;
            _buttonOpen.ExecuteEvent += _buttonOpen_ExecuteEvent;
            _buttonSave.ExecuteEvent += _buttonSave_ExecuteEvent;

            //Home Tab : New Node group
            _grpNewNode = new RibbonGroup(ribbon, (uint)RibbonMarkupCommands.NewNode);
            _btnNewChildNode = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.NewChildNode);
            _btnNewLongNode = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.NewLongNode);
            _btnNewNodeAbove = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.NewNodeAbove);
            _btnNewNodeBelow = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.NewNodeBelow);
            _btnNewParent = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.NewNodeParent);

            _btnNewChildNode.ExecuteEvent += _btnNewChildNode_ExecuteEvent;
            _btnNewLongNode.ExecuteEvent += _btnNewLongNode_ExecuteEvent;
            _btnNewNodeAbove.ExecuteEvent += _btnNewNodeAbove_ExecuteEvent;
            _btnNewNodeBelow.ExecuteEvent += _btnNewNodeBelow_ExecuteEvent;
            _btnNewParent.ExecuteEvent += _btnNewParent_ExecuteEvent;

            //Home Tab: Edit group
            _grpEdit = new RibbonGroup(ribbon, (uint)RibbonMarkupCommands.GrpEdit);
            _btnEditText = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.EditText);
            _btnEditLong = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.EditLong);
            _btnDeleteNode = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.DeleteNode);

            _btnEditText.ExecuteEvent += _btnEditText_ExecuteEvent;
            _btnEditLong.ExecuteEvent += _btnEditLong_ExecuteEvent;
            _btnDeleteNode.ExecuteEvent += _btnDeleteNode_ExecuteEvent;

            //Home Tab: Cipboard group
            _grpClipboard = new RibbonGroup(ribbon, (uint)RibbonMarkupCommands.GrpClipboard);
            _btnPaste = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.Paste);
            _btnPasteAsText = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.PasteAsText);
            _btnCut = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.Cut);
            _btnCopy = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.Copy);
            _btnFormatPainter = new RibbonToggleButton(ribbon, (uint)RibbonMarkupCommands.FormatPainter);

            _btnPaste.ExecuteEvent += _btnPaste_ExecuteEvent;
            _btnPasteAsText.ExecuteEvent += _btnPasteAsText_ExecuteEvent;
            _btnCut.ExecuteEvent += _btnCut_ExecuteEvent;
            _btnCopy.ExecuteEvent += _btnCopy_ExecuteEvent;
            _btnFormatPainter.ExecuteEvent += _btnFormatPainter_ExecuteEvent;

            //Home Tab: Font group
            _grpFont = new RibbonGroup(ribbon, (uint)RibbonMarkupCommands.GrpFont);
            _RichFont = new RibbonFontControl(ribbon, (uint)RibbonMarkupCommands.RichFont);                        
            
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

        private void _btnPaste_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.Paste();
        }

        private void _btnPasteAsText_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.Paste(true);
        }

        private void _btnCut_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.Cut();
        }

        private void _btnCopy_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.Copy();
        }

        private void _btnFormatPainter_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            if (_btnFormatPainter.BooleanValue)
            {
                bool ctrlKeyDown = (Control.ModifierKeys & Keys.Control) == Keys.Control;
                mainCtrl.mapCtrl.CopyFormat(ctrlKeyDown);
            }
            else
            {
                mainCtrl.mapCtrl.ClearFormatPainter();
            }
        }

        #endregion Home Tab

        internal void RegisterForChange()
        {
            mainCtrl.mapCtrl.MapView.FormatPainter.StateChanged += FormatPainter_StateChanged;
        }

        private void FormatPainter_StateChanged(MapControls.MapViewFormatPainter painter)
        {
            switch(painter.Status)
            {
                case MapControls.FormatPainterStatus.Empty:
                    _btnFormatPainter.BooleanValue = false;
                    break;
                case MapControls.FormatPainterStatus.SingleApply:
                case MapControls.FormatPainterStatus.MultiApply:
                    _btnFormatPainter.BooleanValue = true;
                    break;
            }
        }

        #region Events to Refresh Command State

        #endregion

    }
}

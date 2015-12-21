using MindMate.Model;
using RibbonLib.Controls;
using RibbonLib.Controls.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RibbonLib.Interop;
using System.Diagnostics;
using MindMate.View.EditorTabs;

namespace MindMate.View.Ribbon
{
    public class Ribbon
    {
        private readonly Controller.MainCtrl mainCtrl;

        private readonly RibbonApplicationMenu _applicationMenu;
        private readonly RibbonButton _buttonNew;
        private readonly RibbonButton _buttonOpen;
        private readonly RibbonButton _buttonSave;
        private readonly RibbonButton closeMap;
        private readonly RibbonButton _buttonExit;

        private RibbonGroup _grpNewNode;
        private readonly RibbonButton _btnNewChildNode;
        private readonly RibbonButton _btnNewLongNode;
        private readonly RibbonButton _btnNewNodeAbove;

        private readonly RibbonButton _btnNewNodeBelow;
        private readonly RibbonButton _btnNewParent;

        private RibbonGroup _grpEdit;
        private readonly RibbonButton _btnEditText;
        private readonly RibbonButton _btnEditLong;
        private readonly RibbonButton _btnDeleteNode;

        private RibbonGroup _grpClipboard;
        private readonly RibbonButton _btnPaste;
        private readonly RibbonButton _btnPasteAsText;
        private readonly RibbonButton _btnCut;
        private readonly RibbonButton _btnCopy;
        private readonly RibbonToggleButton _btnFormatPainter;

        private RibbonGroup _grpFont;
        private RibbonFontControl _RichFont;

        private readonly RibbonLib.Ribbon ribbon;

        public Ribbon(RibbonLib.Ribbon ribbon, Controller.MainCtrl mainCtrl, EditorTabs.EditorTabs tabs)
        {
            this.ribbon = ribbon;
            this.mainCtrl = mainCtrl;

            //Application Menu
            _applicationMenu = new RibbonApplicationMenu(ribbon, (uint)RibbonMarkupCommands.ApplicationMenu);
            _buttonNew = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.ButtonNew);
            _buttonOpen = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.ButtonOpen);
            _buttonSave = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.ButtonSave);
            closeMap = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.Close);
            _buttonExit = new RibbonButton(ribbon, (uint)RibbonMarkupCommands.ButtonExit);

            _applicationMenu.TooltipTitle = "Menu";
            _applicationMenu.TooltipDescription = "Application main menu";

            _buttonNew.ExecuteEvent += _buttonNew_ExecuteEvent;
            _buttonExit.ExecuteEvent += _buttonExit_ExecuteEvent;
            _buttonOpen.ExecuteEvent += _buttonOpen_ExecuteEvent;
            _buttonSave.ExecuteEvent += _buttonSave_ExecuteEvent;
            closeMap.ExecuteEvent += CloseMap_ExecuteEvent;

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

            _RichFont.ExecuteEvent += _RichFont_ExecuteEvent;

            mainCtrl.PersistenceManager.CurrentTreeChanged += PersistenceManager_CurrentTreeChanged;
            MindMate.Model.ClipboardManager.StatusChanged += ClipboardManager_StatusChanged;
            tabs.ControlAdded += Tabs_ControlAdded;
            tabs.ControlRemoved += Tabs_ControlRemoved;
            tabs.SelectedIndexChanged += Tabs_SelectedIndexChanged;
            
        }

        private void _buttonSave_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.SaveCurrentMap();
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

        private void CloseMap_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CloseCurrentMap();
        }

        #region Home Tab

        private void _btnNewChildNode_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendChildNodeAndEdit();
        }

        private void _btnNewLongNode_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendMultiLineNodeAndEdit();
        }

        private void _btnNewNodeAbove_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendSiblingAboveAndEdit();
        }

        private void _btnNewNodeBelow_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendSiblingNodeAndEdit();
        }

        private void _btnNewParent_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.InsertParentAndEdit();
        }

        private void _btnEditText_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.BeginCurrentNodeEdit(MapControls.TextCursorPosition.Undefined);
        }

        private void _btnEditLong_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.MultiLineNodeEdit();
        }

        private void _btnDeleteNode_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.DeleteSelectedNodes();
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
                mainCtrl.CurrentMapCtrl.CopyFormat(ctrlKeyDown);
            }
            else
            {
                mainCtrl.CurrentMapCtrl.ClearFormatPainter();
            }
        }

        private void _RichFont_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            
            PropVariant propChangesProperties;
            e.CommandExecutionProperties.GetValue(ref RibbonProperties.FontProperties_ChangedProperties, out propChangesProperties);
            IPropertyStore changedProperties = (IPropertyStore)propChangesProperties.Value;
            uint changedPropertiesNumber;
            changedProperties.GetCount(out changedPropertiesNumber);

            for (uint i = 0; i < changedPropertiesNumber; ++i)
            {
                PropertyKey propertyKey;
                changedProperties.GetAt(i, out propertyKey);
                //Debug.WriteLine(RibbonProperties.GetPropertyKeyName(ref propertyKey));
                if (propertyKey == RibbonProperties.FontProperties_Bold)
                {
                    mainCtrl.CurrentMapCtrl.MakeSelectedNodeBold();
                }
                else if (propertyKey == RibbonProperties.FontProperties_Italic)
                {
                    mainCtrl.CurrentMapCtrl.MakeSelectedNodeItalic();
                }
            }
        }

        #endregion Home Tab
        
        #region Events to Refresh Command State
        
        private void PersistenceManager_CurrentTreeChanged(Serialization.PersistenceManager manager, Serialization.PersistentTree oldTree, Serialization.PersistentTree newTree)
        {
            if (oldTree != null)
            {
                oldTree.Tree.SelectedNodes.NodeSelected -= SelectedNodes_NodeSelected;
                oldTree.Tree.SelectedNodes.NodeDeselected -= SelectedNodes_NodeDeselected;
            }

            if (newTree != null)
            {
                newTree.Tree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
                newTree.Tree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;
                UpdateFontControl(newTree.Tree.SelectedNodes);
            }
            else
            {
                ClearFontControl();
            }
            
        }

        private void SelectedNodes_NodeSelected(MapNode node, SelectedNodes selectedNodes)
        {
            UpdateFontControl(selectedNodes);
        }

        private void SelectedNodes_NodeDeselected(MapNode node, SelectedNodes selectedNodes)
        {
            UpdateFontControl(selectedNodes);
        }

        private void UpdateFontControl(SelectedNodes nodes)
        {
            if (nodes.Count == 1)
            {
                UpdateFontControl(nodes.First);
            }
            else
            {
                ClearFontControl();
            }
        }

        private void UpdateFontControl(MapNode n)
        {
            _RichFont.Bold = n.Bold ? FontProperties.Set : FontProperties.NotSet;
            _RichFont.Italic = n.Italic ? FontProperties.Set : FontProperties.NotSet;
        }

        private void ClearFontControl()
        {
            _RichFont.Bold = FontProperties.NotSet;
            _RichFont.Italic = FontProperties.NotSet;
        }

        private void ClipboardManager_StatusChanged()
        {
            if (ClipboardManager.HasCutNode)
            {
                _btnCut.SmallImage = ribbon.ConvertToUIImage(Win7.Properties.Resources.cut_red_small);
            }
            else
            {
                _btnCut.SmallImage = ribbon.ConvertToUIImage(Win7.Properties.Resources.cut_small);
            }
        }

        private void Tabs_ControlAdded(object sender, ControlEventArgs e)
        {
            Tab tab = e.Control as Tab;
            if (tab != null)
            {
                tab.MapView.FormatPainter.StateChanged += FormatPainter_StateChanged;
            }
        }

        private void Tabs_ControlRemoved(object sender, ControlEventArgs e)
        {
            Tab tab = e.Control as Tab;
            if (tab != null)
            {
                tab.MapView.FormatPainter.StateChanged -= FormatPainter_StateChanged;
            }
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tab tab = ((EditorTabs.EditorTabs)sender).SelectedTab as Tab;
            if (tab != null)
            {
                UpdateFormatPainter(tab.MapView.FormatPainter);
            }
            else
            {
                _btnFormatPainter.BooleanValue = false;
            }
        }

        private void FormatPainter_StateChanged(MapControls.MapViewFormatPainter painter)
        {
            UpdateFormatPainter(painter);
        }

        private void UpdateFormatPainter(MapControls.MapViewFormatPainter painter)
        {
            switch (painter.Status)
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

        #endregion

    }
}

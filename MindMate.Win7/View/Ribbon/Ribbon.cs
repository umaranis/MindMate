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
using System.Drawing;
using MindMate.MetaModel;
using MindMate.View.EditorTabs;
using MindMate.View.MapControls;
using RibbonLib;

namespace MindMate.View.Ribbon
{
    public partial class Ribbon
    {
        private readonly Controller.MainCtrl mainCtrl;
        private readonly RibbonLib.Ribbon ribbon;

        public Ribbon(RibbonLib.Ribbon ribbon, Controller.MainCtrl mainCtrl, EditorTabs.EditorTabs tabs)
        {
            this.ribbon = ribbon;
            this.mainCtrl = mainCtrl;

            InitializeComponents();

            //Application Menu
            ApplicationMenu.TooltipTitle = "Menu";
            ApplicationMenu.TooltipDescription = "Application main menu";

            ButtonNew.ExecuteEvent += _buttonNew_ExecuteEvent;
            ButtonExit.ExecuteEvent += _buttonExit_ExecuteEvent;
            ButtonOpen.ExecuteEvent += _buttonOpen_ExecuteEvent;
            ButtonSave.ExecuteEvent += _buttonSave_ExecuteEvent;
            Close.ExecuteEvent += Close_ExecuteEvent;

            //Home Tab : New Node group
            NewChildNode.ExecuteEvent += NewChildNode_ExecuteEvent;
            NewLongNode.ExecuteEvent += NewLongNode_ExecuteEvent;
            NewNodeAbove.ExecuteEvent += NewNodeAbove_ExecuteEvent;
            NewNodeBelow.ExecuteEvent += NewNodeBelow_ExecuteEvent;
            NewNodeParent.ExecuteEvent += NewParent_ExecuteEvent;

            //Home Tab: Edit group
            EditText.ExecuteEvent += _btnEditText_ExecuteEvent;
            EditLong.ExecuteEvent += _btnEditLong_ExecuteEvent;
            DeleteNode.ExecuteEvent += _btnDeleteNode_ExecuteEvent;

            //Home Tab: Cipboard group
            Paste.ExecuteEvent += _btnPaste_ExecuteEvent;
            PasteAsText.ExecuteEvent += _btnPasteAsText_ExecuteEvent;
            Cut.ExecuteEvent += _btnCut_ExecuteEvent;
            Copy.ExecuteEvent += _btnCopy_ExecuteEvent;
            FormatPainter.ExecuteEvent += _btnFormatPainter_ExecuteEvent;

            //Home Tab: Font group
            RichFont.ExecuteEvent += _RichFont_ExecuteEvent;
            
            //Home Tab: Icons Group
            IconsGallery.ItemsSourceReady += _iconGallery_ItemsSourceReady;
            IconsGallery.ExecuteEvent += _iconGallery_ExecuteEvent;
            LaunchIconsDialog.ExecuteEvent += _launchIconsDialog_ExecuteEvent;
            RemoveLastIcon.ExecuteEvent += _removeLastIcon_ExecuteEvent;
            RemoveAllIcons.ExecuteEvent += _removeAllIcons_ExecuteEvent;

            //register for change events
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

        private void Close_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CloseCurrentMap();
        }

        #region Home Tab

        private void NewChildNode_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendChildNodeAndEdit();
        }

        private void NewLongNode_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendMultiLineNodeAndEdit();
        }

        private void NewNodeAbove_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendSiblingAboveAndEdit();
        }

        private void NewNodeBelow_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendSiblingNodeAndEdit();
        }

        private void NewParent_ExecuteEvent(object sender, ExecuteEventArgs e)
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
            if (FormatPainter.BooleanValue)
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
                
                //get property name
                //Debug.WriteLine(RibbonProperties.GetPropertyKeyName(ref propertyKey));

                if (propertyKey == RibbonProperties.FontProperties_Bold)
                {
                    mainCtrl.CurrentMapCtrl.ToggleSelectedNodeBold();
                }
                else if (propertyKey == RibbonProperties.FontProperties_Italic)
                {
                    mainCtrl.CurrentMapCtrl.ToggleSelectedNodeItalic();
                }
                else if (propertyKey == RibbonProperties.FontProperties_Strikethrough)
                {
                    mainCtrl.CurrentMapCtrl.ToggleSelectedNodeStrikeout();
                }
                else if (propertyKey == RibbonProperties.FontProperties_Family)
                {
                    mainCtrl.CurrentMapCtrl.SetFontFamily(RichFont.Family);
                }
                else if (propertyKey == RibbonProperties.FontProperties_Size)
                {
                    mainCtrl.CurrentMapCtrl.SetFontSize((float)RichFont.Size);
                }
                else if (propertyKey == RibbonProperties.FontProperties_BackgroundColor)
                {
                    mainCtrl.CurrentMapCtrl.ChangeBackColor(RichFont.BackgroundColor);
                }
                else if (propertyKey == RibbonProperties.FontProperties_BackgroundColorType)
                {
                    mainCtrl.CurrentMapCtrl.ChangeBackColor(Color.Empty);
                }
                else if (propertyKey == RibbonProperties.FontProperties_ForegroundColor)
                {
                    mainCtrl.CurrentMapCtrl.ChangeTextColor(RichFont.ForegroundColor);
                }
                else if (propertyKey == RibbonProperties.FontProperties_ForegroundColorType)
                {
                    mainCtrl.CurrentMapCtrl.ChangeTextColor(Color.Empty);
                }
            }
        }

        private void _iconGallery_ItemsSourceReady(object sender, EventArgs e)
        {
            IUICollection itemsSource = IconsGallery.ItemsSource;
            itemsSource.Clear();

            foreach (ModelIcon icon in MetaModel.MetaModel.Instance.IconsList)
            {
                itemsSource.Add(new GalleryIconPropertySet(icon, ribbon));
            }

            RemoveLastIcon.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.minus);
            RemoveAllIcons.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.cross_script);
            LaunchIconsDialog.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.smartart_change_color_gallery_16);
        }

        private void _iconGallery_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            GalleryIconPropertySet iconPropertySet = (GalleryIconPropertySet) e.CommandExecutionProperties;
            if (iconPropertySet != null)
            {
                mainCtrl.CurrentMapCtrl.AppendIcon(iconPropertySet.Name);
            }
        }

        private void _launchIconsDialog_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AppendIconFromIconSelectorExt();
        }

        private void _removeLastIcon_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.RemoveLastIcon();
        }

        private void _removeAllIcons_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.RemoveAllIcon();
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
            RichFont.Bold = n.Bold ? FontProperties.Set : FontProperties.NotSet;
            RichFont.Italic = n.Italic ? FontProperties.Set : FontProperties.NotSet;
            RichFont.Strikethrough = n.Strikeout ? FontProperties.Set : FontProperties.NotSet;
            RichFont.Family = n.NodeView.Font.Name;
            RichFont.Size = (decimal)n.NodeView.Font.Size;
        }

        private void ClearFontControl()
        {
            RichFont.Bold = FontProperties.NotSet;
            RichFont.Italic = FontProperties.NotSet;
            RichFont.Strikethrough = FontProperties.NotSet;
            RichFont.Family = null;
            RichFont.Size = 0;
        }

        private void ClipboardManager_StatusChanged()
        {
            if (ClipboardManager.HasCutNode)
            {
                Cut.SmallImage = ribbon.ConvertToUIImage(Win7.Properties.Resources.cut_red_small);
            }
            else
            {
                Cut.SmallImage = ribbon.ConvertToUIImage(Win7.Properties.Resources.cut_small);
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
                FormatPainter.BooleanValue = false;
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
                    FormatPainter.BooleanValue = false;
                    break;
                case MapControls.FormatPainterStatus.SingleApply:
                case MapControls.FormatPainterStatus.MultiApply:
                    FormatPainter.BooleanValue = true;
                    break;
            }
        }

        #endregion

    }
}

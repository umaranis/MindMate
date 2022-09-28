using MindMate.Model;
using RibbonLib.Controls.Events;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RibbonLib.Interop;
using System.Drawing;
using System.Drawing.Drawing2D;
using MindMate.MetaModel;
using MindMate.Plugins;
using MindMate.View.EditorTabs;
using MindMate.Win7.Properties;
using RibbonLib;

namespace MindMate.View.Ribbon
{
    public partial class Ribbon
    {
        private readonly Controller.MainCtrl mainCtrl;
        private readonly RibbonLib.Ribbon ribbon;
        private readonly IMainForm mainForm;

        public Ribbon(RibbonLib.Ribbon ribbon, Controller.MainCtrl mainCtrl, IMainForm mainForm)
        {
            this.ribbon = ribbon;
            this.mainCtrl = mainCtrl;
            this.mainForm = mainForm;

            InitializeComponents();

            HelpButton.ExecuteEvent += HelpButton_ExecuteEvent;

            //Application Menu
            ApplicationMenu.TooltipTitle = "Menu";
            ApplicationMenu.TooltipDescription = "Application main menu";

            ButtonNew.ExecuteEvent += _buttonNew_ExecuteEvent;
            ButtonExit.ExecuteEvent += _buttonExit_ExecuteEvent;
            ButtonOpen.ExecuteEvent += _buttonOpen_ExecuteEvent;
            ButtonSave.ExecuteEvent += _buttonSave_ExecuteEvent;
            SaveAs.ExecuteEvent += SaveAs_ExecuteEvent;
            SaveAll.ExecuteEvent += SaveAll_ExecuteEvent;
            ExportAsPNG.ExecuteEvent += ExportAsPNG_ExecuteEvent;
            ExportAsJPG.ExecuteEvent += ExportAsJPG_ExecuteEvent;
            ExportAsHTML.ExecuteEvent += (o, e) => mainCtrl.ExportAsHtml();
            Close.ExecuteEvent += Close_ExecuteEvent;

            RecentItems.RecentItems = CreateRecentItemsList();
            RecentItems.ExecuteEvent += RecentItems_ExecuteEvent;

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
            PasteAsImage.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.Paste(false, true);
            Cut.ExecuteEvent += _btnCut_ExecuteEvent;
            Copy.ExecuteEvent += _btnCopy_ExecuteEvent;
            FormatPainter.ExecuteEvent += _btnFormatPainter_ExecuteEvent;

            //Home Tab: Font group
            RichFont.ExecuteEvent += _RichFont_ExecuteEvent;

            //Home Tab: Format Group
            BackColor.ExecuteEvent += BackColor_ExecuteEvent;
            ClearFormatting.ExecuteEvent += ClearFormatting_ExecuteEvent;
            
            //Home Tab: Icons Group
            IconsGallery.ItemsSourceReady += _iconGallery_ItemsSourceReady;
            IconsGallery.ExecuteEvent += _iconGallery_ExecuteEvent;
            LaunchIconsDialog.ExecuteEvent += _launchIconsDialog_ExecuteEvent;
            RemoveLastIcon.ExecuteEvent += _removeLastIcon_ExecuteEvent;
            RemoveAllIcons.ExecuteEvent += _removeAllIcons_ExecuteEvent;

            //Edit Tab: Select Group
            SelectAll.ExecuteEvent += SelectAll_ExecuteEvent;
            SelectSiblings.ExecuteEvent += SelectSiblings_ExecuteEvent;
            SelectAncestors.ExecuteEvent += SelectAncestors_ExecuteEvent;
            SelectChildren.ExecuteEvent += SelectChildren_ExecuteEvent;
            SelectDescendents.ExecuteEvent += SelectDescendents_ExecuteEvent;
            SelectDescendentsUpto1.ExecuteEvent += SelectDescendentsUpto1_ExecuteEvent;
            SelectDescendentsUpto2.ExecuteEvent += SelectDescendentsUpto2_ExecuteEvent;
            SelectDescendentsUpto3.ExecuteEvent += SelectDescendentsUpto3_ExecuteEvent;
            SelectDescendentsUpto4.ExecuteEvent += SelectDescendentsUpto4_ExecuteEvent;
            SelectDescendentsUpto5.ExecuteEvent += SelectDescendentsUpto5_ExecuteEvent;
            SelectLevel1.ExecuteEvent += SelectLevel1_ExecuteEvent;
            SelectLevel2.ExecuteEvent += SelectLevel2_ExecuteEvent;
            SelectLevel3.ExecuteEvent += SelectLevel3_ExecuteEvent;
            SelectLevel4.ExecuteEvent += SelectLevel4_ExecuteEvent;
            SelectLevel5.ExecuteEvent += SelectLevel5_ExecuteEvent;
            SelectLevelCurrent.ExecuteEvent += SelectLevelCurrent_ExecuteEvent;

            //Edit Tab: Expand / Collapse Group
            ExpandAll.ExecuteEvent += ExpandAll_ExecuteEvent;
            CollapseAll.ExecuteEvent += CollapseAll_ExecuteEvent;
            ToggleCurrent.ExecuteEvent += ToggleCurrent_ExecuteEvent;
            ToggleBranch.ExecuteEvent += ToggleBranch_ExecuteEvent;
            ExpandMapToCurrentLevel.ExecuteEvent += ExpandMapToCurrentLevel_ExecuteEvent;
            ExpandMapToLevel1.ExecuteEvent += ExpandMapToLevel1_ExecuteEvent;
            ExpandMapToLevel2.ExecuteEvent += ExpandMapToLevel2_ExecuteEvent;
            ExpandMapToLevel3.ExecuteEvent += ExpandMapToLevel3_ExecuteEvent;
            ExpandMapToLevel4.ExecuteEvent += ExpandMapToLevel4_ExecuteEvent;
            ExpandMapToLevel5.ExecuteEvent += ExpandMapToLevel5_ExecuteEvent;

            //Edit Tab: Navigate Group
            NavigateToCenter.ExecuteEvent += NavigateToCenter_ExecuteEvent;
            NavigateToFirstSibling.ExecuteEvent += NavigateToFirstSibling_ExecuteEvent;
            NavigateToLastSibling.ExecuteEvent += NavigateToLastSibling_ExecuteEvent;

            //Edit Tab: Move
            MoveUp.ExecuteEvent += MoveUp_ExecuteEvent;
            MoveDown.ExecuteEvent += MoveDown_ExecuteEvent;

            //Edit Tab: Sort
            SortAlphabetic.ExecuteEvent += SortAlphabetic_ExecuteEvent;
            SortDueDate.ExecuteEvent += SortDueDate_ExecuteEvent;
            SortNodeCount.ExecuteEvent += SortNodeCount_ExecuteEvent;
            SortModifiedDate.ExecuteEvent += SortModifiedDate_ExecuteEvent;
            SortCreateDate.ExecuteEvent += SortCreateDate_ExecuteEvent;
            SortOrder.ExecuteEvent += SortOrder_ExecuteEvent;

            SortOrder.BooleanValue = true;

            //Edit Tab: Undo / Redo
            Undo.ExecuteEvent += Undo_ExecuteEvent;
            Redo.ExecuteEvent += Redo_ExecuteEvent;

            //Insert Tab: Hyperlink
            Hyperlink.ExecuteEvent += Hyperlink_ExecuteEvent;
            HyperlinkFile.ExecuteEvent += HyperlinkFile_ExecuteEvent;
            HyperlinkFolder.ExecuteEvent += HyperlinkFolder_ExecuteEvent;
            RemoveHyperlink.ExecuteEvent += RemoveHyperlink_ExecuteEvent;

            //Insert Tab: Note
            InsertNote.ExecuteEvent += InsertNote_ExecuteEvent;

            //Insert Tab: Image
            InsertImage.ExecuteEvent += (o,e) => mainCtrl.InsertImage();

            //Format Tab: Node Format
            NodeShape.ItemsSourceReady += NodeShape_ItemsSourceReady;
            NodeShape.ExecuteEvent += NodeShape_ExecuteEvent;
            ClearShapeFormat.ExecuteEvent += ClearShapeFormat_ExecuteEvent;
            LineColor.ExecuteEvent += LineColor_ExecuteEvent;
            LinePatternSolid.ExecuteEvent += LinePatternSolid_ExecuteEvent;
            LinePatternDashed.ExecuteEvent += LinePatternDashed_ExecuteEvent;
            LinePatternDotted.ExecuteEvent += LinePatternDotted_ExecuteEvent;
            LinePatternMixed.ExecuteEvent += LinePatternMixed_ExecuteEvent;
            LineThickness1.ExecuteEvent += LineThickness1_ExecuteEvent;
            LineThickness2.ExecuteEvent += LineThickness2_ExecuteEvent;
            LineThickness4.ExecuteEvent += LineThickness4_ExecuteEvent;
            NodeStyleGallery.ItemsSourceReady += NodeStyleGallery_ItemsSourceReady;
            NodeStyleGallery.ExecuteEvent += NodeStyleGallery_ExecuteEvent;

            //Format Tab: Node Style
            CreateNodeStyle.ExecuteEvent += CreateNodeStyle_ExecuteEvent;

            //Format Tab: Default Format
            DefaultFormatSettings.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.SetDefaultFormatDialog();
            SetSelectedAsDefaultFormat.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.SetSelectedNodeFormatAsDefault();
            ApplyTheme.ItemsSourceReady += ApplyTheme_ItemsSourceReady;
            ApplyTheme.ExecuteEvent += ApplyTheme_ExecuteEvent;

            //View Tab: View Tasks
            ViewTaskList.ExecuteEvent += ViewTaskList_ExecuteEvent;

            //View Tab: View Note
            ViewNote.ExecuteEvent += ViewNote_ExecuteEvent;

            //Image Tab
            RemoveImage.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.RemoveImage();
            IncreaseImageSize.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.IncreaseImageSize();
            DecreaseImageSize.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.DecreaseImageSize();
            ImageAlignStart.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.ImageAlignStart();
            ImageAlignCenter.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.ImageAlignCenter();
            ImageAlignEnd.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.ImageAlignEnd();
            ImagePosAbove.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.ImagePosAbove();
            ImagePosBelow.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.ImagePosBelow();
            ImagePosBefore.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.ImagePosBefore();
            ImagePosAfter.ExecuteEvent += (o, e) => mainCtrl.CurrentMapCtrl.ImagePosAfter();

            //Note Editor Tab: Paragraph
            Bullets.ExecuteEvent += (o, e) => mainForm.NoteEditor.AddBullets();
            Numbers.ExecuteEvent += (o, e) => mainForm.NoteEditor.AddNumbering();
            Indent.ExecuteEvent += (o, e) => mainForm.NoteEditor.IndentSelection();
            Outdent.ExecuteEvent += (o, e) => mainForm.NoteEditor.OutdentSelection();
            AlignLeft.ExecuteEvent += (o, e) => mainForm.NoteEditor.AlignSelectionLeft();
            AlignRight.ExecuteEvent += (o, e) => mainForm.NoteEditor.AlignSelectionRight();
            AlignCenter.ExecuteEvent += (o, e) => mainForm.NoteEditor.AlignSelectionCenter();
            Justify.ExecuteEvent += (o, e) => mainForm.NoteEditor.AlignSelectionFull();

            //Note Editor Tab: Note Styles
            NoteHeading1.ExecuteEvent += (o, e) => mainForm.NoteEditor.ApplyHeading1();
            NoteHeading2.ExecuteEvent += (o, e) => mainForm.NoteEditor.ApplyHeading2();
            NoteHeading3.ExecuteEvent += (o, e) => mainForm.NoteEditor.ApplyHeading3();
            NoteNormal.ExecuteEvent += (o, e) => mainForm.NoteEditor.ApplyNormalStyle();

            //Note Editor Tab: Table
            NoteInsertTable.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.InsertTable();

			//Note Editor Tab: Image
			NoteInsertImage.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.InsertImage();

            //Note Editor Tab: Html Code
            NoteCleanHtml.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.CleanHtmlCode();
            NoteEditHtml.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.ShowHtmlSourceDialog();


            //Note Editor Table Tab
            ModifyTableProperties.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.ModifyTable();
            DeleteRow.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.DeleteTableRow();
            DeleteColumn.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.DeleteTableColumn();
            DeleteTable.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.DeleteTable();
            InsertRowAbove.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.InsertTableRowAbove();
            InsertRowBelow.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.InsertTableRowBelow();
            InsertColumnLeft.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.InsertTableColumnLeft();
            InsertColumnRight.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.InsertTableColumnRight();
            MoveRowUp.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.MoveTableRowUp();
            MoveRowDown.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.MoveTableRowDown();
            MoveColumnLeft.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.MoveTableColumnLeft();
            MoveColumnRight.ExecuteEvent += (o, e) => mainCtrl.NoteCrtl.MoveTableColumnRight();

            //register for change events
            mainCtrl.PersistenceManager.CurrentTreeChanged += PersistenceManager_CurrentTreeChanged;
            MindMate.Model.ClipboardManager.StatusChanged += ClipboardManager_StatusChanged;
            mainForm.EditorTabs.ControlAdded += Tabs_ControlAdded;
            mainForm.EditorTabs.ControlRemoved += Tabs_ControlRemoved;
            mainForm.EditorTabs.SelectedIndexChanged += Tabs_SelectedIndexChanged;
            mainForm.NoteEditor.CursorMoved += NoteEditor_CursorMoved;       
        }

        /// <summary>
        /// Setting certain properties (like image) doesn't work if Ribbon is not loaded. Use this method to set such properties.
        /// </summary>
        public void OnRibbonLoaded()
        {
            ExportAsHTML.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.icons8_html_filetype_32);
            ExportAsHTML.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.icons8_html_filetype_16);
			InsertImage.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Add_Image_32);
			InsertImage.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Add_Image_16);
            RemoveImage.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Remove_Image_32);
            RemoveImage.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Remove_Image_16);
            IncreaseImageSize.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.resize_8_32);
            IncreaseImageSize.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.resize_8_16);
            DecreaseImageSize.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.resize_9_32);
            DecreaseImageSize.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.resize_9_32);
            ImageAlignStart.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.image_align_start);
            ImageAlignCenter.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.image_align_center);
            ImageAlignEnd.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.image_align_end);
            ImagePosAbove.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.image_align_center);
            ImagePosBelow.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.image_align_bottom);
            ImagePosBefore.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.image_align_before);
            ImagePosAfter.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.image_align_after);

            NodeShape.LargeImage = ribbon.ConvertToUIImage(Resources.Node_Format_Bubble);
            Bullets.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Bullets_SmallImage);
            Numbers.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Numbers_SmallImage);
            Outdent.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Outdent_SmallImage);
            Indent.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Indent_SmallImage);
            AlignLeft.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.AlignLeft_SmallImage);
            AlignCenter.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.AlignCenter_SmallImage);
            AlignRight.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.AlignRight_SmallImage);
            Justify.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Justify_SmallImage);
            NoteHeading1.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteHeading1_32bit);
            NoteHeading1.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteHeading1_16bit);
            NoteHeading2.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteHeading2_32bit);
            NoteHeading2.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteHeading2_16bit);
            NoteHeading3.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteHeading3_32bit);
            NoteHeading3.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteHeading3_16bit);
            NoteNormal.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteNormal_32bit);
            NoteNormal.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteNormal_16bit);
            NoteInsertTable.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertTable_SmallImage);
            NoteInsertTable.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertTable_LargeImage);
			NoteInsertImage.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Add_Image_16);
			NoteInsertImage.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.Add_Image_32);
			NoteEditHtml.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteEditHtml_16bit);
            NoteEditHtml.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.NoteEditHtml_32bit);
            ModifyTableProperties.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.TableProperties_SmallImage);
            ModifyTableProperties.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.TableProperties_LargeImage);
            DeleteRow.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.DeleteRow_SmallImage);
            DeleteRow.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.DeleteTable_LargeImage);
            DeleteColumn.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.DeleteColumn_SmallImage);
            DeleteColumn.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.DeleteColumn_LargeImage);
            DeleteTable.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.DeleteTable_SmallImage);
            DeleteTable.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.DeleteTable_LargeImage);
            InsertRowAbove.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertRowAbove_SmallImage);
            InsertRowAbove.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertRowAbove_LargeImage);
            InsertRowBelow.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertRowBelow_SmallImage);
            InsertRowBelow.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertRowBelow_LargeImage);
            InsertColumnLeft.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertColumnLeft_SmallImage);
            InsertColumnLeft.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertColumnLeft_LargeImage);
            InsertColumnRight.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertColumnRight_SmallImage);
            InsertColumnRight.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.InsertColumnRight_LargeImage);
            MoveRowUp.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.MoveRowUp_SmallImage);
            MoveRowUp.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.MoveRowUp_LargeImage);
            MoveRowDown.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.MoveRowDown_SmallImage);
            MoveRowDown.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.MoveRowDown_LargeImage);
            MoveColumnLeft.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.MoveColumnLeft_SmallImage);
            MoveColumnLeft.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.MoveColumnLeft_LargeImage);
            MoveColumnRight.SmallImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.MoveColumnRight_SmallImage);
            MoveColumnRight.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.MoveColumnRight_LargeImage);
            NoteCleanHtml.LargeImage = ribbon.ConvertToUIImage(MindMate.Properties.Resources.html_clean_32);
				

			mainForm.FocusedControlChanged += MainForm_FocusedControlChanged;
        }
        
        //TODO: This will not work if there are more plugins with MainMenu (fix it)
        public void SetupPluginCommands(MainMenuItem[] pluginItems)
        {
            MainMenuItem mTask = pluginItems[0];

            var handlerAddTask = mTask.DropDownItems[0].Click;
            AddTask.ExecuteEvent += (sender, args) => handlerAddTask(sender, args);

            var handlerAddTaskToday = mTask.DropDownItems[1].Click;
            AddTaskToday.ExecuteEvent += (sender, args) => handlerAddTaskToday(sender, args);

            var handlerAddTaskTomorrow = mTask.DropDownItems[2].Click;
            AddTaskTomorrow.ExecuteEvent += (sender, args) => handlerAddTaskTomorrow(sender, args);

            var handlerAddTaskNextWeek = mTask.DropDownItems[3].Click;
            AddTaskNextWeek.ExecuteEvent += (sender, args) => handlerAddTaskNextWeek(sender, args);

            var handlerAddTaskNextMonth = mTask.DropDownItems[4].Click;
            AddTaskNextMonth.ExecuteEvent += (sender, args) => handlerAddTaskNextMonth(sender, args);

            var handlerAddTaskNextQuarter = mTask.DropDownItems[5].Click;
            AddTaskNextQuarter.ExecuteEvent += (sender, args) => handlerAddTaskNextQuarter(sender, args);

            var handlerCompleteTask = mTask.DropDownItems[6].Click;
            CompleteTask.ExecuteEvent += (sender, args) => handlerCompleteTask(sender, args);

            var handlerRemoveTask = mTask.DropDownItems[7].Click;
            RemoveTask.ExecuteEvent += (sender, args) => handlerRemoveTask(sender, args);

            var handlerViewCalendar = mTask.DropDownItems[8].Click;
            ViewCalendar.ExecuteEvent += (sender, args) => handlerViewCalendar(sender, args);

        }

        #region Application Menu

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

        private void _buttonSave_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.SaveCurrentMap();
        }

        private void SaveAs_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.SaveCurrentMapAs();
        }

        private void SaveAll_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.SaveAll();
        }

        private void ExportAsPNG_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.ExportAsPng();
        }

        private void ExportAsJPG_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.ExportAsJpg();
        }

        private void Close_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CloseCurrentMap();
        }

        private List<RecentItemsPropertySet> CreateRecentItemsList()
        {
            var recentItems = new List<RecentItemsPropertySet>(MetaModel.MetaModel.RecentFilesCount);
            for (int i = 0; i < MetaModel.MetaModel.Instance.RecentFiles.Count; i++)
            {
                string recentFile = MetaModel.MetaModel.Instance.RecentFiles[i];
                recentItems.Add(new RecentItemsPropertySet()
                {
                    Label = System.IO.Path.GetFileName(recentFile),
                    LabelDescription = recentFile,
                    Pinned = false
                });
            }
            return recentItems;
        }

        public void RefreshRecentItemsList()
        {
            var ribbonRecentItems = RecentItems.RecentItems;
            var recentFiles = MetaModel.MetaModel.Instance.RecentFiles;

            // start: make two list same in size
            int countDiff = recentFiles.Count - ribbonRecentItems.Count;
            if (countDiff != 0)
            {
                if (countDiff > 0)
                {
                    do
                    {
                        ribbonRecentItems.Add(new RecentItemsPropertySet());
                        countDiff--;
                    } while (countDiff != 0);
                }
                else
                {
                    do
                    {
                        ribbonRecentItems.RemoveAt(ribbonRecentItems.Count - 1);
                        countDiff++;
                    } while (countDiff != 0);
                }
            }
            // end: make two list same in size

            // refresh ribbon recent items list
            for (int i = 0; i < recentFiles.Count; i++)
            {
                string fileName = recentFiles[i];
                ribbonRecentItems[i].Label = System.IO.Path.GetFileName(fileName);
                ribbonRecentItems[i].LabelDescription = fileName;
                ribbonRecentItems[i].Pinned = false;
            }
        }

        private void RecentItems_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            if (e.Key.PropertyKey == RibbonProperties.SelectedItem)
            {
                // get selected item label description
                PropVariant propLabelDescription;
                e.CommandExecutionProperties.GetValue(ref RibbonProperties.LabelDescription,
                                                    out propLabelDescription);
                string labelDescription = (string)propLabelDescription.Value;
                
                // open file
                mainCtrl.OpenMap(labelDescription);
            }
        }

        private void HelpButton_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.ShowAboutBox();
        }

        #endregion

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
                    mainCtrl.Bold(RichFont.Bold == FontProperties.Set);
                }
                else if (propertyKey == RibbonProperties.FontProperties_Italic)
                {
                    mainCtrl.Italic(RichFont.Italic == FontProperties.Set);
                }
                else if(propertyKey == RibbonProperties.FontProperties_Underline)
                {
                    mainCtrl.Underline(RichFont.Underline == FontUnderline.Set);
                }
                else if (propertyKey == RibbonProperties.FontProperties_Strikethrough)
                {
                    mainCtrl.Strikethrough(RichFont.Strikethrough == FontProperties.Set);
                }
                else if(propertyKey == RibbonProperties.FontProperties_VerticalPositioning)
                {
                    switch(RichFont.VerticalPositioning)
                    {
                        case FontVerticalPosition.SubScript:
                            mainCtrl.Subscript();
                            break;
                        case FontVerticalPosition.SuperScript:
                            mainCtrl.Superscript();
                            break;
                    }
                }
                else if (propertyKey == RibbonProperties.FontProperties_Family)
                {
                    mainCtrl.SetFontFamily(RichFont.Family);
                }
                else if (propertyKey == RibbonProperties.FontProperties_Size)
                {
                    mainCtrl.SetFontSize((float)RichFont.Size);
                }
                else if (propertyKey == RibbonProperties.FontProperties_BackgroundColor)
                {
                    mainCtrl.SetBackColor(RichFont.BackgroundColor);
                }
                else if (propertyKey == RibbonProperties.FontProperties_BackgroundColorType)
                {
                    mainCtrl.SetBackColor(Color.Empty);
                }
                else if (propertyKey == RibbonProperties.FontProperties_ForegroundColor)
                {
                    mainCtrl.SetForeColor(RichFont.ForegroundColor);
                }
                else if (propertyKey == RibbonProperties.FontProperties_ForegroundColorType)
                {
                    mainCtrl.SetForeColor(Color.Empty);
                }
            }
        }

        private void BackColor_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            switch ((uint) e.CurrentValue.PropVariant.Value)
            {
                case 0: //No Color
                    mainCtrl.SetBackColor(Color.Empty);
                    break;
                case 1: //Automatic Color
                    break;
                case 2: //Color
                    mainCtrl.SetBackColor(BackColor.Color);
                    break;
            }
        }

        private void ClearFormatting_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.ClearSelectionFormatting();
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

        #region Edit Tab

        private void SelectAll_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectAllNodes(ExpandOnSelect.BooleanValue);
        }

        private void SelectSiblings_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectSiblings();
        }

        private void SelectAncestors_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectAncestors();
        }

        private void SelectChildren_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectChildren(true);
        }

        private void SelectDescendents_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectDescendents(ExpandOnSelect.BooleanValue);
        }

        private void SelectDescendentsUpto1_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectDescendents(1, ExpandOnSelect.BooleanValue);
        }

        private void SelectDescendentsUpto2_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectDescendents(2, ExpandOnSelect.BooleanValue);
        }

        private void SelectDescendentsUpto3_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectDescendents(3, ExpandOnSelect.BooleanValue);
        }

        private void SelectDescendentsUpto4_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectDescendents(4, ExpandOnSelect.BooleanValue);
        }

        private void SelectDescendentsUpto5_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectDescendents(5, ExpandOnSelect.BooleanValue);
        }

        private void SelectLevel1_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectLevel(1, false, ExpandOnSelect.BooleanValue);
        }

        private void SelectLevel2_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectLevel(2, false, ExpandOnSelect.BooleanValue);
        }

        private void SelectLevel3_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectLevel(3, false, ExpandOnSelect.BooleanValue);
        }

        private void SelectLevel4_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectLevel(4, false, ExpandOnSelect.BooleanValue);
        }

        private void SelectLevel5_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectLevel(5, false, ExpandOnSelect.BooleanValue);
        }

        private void SelectLevelCurrent_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectCurrentLevel(ExpandOnSelect.BooleanValue);
        }

        private void ExpandAll_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.UnfoldAll();
        }

        private void CollapseAll_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.FoldAll();
        }

        private void ToggleCurrent_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ToggleFolded();
        }

        private void ToggleBranch_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ToggleBranchFolding();
        }

        private void ExpandMapToCurrentLevel_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.UnfoldMapToCurrentLevel();
        }

        private void ExpandMapToLevel1_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.UnfoldMapToLevel(1);
        }

        private void ExpandMapToLevel2_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.UnfoldMapToLevel(2);
        }

        private void ExpandMapToLevel3_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.UnfoldMapToLevel(3);
        }

        private void ExpandMapToLevel4_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.UnfoldMapToLevel(4);
        }

        private void ExpandMapToLevel5_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.UnfoldMapToLevel(5);
        }

        private void NavigateToCenter_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectRootNode();
        }

        private void NavigateToFirstSibling_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectTopSibling();
        }

        private void NavigateToLastSibling_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.SelectBottomSibling();
        }

        private void MoveUp_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.MoveNodeUp();
        }

        private void MoveDown_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.MoveNodeDown();
        }

        private const string AscendingOrderString = "Ascending Order";
        private const string DescendingOrderString = "Descending Order";

        private bool IsAscendingSortOrder { get { return SortOrder.Label == AscendingOrderString || SortOrder.Label == null; }}

        private void SortAlphabetic_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            if (IsAscendingSortOrder)
            {
                mainCtrl.CurrentMapCtrl.SortAlphabeticallyAsc();
            }
            else
            {
                mainCtrl.CurrentMapCtrl.SortAlphabeticallyDesc();
            }
        }

        private void SortDueDate_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            if (IsAscendingSortOrder)
            {
                mainCtrl.CurrentMapCtrl.SortByTaskAsc();
            }
            else
            {
                mainCtrl.CurrentMapCtrl.SortByTaskDesc();
            }

        }

        private void SortNodeCount_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            if (IsAscendingSortOrder)
            {
                mainCtrl.CurrentMapCtrl.SortByDescendentsCountAsc();
            }
            else
            {
                mainCtrl.CurrentMapCtrl.SortByDescendentsCountDesc();
            }
        }

        private void SortModifiedDate_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            if (IsAscendingSortOrder)
            {
                mainCtrl.CurrentMapCtrl.SortByModifiedDateAsc();
            }
            else
            {
                mainCtrl.CurrentMapCtrl.SortByModifiedDateDesc();
            }
        }

        private void SortCreateDate_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            if (IsAscendingSortOrder)
            {
                mainCtrl.CurrentMapCtrl.SortByCreateDateAsc();
            }
            else
            {
                mainCtrl.CurrentMapCtrl.SortByCreateDateDesc();
            }
        }

        private void SortOrder_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            if (SortOrder.Label == null || SortOrder.Label.Equals(AscendingOrderString)) //ascending, swtich to descending
            {
                SortOrder.BooleanValue = true;
                SortOrder.Label = DescendingOrderString;
                SortOrder.TooltipTitle = "Sort Order: Descending";
                SortOrder.SmallImage = ribbon.ConvertToUIImage(Resources.Descending_Sorting_32);
            }
            else //switch to ascending
            {
                SortOrder.BooleanValue = true;
                SortOrder.Label = AscendingOrderString;
                SortOrder.TooltipTitle = "Sort Order: Ascending";
                SortOrder.SmallImage = ribbon.ConvertToUIImage(Resources.Ascending_Sorting_32);
            }
            
        }

        private void Undo_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.Undo();
        }

        private void Redo_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.Redo();
        }

        #endregion

        #region Insert Tab

        private void Hyperlink_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AddHyperlinkUsingTextbox();
        }

        private void HyperlinkFile_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AddHyperlinkUsingFileDialog();
        }

        private void HyperlinkFolder_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.AddHyperlinkUsingFolderDialog();
        }

        private void RemoveHyperlink_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.RemoveHyperlink();
        }

        private void InsertNote_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.StartNoteEditing();
        }

        #endregion

        #region Format Tab

        private void NodeShape_ItemsSourceReady(object sender, EventArgs e)
        {
            var itemSource = NodeShape.ItemsSource;
            itemSource.Clear();

            itemSource.Add(new GalleryItemPropertySet()
            {
                Label = "Fork",
                ItemImage = ribbon.ConvertToUIImage(Resources.Node_Format_Fork)
            });
            itemSource.Add(new GalleryItemPropertySet()
            {
                Label = "Bubble",
                ItemImage = ribbon.ConvertToUIImage(Resources.Node_Format_Bubble)
            });
            itemSource.Add(new GalleryItemPropertySet()
            {
                Label = "Box",
                ItemImage = ribbon.ConvertToUIImage(Resources.Node_Format_Box)
            });
            itemSource.Add(new GalleryItemPropertySet()
            {
                Label = "Bullet",
                ItemImage = ribbon.ConvertToUIImage(Resources.Node_Format_Bullet)
            });
        }

        private void NodeShape_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            var item = (GalleryItemPropertySet)e.CommandExecutionProperties;
            if (item != null)
            {
                mainCtrl.CurrentMapCtrl.ChangeNodeShape(item.Label);
            }
        }

        private void ClearShapeFormat_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ClearNodeShape();
        }

        private void LineColor_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            switch ((uint)e.CurrentValue.PropVariant.Value)
            {
                case 0: //No Color
                    mainCtrl.CurrentMapCtrl.ChangeLineColor(Color.Empty);
                    break;
                case 1: //Automatic Color
                    mainCtrl.CurrentMapCtrl.ChangeLineColor(Color.Empty);
                    break;
                case 2: //Color
                    mainCtrl.CurrentMapCtrl.ChangeLineColor(LineColor.Color);
                    break;
            }
        }

        private void LinePatternSolid_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ChangeLinePattern(DashStyle.Solid);
        }

        private void LinePatternDashed_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ChangeLinePattern(DashStyle.Dash);
        }

        private void LinePatternDotted_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ChangeLinePattern(DashStyle.Dot);
        }

        private void LinePatternMixed_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ChangeLinePattern(DashStyle.DashDotDot);
        }

        private void LineThickness1_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ChangeLineWidth(1);
        }

        private void LineThickness2_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ChangeLineWidth(2);
        }

        private void LineThickness4_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.CurrentMapCtrl.ChangeLineWidth(4);
        }

        private void NodeStyleGallery_ItemsSourceReady(object sender, EventArgs e)
        {
            foreach (var nodeStyle in MetaModel.MetaModel.Instance.NodeStyles)
            {
                NodeStyleGallery.ItemsSource.Add(new GalleryNodeStylePropertySet(nodeStyle, ribbon));
            }
        }

        private void CreateNodeStyle_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            var style = mainCtrl.CurrentMapCtrl.CreateNodeStyle();
            if (style != null)
            {
                NodeStyleGallery.SelectedItem = uint.MaxValue;//Remove the selection, otherwise Execute event is generated for the selected style on adding new style
                NodeStyleGallery.ItemsSource.Add(new GalleryNodeStylePropertySet(style, ribbon));
            }
        }

        private void NodeStyleGallery_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            var galleryStyle = (GalleryNodeStylePropertySet)e.CommandExecutionProperties;
            if (galleryStyle != null)
            {
                mainCtrl.CurrentMapCtrl.ApplyNodeStyle(galleryStyle.NodeStyle);
            }
        }

        private void ApplyTheme_ItemsSourceReady(object sender, EventArgs e)
        {
            var itemSource = ApplyTheme.ItemsSource;
            itemSource.Clear();

            foreach(var theme in MetaModel.MetaModel.Instance.Themes.Themes)
            {
                itemSource.Add(new GalleryItemPropertySet()
                {
                    Label = theme
                });
            }            
        }

        private void ApplyTheme_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            var theme = ((GalleryItemPropertySet)e.CommandExecutionProperties).Label;
            mainCtrl.CurrentMapCtrl.ApplyTheme(theme);
        }

        #endregion

        #region View Tab

        private void ViewNote_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.ViewNoteTab();
        }

        //TODO: Should be done same way as 'View Calender'
        private void ViewTaskList_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            mainCtrl.ViewTaskListTab();
        }

        #endregion

        #region Events to Refresh Command State

        private void PersistenceManager_CurrentTreeChanged(Serialization.PersistenceManager manager, Serialization.PersistentTree oldTree, Serialization.PersistentTree newTree)
        {
            if (oldTree != null)
            {
                oldTree.SelectedNodes.NodeSelected -= SelectedNodes_NodeSelected;
                oldTree.SelectedNodes.NodeDeselected -= SelectedNodes_NodeDeselected;
                oldTree.NodePropertyChanged -= Tree_NodePropertyChanged;
                //oldTree.IconChanged -= Tree_IconChanged;
                //oldTree.TreeStructureChanged -= Tree_TreeStructureChanged;
                //oldTree.AttributeChanged -= Tree_AttributeChanged;
                //oldTree.AttributeSpecChangeEvent -= Tree_AttributeSpecChangeEvent;

                newTree.ChangeManager.ChangeRecorded -= ChangeManager_ChangeRecorded;
            }

            if (newTree != null)
            {
                newTree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
                newTree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;
                newTree.NodePropertyChanged += Tree_NodePropertyChanged;
                //newTree.IconChanged += Tree_IconChanged;
                //newTree.TreeStructureChanged += Tree_TreeStructureChanged;
                //newTree.AttributeChanged += Tree_AttributeChanged;
                //newTree.AttributeSpecChangeEvent += Tree_AttributeSpecChangeEvent;

                newTree.ChangeManager.ChangeRecorded += ChangeManager_ChangeRecorded;

                UpdateFontControl(newTree.SelectedNodes);
                UpdateUndoGroup(newTree.ChangeManager);
            }
            else
            {
                ClearFontControl();
            }
            
        }

        private void ChangeManager_ChangeRecorded(Modules.Undo.ChangeManager changeManager, Modules.Undo.IChange change)
        {
            UpdateUndoGroup(changeManager);
        }

        private void Tree_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs e)
        {
            if (node.Tree == mainCtrl.PersistenceManager.CurrentTree)
            {
                if (e.ChangedProperty == NodeProperties.Bold || e.ChangedProperty == NodeProperties.Italic
                    || e.ChangedProperty == NodeProperties.Strikeout || e.ChangedProperty == NodeProperties.FontName
                    || e.ChangedProperty == NodeProperties.FontSize)
                {
                    UpdateFontControl(node.Tree.SelectedNodes);
                }
                UpdateUndoGroup(node.Tree.ChangeManager);
            }
        }

        //private void Tree_IconChanged(MapNode node, IconChangedEventArgs arg2)
        //{
        //    UpdateUndoGroup(node.Tree);
        //}

        //private void Tree_TreeStructureChanged(MapNode node, TreeStructureChangedEventArgs arg2)
        //{
        //    UpdateUndoGroup(node.Tree);
        //}

        //private void Tree_AttributeChanged(MapNode node, AttributeChangeEventArgs arg2)
        //{
        //    UpdateUndoGroup(node.Tree);
        //}

        //private void Tree_AttributeSpecChangeEvent(MapTree.AttributeSpec aSpec, MapTree.AttributeSpecEventArgs e)
        //{
        //    UpdateUndoGroup(aSpec.Tree);
        //}

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
            if (nodes.Count > 0)
            {
                MapNode n = nodes.First;
                RichFont.Bold = n.Bold ? FontProperties.Set : FontProperties.NotSet;
                RichFont.Italic = n.Italic ? FontProperties.Set : FontProperties.NotSet;
                RichFont.Strikethrough = n.Strikeout ? FontProperties.Set : FontProperties.NotSet;
                RichFont.Family = n.NodeView.NodeFormat.Font.Name;
                RichFont.Size = (decimal)n.NodeView.NodeFormat.Font.Size;
            }
            //else
            //{
            //    ClearFontControl();
            //}
        }

        private void ClearFontControl()
        {
            RichFont.Bold = FontProperties.NotSet;
            RichFont.Italic = FontProperties.NotSet;
            RichFont.Strikethrough = FontProperties.NotSet;
            RichFont.Family = null;
            RichFont.Size = 0;
        }

        private void UpdateUndoGroup(Modules.Undo.ChangeManager changeManager)
        {
            Undo.Enabled = changeManager.CanUndo;
            Redo.Enabled = changeManager.CanRedo;
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

        private void MainForm_FocusedControlChanged(Control gotFocus, Control lostFocus)
        {
            if(gotFocus == mainForm.NoteEditor)
            {
                TabGroupNote.ContextAvailable = ContextAvailability.Active;
            }
            else if(lostFocus == mainForm.NoteEditor)
            {
                TabGroupNote.ContextAvailable = ContextAvailability.NotAvailable;
                TabGroupNoteTable.ContextAvailable = ContextAvailability.NotAvailable;                
            }
        }

        private void NoteEditor_CursorMoved(object obj)
        {
            if(mainForm.NoteEditor.TableEditor.InsideTable())
            {
                TabGroupNoteTable.ContextAvailable = ContextAvailability.Available;
            }
            else
            {
                TabGroupNoteTable.ContextAvailable = ContextAvailability.NotAvailable;
            }
        }

        #endregion Events to Refresh Command State

    }
}

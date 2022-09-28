using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.MapControls;
using System.Windows.Forms;
using MindMate.MetaModel;
using MindMate.Controller;
using System.IO;
using XnaFan.ImageComparison;
using System.Drawing;
using System.Linq;
using MindMate.Tests.TestDouble;
using FakeItEasy;
using MindMate.View.Dialogs;

namespace MindMate.Tests.IntegrationTest
{
    [TestClass]
    public class MapCtrlTests
    {
        public const bool SAVE_ACTUAL_IMAGE = true;
        public const bool CONDUCT_INTERMEDIATE_TESTS = false;

        [TestMethod]
        public void MapCtrl_Test()
        {
            string xmlString = System.IO.File.ReadAllText(@"Resources\Feature Display.mm");

            MapTree tree = new MapTree();
            new MindMapSerializer().Deserialize(xmlString, tree);

            tree.SelectedNodes.Add(tree.RootNode, false);

            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MetaModel.MetaModel.Instance.MapEditorBackColor = Color.White;
            MetaModel.MetaModel.Instance.NoteEditorBackColor = Color.White;
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(dialogs).Where(call => call.Method.Name == "SeekDeleteConfirmation").WithReturnType<bool>().Returns(true);
            A.CallTo(dialogs).Where(call => call.Method.Name == "ShowColorPicker").WithReturnType<Color>().Returns(Color.Red);
            A.CallTo(dialogs).Where(call => call.Method.Name == "ShowFontDialog").WithReturnType<Font>().Returns(new System.Drawing.Font(System.Drawing.FontFamily.GenericSerif, 16));
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);

            tree.TurnOnChangeManager();

            // folding test
            mapCtrl.AppendNodeAndEdit();
            mapCtrl.MapView.NodeTextEditor.EndNodeEdit(true, true);
            mapCtrl.UpdateNodeText(tree.RootNode.LastChild, "Test Folding");
            mapCtrl.AppendChildNode(tree.RootNode.LastChild);
            mapCtrl.AppendChildNode(tree.RootNode.LastChild);
            mapCtrl.AppendChildNode(tree.RootNode.LastChild);
            mapCtrl.SelectNodeRightOrUnfold();
            mapCtrl.ToggleFolded();

            // delete test
            mapCtrl.SelectNodeAbove();
            mapCtrl.DeleteSelectedNodes();

            // move up
            mapCtrl.MoveNodeUp();

            // move right
            mapCtrl.SelectNodeBelow();
            for (int i = 0; i < 20; i++) mapCtrl.MoveNodeUp();

            //*****
            if (CONDUCT_INTERMEDIATE_TESTS) ImageTest(mapCtrl.MapView, "MapCtrl1");

            // move down     
            mapCtrl.SelectNodeRightOrUnfold();
            for (int i = 0; i < 5; i++) mapCtrl.SelectNodeAbove();
            for (int i = 0; i < 5; i++) mapCtrl.MoveNodeDown();

            // move up
            mapCtrl.SelectNodeAbove();
            for (int i = 0; i < 5; i++) mapCtrl.MoveNodeUp();

            // move left
            mapCtrl.SelectNodeLeftOrUnfold();
            for (int i = 0; i < 20; i++) mapCtrl.MoveNodeDown();

            // select siblings above
            mapCtrl.SelectNodeLeftOrUnfold();
            for (int i = 0; i < 3; i++) mapCtrl.SelectNodeBelow();
            mapCtrl.SelectAllSiblingsAbove();
            mapCtrl.ToggleBold();

            // select siblings below
            mapCtrl.SelectNodeRightOrUnfold();
            mapCtrl.SelectNodeLeftOrUnfold();
            for (int i = 0; i < 3; i++) mapCtrl.SelectNodeAbove();
            mapCtrl.SelectNodeBelow();
            mapCtrl.SelectAllSiblingsBelow();
            mapCtrl.ToggleItalic();

            //*****
            if (CONDUCT_INTERMEDIATE_TESTS) ImageTest(mapCtrl.MapView, "MapCtrl2");

            // add icon
            mapCtrl.AppendIcon("clock");
            mapCtrl.AppendIcon("idea");

            // remove last icon
            mapCtrl.RemoveLastIcon();

            // remove all icon
            mapCtrl.SelectNodeRightOrUnfold();
            mapCtrl.SelectNodeLeftOrUnfold();
            for (int i = 0; i < 3; i++) mapCtrl.SelectNodeBelow();
            mapCtrl.RemoveAllIcon();

            //*****
            if (CONDUCT_INTERMEDIATE_TESTS) ImageTest(mapCtrl.MapView, "MapCtrl3");

            mapCtrl.AppendNodeAndEdit();
            mapCtrl.MapView.NodeTextEditor.EndNodeEdit(true, true);
            mapCtrl.UpdateNodeText(tree.SelectedNodes.First, "Format Test");

            mapCtrl.ChangeLineColorUsingPicker();
            mapCtrl.ChangeLinePattern(System.Drawing.Drawing2D.DashStyle.Dash);
            mapCtrl.ChangeLineWidth(2);
            mapCtrl.ChangeFont();

            //*****
            if (CONDUCT_INTERMEDIATE_TESTS) ImageTest(mapCtrl.MapView, "MapCtrl4");


            mapCtrl.SelectNodeRightOrUnfold();
            mapCtrl.AppendSiblingNodeAndEdit();
            mapCtrl.MapView.NodeTextEditor.EndNodeEdit(true, true);
            mapCtrl.UpdateNodeText(tree.SelectedNodes.First, "Node Color");

            // change node color
            mapCtrl.AppendChildNode(tree.SelectedNodes.First);
            mapCtrl.UpdateNodeText(tree.SelectedNodes.First, "Node Color");
            mapCtrl.ChangeTextColorByPicker();

            // unfolding
            mapCtrl.SelectNodeRightOrUnfold();
            mapCtrl.ToggleFolded();
            mapCtrl.SelectNodeLeftOrUnfold();

            // change background color
            mapCtrl.AppendChildNodeAndEdit();
            mapCtrl.MapView.NodeTextEditor.EndNodeEdit(true, true);
            mapCtrl.UpdateNodeText(tree.SelectedNodes.First, "Background Color");
            mapCtrl.ChangeBackColorByPicker();
            mapCtrl.SelectNodeRightOrUnfold();

            //*****
            if (CONDUCT_INTERMEDIATE_TESTS) ImageTest(mapCtrl.MapView, "MapCtrl5");

            //select top/bottom sibling
            mapCtrl.SelectTopSibling();
            mapCtrl.AppendSiblingAboveAndEdit();
            mapCtrl.MapView.NodeTextEditor.EndNodeEdit(true, true);
            mapCtrl.UpdateNodeText(tree.SelectedNodes.First, "SelectTopSibling");
            mapCtrl.ChangeNodeShapeBubble();
            mapCtrl.SelectBottomSibling();

            //insert parent
            mapCtrl.InsertParentAndEdit();
            mapCtrl.EndNodeEdit();
            mapCtrl.UpdateNodeText(tree.SelectedNodes.First, "Parent Inserted");
            mapCtrl.ChangeNodeShapeBullet();

            //move nodes
            var n = mapCtrl.MapView.SelectedNodes.First;
            tree.RootNode.GetLastChild(NodePosition.Right).Selected = true;
            mapCtrl.SelectNodeAbove(true);
            mapCtrl.MapView.SelectedNodes.Last.ForEach(a => tree.SelectedNodes.Add(a, true));
            mapCtrl.MoveNodes(new DropLocation()
            {
                Parent = n,
                InsertAfterSibling = false
            });
            mapCtrl.SelectNodeBelow(true);

            //change node shape
            mapCtrl.ChangeNodeShapeBox();
            mapCtrl.MapView.SelectedNodes.Add(tree.SelectedNodes.First(a => a.Text == "Deep Hierarchy"));
            mapCtrl.ToggleFolded();
            mapCtrl.SelectNodeLeftOrUnfold();
            mapCtrl.SelectNodeLeftOrUnfold();
            mapCtrl.SelectNodeBelow(true);
            mapCtrl.SelectNodeAbove(true);
            mapCtrl.ChangeNodeShapeFork();

            ImageTest(mapCtrl.MapView, "MapCtrl6");

            VerifyUndoRedo(mapCtrl, "Feature Display");

            VerifySerializeDeserialize(mapCtrl);
            
            form.Dispose();
            //form.Close();                 

        }

        /// <summary>
        /// Confirming that MapView look doesn't change by serializing and deserializing
        /// </summary>
        /// <param name="mapCtrl"></param>
        private void VerifySerializeDeserialize(MapCtrl mapCtrl)
        {
            //1. save MapView as image
            mapCtrl.MapView.SelectedNodes.Add(mapCtrl.MapView.Tree.RootNode, false);
            using (var refImage = mapCtrl.MapView.DrawToBitmap())
            {

                //2. serialize
                var s = new MindMapSerializer();
                MemoryStream stream = new MemoryStream();
                s.Serialize(stream, mapCtrl.MapView.Tree);
                stream.Position = 0;
                string generatedText = new StreamReader(stream).ReadToEnd();
                stream.Close();

                //3. deserialize
                MapTree newTree = new MapTree();
                s.Deserialize(generatedText, newTree);
                newTree.SelectedNodes.Add(newTree.RootNode, false);
                DialogManager dialogs = A.Fake<DialogManager>();
                A.CallTo(dialogs).Where(call => call.Method.Name == "SeekDeleteConfirmation").WithReturnType<bool>().Returns(true);
                MapCtrl mapCtrl2 = new MapCtrl(new MapView(newTree), dialogs, null);

                //4. save new MapView image and compare
                using (var image = mapCtrl2.MapView.DrawToBitmap())
                {
                    if (SAVE_ACTUAL_IMAGE) refImage.Save(@"Resources\MapCtrl_BeforeSerialization.png");
                    if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\MapCtrl_AfterDeseriallization.png");
                    Assert.AreEqual(0.0f, refImage.PercentageDifference(image, 0),
                        "MapCtrl Test: Final image doesn't match.");

                }
            }
        }

        private void VerifyUndoRedo(MapCtrl mapCtrl, string originalFile)
        {
            mapCtrl.MapView.Tree.SelectedNodes.Clear();
            using (var refRedoImage = mapCtrl.MapView.DrawToBitmap())
            {
                while (mapCtrl.MapView.Tree.ChangeManager.CanUndo)
                {
                    mapCtrl.MapView.Tree.ChangeManager.Undo();
                }

                ImageTest(mapCtrl.MapView, "Feature Display");

                while (mapCtrl.MapView.Tree.ChangeManager.CanRedo)
                {
                    mapCtrl.MapView.Tree.ChangeManager.Redo();
                }

                using (var image = mapCtrl.MapView.DrawToBitmap())
                {
                    Assert.AreEqual(0.0f, refRedoImage.PercentageDifference(image, 0));
                }
            }
        }

        private void ImageTest(MapView view, string imageName)
        {
            using (var image = view.DrawToBitmap())
            {
                if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\" + imageName + " - Actual.png");
                using (var refImage = (Bitmap)Bitmap.FromFile(@"Resources\" + imageName + ".png"))
                {
                    Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "MapCtrlTest failed for image:" + imageName + ".png");
                }
            }
        }
    }
}

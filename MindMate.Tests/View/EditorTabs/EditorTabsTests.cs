using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using MindMate.View.EditorTabs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View
{
    [TestClass()]
    public class EditorTabsTests
    {
        [TestMethod()]
        public void EditorTabs()
        {
            var sut = new EditorTabs();
        }

        [TestMethod()]
        public void OpenTab_MapView()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new EditorTabs();
            Form f = CreateForm();
            f.Controls.Add(sut);
            f.Show();
            PersistenceManager pManager = new PersistenceManager();
            Tab tab = sut.OpenTab(pManager.NewTree());
            Assert.IsNotNull(tab.MapView);
        }

        [TestMethod()]
        public void OpenTab_NonMapView()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new EditorTabs();
            Form f = CreateForm();
            f.Controls.Add(sut);
            f.Show();
            TabPage tab = sut.OpenTab(new Control(), "Hello");
            Assert.IsNotNull(tab);
        }


        //[TestMethod()]
        //public void CloseTab()
        //{
        //    MetaModel.MetaModel.Initialize();
        //    var sut = new EditorTabs();
        //    Form f = CreateForm();
        //    f.Controls.Add(sut);
        //    PersistenceManager pManager = new PersistenceManager();
        //    f.Show();
        //    sut.OpenTab(pManager.NewTree());            
        //    //sut.OpenTab(pManager.NewTree());            
        //    //sut.CloseTab(pManager[0]);
        //    Assert.AreEqual(1, sut.TabCount, "Tab Count not matching.");
        //}

        //[TestMethod()]
        //public void CloseTab1()
        //{

        //    Assert.Fail();
        //}

        [TestMethod()]
        public void Focus()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new EditorTabs();
            Form f = CreateForm();
            f.Controls.Add(sut);
            f.Show();
            PersistenceManager pManager = new PersistenceManager();
            Tab tab = sut.OpenTab(pManager.NewTree());
            sut.Focus();
            Assert.IsTrue(tab.Control.Focused);
        }

        [TestMethod()]
        public void FindTab()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new EditorTabs();
            Form f = CreateForm();
            f.Controls.Add(sut);
            f.Show();
            PersistenceManager pManager = new PersistenceManager();
            PersistentTree tree = pManager.NewTree();
            Tab tab = sut.OpenTab(tree);

            Assert.IsNotNull(sut.FindTab(tree));
        }

        [TestMethod()]
        public void UpdateAppTitle()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new EditorTabs();
            Form f = CreateForm();
            f.Controls.Add(sut);
            f.Show();
            PersistenceManager pManager = new PersistenceManager();
            PersistentTree tree = pManager.NewTree();
            tree.RootNode.Text = "UpdateTitle";
            Tab tab = sut.OpenTab(tree);

            Assert.IsTrue(f.Text.Contains("UpdateTitle"));
        }

        [TestMethod()]
        public void ControlGotFocus()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new EditorTabs();
            Form f = CreateForm();
            f.Controls.Add(sut);
            int gotFocus = 0;
            sut.ControlGotFocus += (a, b) => gotFocus++;
            f.Show();
            PersistenceManager pManager = new PersistenceManager();
            PersistentTree tree = pManager.NewTree();            
            Tab tab = sut.OpenTab(tree);
            sut.Focus();

            Assert.AreEqual(1, gotFocus);
        }

        [TestMethod()]
        public void ControlGotFocus_MultipleCallsToFocus()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new EditorTabs();
            Form f = CreateForm();
            f.Controls.Add(sut);
            int gotFocus = 0;
            sut.ControlGotFocus += (a, b) => gotFocus++;
            f.Show();
            PersistenceManager pManager = new PersistenceManager();
            PersistentTree tree = pManager.NewTree();
            Tab tab = sut.OpenTab(tree);
            sut.Focus();
            sut.Focus();

            Assert.AreEqual(1, gotFocus);
        }

        private Form CreateForm()
        {
            Form form = new Form();
            form.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            form.ClientSize = new System.Drawing.Size(415, 304);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            form.KeyPreview = true;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Name = "TestForm";
            form.ShowInTaskbar = false;
            form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            form.Text = "Test Form";
            form.Size = new Size(320, 320);
            return form;
        }
    }
}
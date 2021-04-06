using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View.NoteEditing
{
    [TestClass()]
    public class HtmlCodeCleanerTests
    {
        [TestMethod()]
        public void Clean()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\Html Code Cleaner.mm");
            string html = null;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                form.Shown += (sender, args) =>
                {
                    editor.HTML = tree.RootNode.FirstChild.NoteText;

                    //pre change tests

                    //change
                    HtmlCodeCleaner.Clean(editor);

                    //post change tests

                    tree.RootNode.FirstChild.NoteText = editor.HTML;
                    p.CurrentTree.Save(@"Resources\Html Code Cleaner - Cleaned.mm");
                    html = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();

            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            //Assert.IsTrue(html.Contains("srcOrig"));
            //int imgUpdated = Regex.Matches(html, "srcOrig", RegexOptions.IgnoreCase).Count;
            //Assert.IsTrue(imgUpdated > 50);

            //int imgCount = Regex.Matches(html, "<img", RegexOptions.IgnoreCase).Count;
            //Assert.IsTrue(imgCount >= imgUpdated);
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

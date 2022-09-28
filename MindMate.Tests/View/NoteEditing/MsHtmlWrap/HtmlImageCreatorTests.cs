using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.NoteEditing;
using MindMate.View.NoteEditing.MsHtmlWrap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View.NoteEditing
{
    [TestClass()]
    public class HtmlImageCreatorTests
    {
        [TestMethod()]
        public void HtmlImageCreator()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    editor.HTML = null;
                    var sut = new HtmlImageCreator(editor);
                    sut.InsertImage("testimage.png","");                    
                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                result = editor.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<img"));
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
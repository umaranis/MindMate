using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.NoteEditing;
using mshtml;
using System.Drawing;
using System.Windows.Forms;

namespace MindMate.Tests.View.NoteEditing
{
    /// <summary>
    /// Test methods in this are executed in a separate thread to avoid following exception:
    ///     MindMate.Tests.View.NoteEditing.NoteEditorTests.CanExecuteCommand threw exception: 
    ///     System.Threading.ThreadStateException: ActiveX control '8856f961-340a-11d0-a96b-00c04fd705a2' cannot be instantiated because the current thread is not in a single-threaded apartment.    
    /// This exception occurs in some machines also (not sure what causes it).
    /// </summary>
    [TestClass()]
    public class NoteEditorTests
    {
        [TestMethod()]
        public void NoteEditor_Creation()
        {
            NoteEditor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                sut = new NoteEditor();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsNotNull(sut);
        }

        [TestMethod()]
        public void NoteEditor_Creation_HTMLIsNull()
        {
            bool result = true;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    result = sut.HTML == null;
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void NoteEditor_Creation_IsDirtyFalse()
        {
            bool result = true;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    result = !sut.Dirty;
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Setting HTML property programmatically doesn't make Dirty true. Only changes made through interface will make NoteEditor dirty.
        /// </summary>
        [TestMethod()]
        public void NoteEditor_SetHTML_IsDirtyFalse()
        {
            bool result = true;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    result = !sut.Dirty;
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Clear()
        {
            bool result = false;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();                
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    sut.HTML = "Sample Test";
                    sut.Clear();
                    result = sut.HTML == null;
                    form.Close();
                };
                form.ShowDialog();                
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void CanExecuteCommand()
        {
            bool result = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();            
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    result = sut.CanExecuteCommand(NoteEditorCommand.Bold);
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void QueryCommandState()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();            
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    result = !sut.QueryCommandState(NoteEditorCommand.Bold);
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void ExecuteCommand()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    sut.ExecuteCommand(NoteEditorCommand.Bold);
                    result = sut.QueryCommandState(NoteEditorCommand.Bold);
                    form.Close();
                };
                form.ShowDialog();            
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void InsertHyperlink()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    sut.HTML = "Website";
                    sut.ExecuteCommand(NoteEditorCommand.SelectAll);
                    sut.InsertHyperlink("umaranis.com");
                    result = sut.HTML != null && sut.HTML.Contains("umar");
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Paste()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    Clipboard.SetText("This is clipboard text");
                    sut.ExecuteCommand(NoteEditorCommand.Paste);
                    result = sut.HTML != null && sut.HTML.Contains("This is clipboard text");
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Paste_MakesDirty()
        {
            var result = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate { form.Close(); };
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    Clipboard.SetText("This is clipboard text");
                    sut.Paste();
                };
                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.Dirty;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Paste_SetHTMLNullThenPaste_MakesDirty()
        {
            var result = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate { form.Close(); };
                form.Controls.Add(sut);
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; //Clears Dirty flag
                    Clipboard.SetText("This is clipboard text");
                    sut.Paste();
                };
                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.Dirty;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        /// <summary>
        /// 1- Set HTML as Null (clears dirty and sets flag to ignore next dirty notification)
        /// 2- Set HTML as some text (clears dirty and sets flag to ignore next dirty notification)
        /// 3- Set HTML as some text again in same GUI event (this is to test that setting HTML twice doesn't generate 2 dirty notifications)
        /// 4- Dirty notification is generated in next GUI event and ignored
        /// 5- Change content of NoteEditor from frontend (should make NoteEditor dirty) (done in a separate GUI thread event using Timer)
        /// 6- Dirty notification is generated in next GUI event (assert will check for this)
        /// 7- Close the form in separate GUI thread event using Timer
        /// 8- Assert that NoteEditor is Dirty
        /// </summary>
        [TestMethod()]
        public void Paste_SetHTMLThenPaste_MakesDirty()
        {
            var result = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; //Clears Dirty flag
                    sut.HTML = "Some Text"; //Clears Dirty flag
                    sut.HTML = "Some Text"; //Clears Dirty flag
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        Clipboard.SetText("This is clipboard text");
                        sut.Paste();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);                
                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.Dirty;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        /// <summary>
        /// 1- Set HTML as Null (clears dirty and sets flag to ignore next dirty notification)
        /// 2- Set HTML as some text (clears dirty and sets flag to ignore next dirty notification)
        /// 3- Set HTML as some text again in another GUI event (this is to test that setting HTML twice ignore both dirty notifications)
        /// 4- Dirty notification is generated in next GUI event and ignored
        /// 5- Change content of NoteEditor from frontend (should make NoteEditor dirty) (done in a separate GUI thread event using Timer)
        /// 6- Dirty notification is generated in next GUI event (assert will check for this)
        /// 7- Close the form in separate GUI thread event using Timer
        /// 8- Assert that NoteEditor is Dirty
        /// </summary>
        [TestMethod()]
        public void Paste_SetHTML2ThenPaste_MakesDirty()
        {
            var result = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; //(1) Clears Dirty flag
                    sut.HTML = "Some Text"; //(2) Clears Dirty flag
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        sut.HTML = "Some Text"; //(3) Clears Dirty flag

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        Clipboard.SetText("This is clipboard text");
                        sut.Paste();//(5)
                    }
                    else
                    {
                        form.Close();//(7)
                    }
                };
                form.Controls.Add(sut);                
                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.Dirty;//(8)
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Bold()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; 
                    sut.HTML = "Some Text"; 
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ToggleSelectionBold();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("strong"));
        }

        [TestMethod()]
        public void Italic()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ToggleSelectionItalic();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<em>"));
        }

        [TestMethod()]
        public void Underline()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ToggleSelectionUnderline();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<u>"));
        }

        [TestMethod()]
        public void Strikeout()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ToggleSelectionStrikethrough();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<strike>"));
        }

        [TestMethod()]
        public void SetFontFamily()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionFontFamily("Arial");
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("arial"));
        }

        [TestMethod()]
        public void SetFontSize()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; //Clears Dirty flag
                    sut.HTML = "Some Text"; //Clears Dirty flag
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";                        
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionFontSize(22);                        
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);
                
                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("font-size: 22pt"));
        }

        [TestMethod()]
        public void SetFontSize_NoSelection()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; //Clears Dirty flag
                    sut.HTML = "Some Text"; //Clears Dirty flag
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        sut.SetSelectionFontSize(22);
                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";                        
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsFalse(result.ToLower().Contains("font-size: 22pt"));
        }

        [TestMethod()]
        public void SetFontSize_NoSelectionWithCursorPosition_SelectionExpands()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; //Clears Dirty flag
                    sut.HTML = "Some Text"; //Clears Dirty flag
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.collapse();
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionFontSize(22);
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("font-size: 22pt"));
        }

        [TestMethod()]
        public void SetFontSize_SelectionOnNode_ChangesNodeFontSize()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; //Clears Dirty flag
                    sut.HTML = "<span style='font-size:30pt'>Some Text</span>"; //Clears Dirty flag
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;                        
                        r.findText("Some Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionFontSize(22);
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("font-size: 22pt"));
            Assert.IsFalse(result.Contains("30"));
        }

        [TestMethod()]
        public void SetFontSize_SelectionOnNodeWithChildren_ChangesNodeFontSize()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null; 
                    sut.HTML = "<span style='font-size:30pt'>Some <span style='font-size:30pt'>Text</span></span>"; 
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Some Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionFontSize(22);
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("font-size: 22pt"));
            Assert.IsFalse(result.Contains("30"));
        }

        /// <summary>
        /// Mixed selection: Text + DOM Node
        /// </summary>
        [TestMethod()]
        public void SetFontSize_MixedSelectionWithChildren_ChangesNodeFontSize()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "This is <span style='font-size:30pt'>Some <span style='font-size:30pt'>Text</span></span>.";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("This is Some Text.");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionFontSize(22);
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("font-size: 22pt"));
            Assert.IsFalse(result.Contains("30"));
        }

        [TestMethod()]
        public void SetSelectionAsSubscript()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionAsSubscript();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<sub>"));
        }

        [TestMethod()]
        public void SetSelectionAsSuperscript()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionAsSuperscript();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<sup>"));
        }

        [TestMethod()]
        public void SetSelectionForeColor()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionForeColor(Color.Azure);
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("color"));
        }

        [TestMethod()]
        public void SetSelectionForeColor_ClearColor()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some <FONT color=azure>Text</FONT>";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionForeColor(Color.Empty);
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsFalse(result.ToLower().Contains("color"));
        }

        [TestMethod()]
        public void SetSelectionBackColor()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionBackColor(Color.Azure);
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("background-color"));
        }

        [TestMethod()]
        public void SetSelectionBackColor_ClearColor()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some <FONT style=\"BACKGROUND-COLOR: azure\">Text</FONT>>";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.SetSelectionBackColor(Color.Empty);
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsFalse(result.ToLower().Contains("background-color"));
        }

        [TestMethod]
        public void RemoveSelectionFormatting()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some <FONT style=\"BACKGROUND-COLOR: azure\">Text</FONT>>";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ClearSelectionFormatting();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsFalse(result.ToLower().Contains("background-color"));
        }

        [TestMethod]
        public void AddBullets()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.AddBullets();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("ul"));
        }

        [TestMethod]
        public void AddNumbering()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.AddNumbering();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("ol"));
        }

        [TestMethod]
        public void AlignSelectionLeft()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.AlignSelectionLeft();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("align=left"));
        }

        [TestMethod]
        public void AlignSelectionRight()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.AlignSelectionRight();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("align=right"));
        }

        [TestMethod]
        public void AlignSelectionCenter()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.AlignSelectionCenter();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("align=center"));
        }

        [TestMethod]
        public void AlignSelectionFull()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.AlignSelectionFull();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("align=justify"));
        }

        [TestMethod]
        public void IndentSelection()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.IndentSelection();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("blockquote"));
            Assert.IsTrue(result.ToLower().Contains("margin-right"));
        }

        [TestMethod]
        public void OutdentSelection()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.IndentSelection();
                    }
                    else
                    {
                        sut.OutdentSelection();
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsFalse(result.ToLower().Contains("blockquote"));            
        }

        [TestMethod]
        public void ApplyHeading1()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ApplyHeading1();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<h1>"));
        }

        [TestMethod]
        public void ApplyHeading2()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ApplyHeading2();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<h2>"));
        }

        [TestMethod]
        public void ApplyHeading3()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ApplyHeading3();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("<h3>"));
        }

        [TestMethod]
        public void ApplyNormalStyle()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    sut.HTML = null;
                    sut.HTML = "Some Text";
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";
                        var body = sut.Document.Body.DomElement as IHTMLBodyElement;
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.findText("Text");
                        r.select();

                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                        sut.ApplyNormalStyle();
                    }
                    else
                    {
                        form.Close();
                    }
                };
                form.Controls.Add(sut);

                timer.Start();
                form.ShowDialog();
                timer.Stop();
                result = sut.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsFalse(result.ToLower().Contains("<h"));
        }

        //[TestMethod()]
        //public void InsertImage()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Search()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Cut()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Copy()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ClearUndoStack()
        //{
        //    Assert.Fail();
        //}

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
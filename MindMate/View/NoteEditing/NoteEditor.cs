using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.Runtime.InteropServices;

namespace MindMate.View.NoteEditing
{
    public partial class NoteEditor : WebBrowser, IHTMLChangeSink
    {
        private IHTMLDocument2 htmlDoc;

        public NoteEditor()
        {
            InitializeComponent();

            // setting up WebBrowser for editing
            this.DocumentText = "<html><body></body></html>";
            htmlDoc = this.Document.DomDocument as IHTMLDocument2;
            htmlDoc.designMode = "On";            

            ScriptErrorsSuppressed = true;
            IsWebBrowserContextMenuEnabled = false;            

            // events
            this.Navigated += new WebBrowserNavigatedEventHandler(this_Navigated);
            this.GotFocus += new EventHandler(this_GotFocus);            
        }

                
        /// <summary>
        /// Event is fired after initlal document loading is cmplete and document is editable. (ReadyState = Complete)
        /// </summary>
        public event Action<object> Ready = delegate { };

        public event Action<object> OnDirty = delegate { };

        private bool ignoreDirtyNotification = true;               // Ignore when body as HTML property is set. Initialized by true to skip initial setup notification.
        public bool Dirty { get ; set; }            

        
        /// <summary>
        /// This is called when the initial html/body framework is set up, 
        /// or when document.DocumentText is set.  At this point, the 
        /// document is editable.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">navigation args</param>
        private void this_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            SetBackgroundColor(BackColor);

            Ready(sender);

            // register for change notification
            IMarkupContainer2 cont2 = (IMarkupContainer2)htmlDoc;
            uint m_cookie;
            cont2.RegisterForDirtyRange(this, out m_cookie);
        }

        /// <summary>
        /// Notification that contents of NoteEditor have changed. 
        /// Notification should be ignored if the content are changed due to MapNode selection changes (i.e. when HTML property is set)
        /// </summary>
        void IHTMLChangeSink.Notify()
        {
            if (ignoreDirtyNotification)
                ignoreDirtyNotification = false;
            else
            {
                Dirty = true;
                OnDirty(this);
            }            
        }


        /// <summary>
        /// Get/Set the background color of the editor.
        /// Note that if this is called before the document is rendered and 
        /// complete, the navigated event handler will set the body's 
        /// background color based on the state of BackColor.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                if (DocumentReady)
                {
                    SetBackgroundColor(value);
                }
            }
        }

        /// <summary>
        /// Set the background color of the body by setting it's CSS style
        /// </summary>
        /// <param name="value">the color to use for the background</param>
        private void SetBackgroundColor(Color value)
        {
            if (this.Document != null && this.Document.Body != null)
                this.Document.Body.Style = "background-color: " + MindMate.Serialization.Convert.ToColorHexValue(value);
        }

        /// <summary>
        /// Transfers the focus to HTML document body
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">EventArgs</param>
        private void this_GotFocus(object sender, EventArgs e)
        {
            if (this.Document != null && this.Document.Body != null)
                this.Document.Body.Focus();
        }

        /// <summary>
        /// Initializes editor contents.
        /// Clears Dirty flag and ensures that next dirty notification from Browser is ignored.
        /// </summary>
        public string HTML
        {
            get { return this.Document.Body.InnerHtml; } 
            set 
            {
                Dirty = false;
                if (this.Document.Body.InnerHtml == null && value == null) return; //should not set ignore dirty flag in this case
                ignoreDirtyNotification = true;
                this.Document.Body.InnerHtml = value;
            }
        }

        public new bool Enabled
        {
            get { return ((Control)this).Enabled; }
            set { ((Control)this).Enabled = value; }
        }

        /// <summary>
        /// Clear the contents of the document, leaving the body intact.
        /// </summary>
        public void Clear()
        {
            HTML = null;
        }

        public bool CanExecuteCommand(NoteEditorCommand command)
        {
            return htmlDoc.queryCommandEnabled(command.Value);
        }

        public bool QueryCommandState(NoteEditorCommand command)
        {
            return htmlDoc.queryCommandState(command.Value);
        }

        public void ExecuteCommand(NoteEditorCommand command)
        {
            htmlDoc.execCommand(command.Value);
        }

        public void InsertHyperlink(string url)
        {
            htmlDoc.execCommand("CreateLink", false, url);
        }

        public void InsertImage()
        {
            htmlDoc.execCommand("InsertImage", true, null);
        }

        public bool DocumentReady
        {
            get { return htmlDoc.readyState.Equals("complete", StringComparison.OrdinalIgnoreCase); }
        }

        public bool Empty
        {
            get { return htmlDoc.body.innerHTML == null || htmlDoc.body.innerHTML == "<P>&nbsp;</P>"; }
        }

        /// <summary>
        /// Current font name
        /// </summary>
        public FontFamily DocumentFontName
        {
            get
            {
                if (!DocumentReady)
                    return null;
                string name = htmlDoc.queryCommandValue("FontName") as string;
                if (name == null) return null;
                return new FontFamily(name);
            }
            set
            {
                if (value != null)
                    this.Document.ExecCommand("FontName", false, value.Name);
            }
        }

        /// <summary>
        /// Text color for the current selection
        /// </summary>
        public Color DocumentForeColor
        {
            get
            {
                if (!DocumentReady)
                    return Color.Black;
                return (Color)new ColorConverter().ConvertFromString(htmlDoc.queryCommandValue("ForeColor") as string);
            }
            set
            {
                string colorstr =
                    string.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B);
                htmlDoc.execCommand("ForeColor", false, colorstr);
            }
        }

        /// <summary>
        /// Background color for the current selection.
        /// </summary>
        [Browsable(false)]
        public Color DocumentBackColor
        {
            get
            {
                if (!DocumentReady)
                    return Color.White;
                return (Color)new ColorConverter().ConvertFromString(htmlDoc.queryCommandValue("BackColor") as string);
            }
            set
            {
                string colorstr =
                    string.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B);
                this.Document.ExecCommand("BackColor", false, colorstr);
            }
        }

        /// <summary>
        /// Search the document from the current selection, and reset the 
        /// the selection to the text found, if successful.
        /// </summary>
        /// <param name="text">the text for which to search</param>
        /// <param name="forward">true for forward search, false for backward</param>
        /// <param name="matchWholeWord">true to match whole word, false otherwise</param>
        /// <param name="matchCase">true to match case, false otherwise</param>
        /// <returns></returns>
        public bool Search(string text, bool forward, bool matchWholeWord, bool matchCase)
        {
            bool success = false;
            if (this.Document != null)
            {
                IHTMLDocument2 doc =
                    this.Document.DomDocument as IHTMLDocument2;
                IHTMLBodyElement body = doc.body as IHTMLBodyElement;
                if (body != null)
                {
                    IHTMLTxtRange range;
                    if (doc.selection != null)
                    {
                        range = doc.selection.createRange() as IHTMLTxtRange;
                        IHTMLTxtRange dup = range.duplicate();
                        dup.collapse(true);
                        // if selection is degenerate, then search whole body
                        if (range.isEqual(dup))
                        {
                            range = body.createTextRange();
                        }
                        else
                        {
                            if (forward)
                                range.moveStart("character", 1);
                            else
                                range.moveEnd("character", -1);
                        }
                    }
                    else
                        range = body.createTextRange();
                    int flags = 0;
                    if (matchWholeWord) flags += 2;
                    if (matchCase) flags += 4;
                    success =
                        range.findText(text, forward ? 999999 : -999999, flags);
                    if (success)
                    {
                        range.select();
                        range.scrollIntoView(!forward);
                    }
                }
            }
            return success;
        }


        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            //to block built-in dialogs from opening
            if (e.Control)
            {
                if (e.KeyCode == Keys.O || e.KeyCode == Keys.L)
                    e.IsInputKey = true;
            }

            base.OnPreviewKeyDown(e);
        }

        public void Cut()
        {            
            Document.ExecCommand("Cut", false, null);
        }

        public void Paste()
        {
            Document.ExecCommand("Paste", false, null);
        }

        public void Copy()
        {
            Document.ExecCommand("Copy", false, null);
        }

        /// <summary>
        /// Toggle bold formatting on the current selection.
        /// </summary>
        public void ToggleSelectionBold()
        {
            Document.ExecCommand("Bold", false, null);
        }

        /// <summary>
        /// Toggle italic formatting on the current selection.
        /// </summary>
        public void ToggleSelectionItalic()
        {
            Document.ExecCommand("Italic", false, null);
        }

        /// <summary>
        /// Toggle underline formatting on the current selection.
        /// </summary>
        public void ToggleSelectionUnderline()
        {
            Document.ExecCommand("Underline", false, null);
        }

        public void ToggleSelectionStrikethrough()
        {
            Document.ExecCommand("StrikeThrough", false, null);
        }

        public void SetSelectionFontFamily(string fontName)
        {
            Document.ExecCommand("FontName", false, fontName);
        }

        public void SetSelectionFontSize(float size)
        {
            try
            {
                //get selected range
                IHTMLTxtRange range = htmlDoc.selection.createRange() as IHTMLTxtRange;

                //range could be null if selection type is not text
                if (range == null) return;

                //expand to a word if nothing selected
                if (string.IsNullOrEmpty(range.htmlText))
                {
                    range.expand("word");

                    if (string.IsNullOrEmpty(range.htmlText)) return; //return if still null
                }

                //get the parent of selected range
                IHTMLElement elem = range.parentElement() as IHTMLElement;

                //check if selected range contains parent element
                bool isElement = !elem.tagName.Equals("BODY") &&
                    ((range.text == null && elem.outerText == null) || (range.text != null && range.text.Equals(elem.outerText)));

                if (isElement)
                {
                    //clear font size for all children
                    foreach (var c in (elem.children as IHTMLElementCollection))
                    {
                        ClearFontSize(c as IHTMLElement);
                    }

                    //set font size for the element
                    IHTMLStyle style = elem.style as IHTMLStyle;
                    style.fontSize = size + "pt";
                }
                else
                {
                    //clear font size for all elements inside the selection
                    var body = htmlDoc.body as IHTMLBodyElement;
                    foreach (IHTMLElement childElem in (elem.children as IHTMLElementCollection))
                    {
                        IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                        r.moveToElementText(childElem);
                        if (range.inRange(r)) ClearFontSize(childElem);
                    }

                    //set font size by surrounding with span                                     
                    range.pasteHTML("<span style='font-size:" + size + "pt'>" + range.htmlText + "</span>");                    
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + ": NoteEdiitor.SetFontSize" + e.Message);
                System.Diagnostics.Trace.WriteLine(e.StackTrace);
            }            
        }

        private void ClearFontSize(IHTMLElement element)
        {
            IHTMLStyle style = element.style as IHTMLStyle;
            style.fontSize = "";

            foreach(IHTMLElement childElem in (element.children as IHTMLElementCollection))
            {
                ClearFontSize(childElem);
            }
        }

        public void SetSelectionAsSubscript()
        {
            Document.ExecCommand("Subscript", false, null);            
        }

        public void SetSelectionAsSuperscript()
        {
            Document.ExecCommand("Superscript", false, null);
        }

        public void SetSelectionForeColor(Color color)
        {
            string colorHtml = color != Color.Empty? ColorTranslator.ToHtml(color) : "";
            Document.ExecCommand("ForeColor", false, colorHtml);            
        }

        public void SetSelectionBackColor(Color color)
        {
            string colorHtml = color != Color.Empty ? ColorTranslator.ToHtml(color) : "";
            Document.ExecCommand("BackColor", false, colorHtml);
        }

        public void ClearSelectionFormatting()
        {
            Document.ExecCommand("RemoveFormat", false, null);
        }

        public void AddBullets()
        {
            Document.ExecCommand("InsertUnorderedList", false, null);
        }

        public void AddNumbering()
        {
            Document.ExecCommand("InsertOrderedList", false, null);
        }

        public void AlignSelectionLeft()
        {
            Document.ExecCommand("JustifyLeft", false, null);
        }

        public void AlignSelectionRight()
        {
            Document.ExecCommand("JustifyRight", false, null);
        }

        public void AlignSelectionCenter()
        {
            Document.ExecCommand("JustifyCenter", false, null);
        }

        public void AlignSelectionFull()
        {
            Document.ExecCommand("JustifyFull", false, null);
        }

        public void IndentSelection()
        {
            Document.ExecCommand("Indent", false, null);
        }

        public void OutdentSelection()
        {
            Document.ExecCommand("Outdent", false, null);
        }

        /// <summary>
        /// Undo the last operation
        /// </summary>
        public void Undo()
        {
            Document.ExecCommand("Undo", false, null);
        }

        /// <summary>
        /// Redo based on the last Undo
        /// </summary>
        public void Redo()
        {
            Document.ExecCommand("Redo", false, null);
        }

        private IOleUndoManager oleUndoManager;

        private IOleUndoManager GetUndoManager()
        {
            if (oleUndoManager == null)
            {
                Guid SID_SOleUndoManager = new Guid("D001F200-EF97-11CE-9BC9-00AA00608E01");
                Guid IID_IOleUndoManager = new Guid("D001F200-EF97-11CE-9BC9-00AA00608E01");

                UCOMIServiceProvider isp = (UCOMIServiceProvider)htmlDoc;
                IntPtr ip = isp.QueryService(ref SID_SOleUndoManager, ref IID_IOleUndoManager);
                oleUndoManager = (IOleUndoManager)Marshal.GetObjectForIUnknown(ip);
            }
            return oleUndoManager;
        }

        public void ClearUndoStack()
        {
            GetUndoManager().DiscardFrom(null);
        }

    }

    public sealed class NoteEditorCommand
    {
        // check using CanExecuteCommand before executing
        public static readonly NoteEditorCommand Undo = new NoteEditorCommand("Undo");
        public static readonly NoteEditorCommand Redo = new NoteEditorCommand("Redo");
        public static readonly NoteEditorCommand Cut = new NoteEditorCommand("Cut");
        public static readonly NoteEditorCommand Copy = new NoteEditorCommand("Copy");
        public static readonly NoteEditorCommand Paste = new NoteEditorCommand("Paste");
        public static readonly NoteEditorCommand Delete = new NoteEditorCommand("Delete");

        // query for command state
        public static readonly NoteEditorCommand JustifyLeft = new NoteEditorCommand("JustifyLeft");
        public static readonly NoteEditorCommand JustifyRight = new NoteEditorCommand("JustifyRight");
        public static readonly NoteEditorCommand JustifyCenter = new NoteEditorCommand("JustifyCenter");
        public static readonly NoteEditorCommand JustifyFull = new NoteEditorCommand("JustifyFull");
        public static readonly NoteEditorCommand Bold = new NoteEditorCommand("Bold");
        public static readonly NoteEditorCommand Italic = new NoteEditorCommand("Italic");
        public static readonly NoteEditorCommand Underline = new NoteEditorCommand("Underline");
        public static readonly NoteEditorCommand InsertOrderedList = new NoteEditorCommand("InsertOrderedList");
        public static readonly NoteEditorCommand InsertUnorderedList = new NoteEditorCommand("InsertUnorderedList");

        // no need to check before executing
        public static readonly NoteEditorCommand InsertHorizontalRule = new NoteEditorCommand("InsertHorizontalRule");
        public static readonly NoteEditorCommand Print = new NoteEditorCommand("Print");
        public static readonly NoteEditorCommand InsertParagraph = new NoteEditorCommand("InsertParagraph");
        public static readonly NoteEditorCommand SelectAll = new NoteEditorCommand("SelectAll");
        public static readonly NoteEditorCommand Indent = new NoteEditorCommand("Indent");
        public static readonly NoteEditorCommand Outdent = new NoteEditorCommand("Outdent");
        
        private NoteEditorCommand(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}

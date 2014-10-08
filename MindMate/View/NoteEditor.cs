using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;

namespace MindMate.View
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

            // events
            this.Navigated += new WebBrowserNavigatedEventHandler(this_Navigated);
            this.GotFocus += new EventHandler(this_GotFocus);            
        }

                
        /// <summary>
        /// Event is fired after initlal document loading is cmplete and document is editable. (ReadyState = Complete)
        /// </summary>
        public event Action<object> Ready = delegate { };

        public event Action<object> OnDirty = delegate { };

        private bool ignoreDirtyNotification = false;               // ignore when body as HTML property is set
        private bool ignoreInitialDirtyNotification = true;         // ignore initial dirty notification due to initially setting DocumentText 
        public bool Dirty { get ; set; }            

        
        /// <summary>
        /// This is called when the initial html/body framework is set up, 
        /// or when document.DocumentText is set.  At this point, the 
        /// document is editable.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">navigation args</param>
        void this_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            SetBackgroundColor(BackColor);

            IMarkupContainer2 cont2 = (IMarkupContainer2)htmlDoc;
            uint m_cookie;
            cont2.RegisterForDirtyRange(this, out m_cookie);

            Ready(sender);
        }

        void IHTMLChangeSink.Notify()
        {
           if (ignoreDirtyNotification)
                ignoreDirtyNotification = false;
            else if (ignoreInitialDirtyNotification)
                ignoreInitialDirtyNotification = false;
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
                this.Document.Body.Style = "background-color: " + value.Name;
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
        /// Get / Set / Clear editor contents.
        /// Should not be called more than once from a single event. This will increment ignoreDirtyNotification multiple times but only one dirty event will be called.
        /// </summary>
        public string HTML
        {
            get { return this.Document.Body.InnerHtml; } 
            set 
            {
                Dirty = false;
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
        /// Current font size. 0 if not available.
        /// </summary>
        public float DocumentFontSize
        {
            get
            {
                if (!DocumentReady)
                    return 0;
                return float.Parse(htmlDoc.queryCommandValue("FontSize") as string);                                
            }
            set
            {
                htmlDoc.execCommand("FontSize", false, value.ToString());
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

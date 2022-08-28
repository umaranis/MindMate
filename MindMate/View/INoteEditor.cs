using System;
using System.Drawing;

namespace MindMate.View
{
    public interface INoteEditor
    {
        void Dispose();

        /// <summary>
        /// Event is fired after initlal document loading is cmplete and document is editable. (ReadyState = Complete)
        /// </summary>
        event Action<object> Ready;

        event Action<object> CursorMoved;

        /// <summary>
        /// Event is fired before Paste command is executed.
        /// If PastingEventArgs.Handled is set to true, default Paste behaviour is not invoked.
        /// </summary>
        event Action<object, PastingEventArgs> Pasting;

        /// <summary>
        /// Event is fired when something is pasted in the Note Editor or HTML source is edited.
        /// </summary>
        event Action<object> ExternalContentAdded;

        event Action<object> OnDirty;

        /// <summary>
        /// Specifies if the editor has unsaved changes.
        /// Don't expect dirty flag to be checked right after the change. Dirty flag is usually set in the next event loop call triggered by registering IHTMLChangeSink.Notify. 
        /// </summary>
        bool Dirty { get; set; }

        /// <summary>
        /// Get/Set the background color of the editor.
        /// Note that if this is called before the document is rendered and 
        /// complete, the navigated event handler will set the body's 
        /// background color based on the state of BackColor.
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Get/Set the foreground color of the editor.
        /// Note that if this is called before the document is rendered and 
        /// complete, the navigated event handler will set the body's 
        /// color based on the state of ForeColor.
        /// </summary>
        Color ForeColor { get; set; }

        /// <summary>
        /// Initializes editor contents.
        /// Clears Dirty flag and ensures that next dirty notification from Browser is ignored.
        /// </summary>
        string HTML { get; set; }

        new bool Enabled { get; set; }
        bool DocumentReady { get; }
        bool Empty { get; }

        /// <summary>
        /// Text color for the current selection
        /// </summary>
        Color DocumentForeColor { get; set; }

        /// <summary>
        /// Background color for the current selection.
        /// </summary>
        Color DocumentBackColor { get; set; }

        /// <summary>
        /// Event is fired when user presses Ctrl + S in the note editor window
        /// </summary>
        event Action<object> OnSave;

        /// <summary>
        /// This makes the editor dirty as opposed to setting HTML property
        /// </summary>
        /// <param name="html"></param>
        void UpdateHtmlSource(string html);

        /// <summary>
        /// Clear the contents of the document, leaving the body intact.
        /// </summary>
        void Clear();

        bool InsideTable { get; }

        bool CanExecuteCommand(NoteEditorCommand command);
        bool QueryCommandState(NoteEditorCommand command);
        void ExecuteCommand(NoteEditorCommand command);
        void InsertHyperlink(string url);

        /// <summary>
        /// Search the document from the current selection, and reset the 
        /// the selection to the text found, if successful.
        /// </summary>
        /// <param name="text">the text for which to search</param>
        /// <param name="forward">true for forward search, false for backward</param>
        /// <param name="matchWholeWord">true to match whole word, false otherwise</param>
        /// <param name="matchCase">true to match case, false otherwise</param>
        /// <returns></returns>
        bool Search(string text, bool forward, bool matchWholeWord, bool matchCase);

        void Cut();
        void Paste();
        void Copy();

        /// <summary>
        /// Toggle bold formatting on the current selection.
        /// </summary>
        void ToggleSelectionBold();

        /// <summary>
        /// Toggle italic formatting on the current selection.
        /// </summary>
        void ToggleSelectionItalic();

        /// <summary>
        /// Toggle underline formatting on the current selection.
        /// </summary>
        void ToggleSelectionUnderline();

        void ToggleSelectionStrikethrough();
        void SetSelectionFontFamily(string fontName);
        void SetSelectionFontSize(float size);
        void SetSelectionAsSubscript();
        void SetSelectionAsSuperscript();
        void SetSelectionForeColor(Color color);
        void SetSelectionBackColor(Color color);
        void ClearSelectionFormatting();
        void AddBullets();
        void AddNumbering();
        void AlignSelectionLeft();
        void AlignSelectionRight();
        void AlignSelectionCenter();
        void AlignSelectionFull();
        void IndentSelection();
        void OutdentSelection();
        void ApplyHeading1();
        void ApplyHeading2();
        void ApplyHeading3();
        void ApplyNormalStyle();

        /// <summary>
        /// Undo the last operation
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo based on the last Undo
        /// </summary>
        void Redo();

        void ClearUndoStack();
        void Focus();
    }
    
    public class PastingEventArgs : EventArgs
    {
        /// <summary>
        /// If handled is set to true, default pasting behaviour is not executed
        /// </summary>
        public bool Handled { get; set; }
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
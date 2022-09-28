using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    public class UndoUnit : IDisposable
    {
        IMarkupServicesRaw markupServices;

        // Begins Undo Unit
        public UndoUnit(HTMLDocument document, string title)
        {
            markupServices = document as IMarkupServicesRaw;
            markupServices.BeginUndoUnit(title);
        }        

        /// <summary>
        /// Ends Undo Unit
        /// </summary>
        public void Dispose()
        {
            markupServices.EndUndoUnit();
        }
    }
}

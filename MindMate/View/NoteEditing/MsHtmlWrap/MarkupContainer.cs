// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
using mshtml;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    /// <summary>
    /// This class is a convenience wrapper for the MSHTML IMarkupContainer interface.
    /// </summar>
    public class MarkupContainer
    {
        internal readonly IMarkupContainerRaw Container;
        private readonly MshtmlMarkupServices MarkupServices;

        internal MarkupContainer(MshtmlMarkupServices markupServices, IMarkupContainerRaw container)
        {
            MarkupServices = markupServices;
            Container = container;
        }

        private IHTMLDocument2 document;
        /// <summary>
        /// Returns the document object hosted by this container.
        /// </summary>
        public IHTMLDocument2 Document
        {
            get
            {
                if (document == null)
                {
                    //create a temp pointer that can walk the document until it finds an element
                    MarkupPointer p = MarkupServices.CreateMarkupPointer();
                    p.MoveToContainer(this, true);

                    IHTMLElement currentElement = p.CurrentScope;
                    if (currentElement == null)
                    {
                        MarkupContext markupContext = new MarkupContext();
                        p.Right(true, markupContext);
                        while (markupContext.Element == null && markupContext.Context != _MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_None)
                            p.Right(true, markupContext);
                        currentElement = markupContext.Element;
                    }
                    if (currentElement != null)
                        document = (IHTMLDocument2)currentElement.document;
                }
                return document;
            }
        }       

        /// <summary>
        /// Create a text range spanning the specified markup positions.
        /// </summary>
        /// <param name="start">the start point of the text range</param>
        /// <param name="end">the end point of the text range</param>
        /// <returns></returns>
        public IHTMLTxtRange CreateTextRange(MarkupPointer start, MarkupPointer end)
        {
            // when switching between wywiwyg and source view sometimes .body is null
            // and this throws a null ref exception that we catch (can be detected by enabling
            // exception breaking in the debugger)
            IHTMLTxtRange range = (Document.body as IHTMLBodyElement).createTextRange();
            MarkupServices.MoveRangeToPointers(start, end, range);
            return range;
        }
                
    }
}


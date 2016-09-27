using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using mshtml;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    /// <summary>
    /// Delegate used to filter element scanning operations. If this operation returns true, then the
    /// scanning operation will consider the element as relevant to the scan.
    /// </summary>
    public delegate bool IHTMLElementFilter(IHTMLElement e);

    /// <summary>
    /// Range of markup within a document
    /// </summary>
    public class MarkupRange
    {
        /// <summary>
        /// Initialize with begin and end pointers
        /// </summary>
        /// <param name="start">start</param>
        /// <param name="end">end</param>
        /// <param name="markupServices"></param>
        internal MarkupRange(MarkupPointer start, MarkupPointer end, MshtmlMarkupServices markupServices)
        {
            Start = start;
            End = end;
            MarkupServices = markupServices;
        }
                                        
        /// <summary>
        /// Creates a clone that spans the same range as this MarkupRange.
        /// Note: The clone can be manipulated without changing the position of this range.
        /// </summary>
        /// <returns></returns>
        public MarkupRange Clone()
        {
            MarkupRange clone = MarkupServices.CreateMarkupRange();
            clone.Start.MoveToPointer(Start);
            clone.Start.Cling = Start.Cling;
            clone.Start.Gravity = Start.Gravity;
            clone.End.MoveToPointer(End);
            clone.End.Cling = End.Cling;
            clone.End.Gravity = End.Gravity;
            return clone;
        }
                
        ///// <summary>
        ///// Gets the elements in the range that match the filter.
        ///// </summary>
        ///// <param name="filter">the delegate testing each element to determine if it should be added to the list of elements to return</param>
        ///// <param name="inScopeElementsOnly">if true, the only</param>
        ///// <returns></returns>
        //public IHTMLElement[] GetElements(IHTMLElementFilter filter, bool inScopeElementsOnly)
        //{
        //    ArrayList list = new ArrayList();
        //    if (!IsEmpty())
        //    {
        //        Hashtable usedElements = new Hashtable();
        //        MarkupPointer p = MarkupServices.CreateMarkupPointer(Start);
        //        MarkupPointer end = MarkupServices.CreateMarkupPointer(End);
        //        MarkupContext context = p.Right(false);

        //        //move p through the range to locate each the elements adding elements that pass the filter
        //        while (p.IsLeftOfOrEqualTo(end))
        //        {
        //            if (context.Context == _MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_EnterScope
        //                || context.Context == _MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_ExitScope
        //                || context.Context == _MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_NoScope)
        //            {
        //                if (usedElements[context.Element] == null)
        //                {
        //                    if ((inScopeElementsOnly && isInScope(context.Element)) || !inScopeElementsOnly)
        //                        if (filter(context.Element))
        //                        {
        //                            list.Add(context.Element);
        //                        }

        //                    //cache the fact that we've already tested this element.
        //                    usedElements[context.Element] = context.Element;
        //                }
        //            }
        //            p.Right(true, context);
        //        }
        //    }
        //    return HTMLElementHelper.ToElementArray(list);
        //}

        /// <summary>
        /// Gets the first element in the range that match the filter.
        /// </summary>
        /// <param name="filter">the delegate testing each element to determine if it should be added to the list of elements to return</param>
        /// <param name="inScopeElementsOnly">if true, the only</param>
        /// <returns></returns>
        public IHTMLElement GetFirstElement(IHTMLElementFilter filter, bool inScopeElementsOnly)
        {
            ArrayList list = new ArrayList();
            if (!IsEmpty())
            {
                Hashtable usedElements = new Hashtable();
                MarkupPointer p = MarkupServices.CreateMarkupPointer(Start);
                MarkupPointer end = MarkupServices.CreateMarkupPointer(End);
                MarkupContext context = p.Right(false);

                //move p through the range to locate each the elements adding elements that pass the filter
                while (p.IsLeftOfOrEqualTo(end))
                {
                    if (context.Context == _MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_EnterScope
                        || context.Context == _MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_ExitScope
                        || context.Context == _MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_NoScope)
                    {
                        if (usedElements[context.Element] == null)
                        {
                            if ((inScopeElementsOnly && isInScope(context.Element)) || !inScopeElementsOnly)
                                if (filter(context.Element))
                                {
                                    return context.Element;
                                }

                            //cache the fact that we've already tested this element.
                            usedElements[context.Element] = context.Element;
                        }
                    }
                    p.Right(true, context);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a text range located at the same position as this MarkupRange.
        /// </summary>
        /// <returns></returns>
        public IHTMLTxtRange ToTextRange()
        {
            return MarkupServices.CreateTextRange(Start, End);
        }

        /// <summary>
        /// Returns true if the start and end points of the range are equal.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Start.IsEqualTo(End);
        }

        /// <summary>
        /// Beginning of range
        /// </summary>
        public MarkupPointer Start;

        /// <summary>
        /// End of range
        /// </summary>
        public MarkupPointer End;

        internal MshtmlMarkupServices MarkupServices;

        #region PRIVATE UTILITIES

        /// <summary>
        /// Returns true if the specified element begins and ends within the range.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool isInScope(IHTMLElement e)
        {
            MarkupPointer p = MarkupServices.CreateMarkupPointer(e, _ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
            if (p.IsRightOfOrEqualTo(Start))
            {
                p = MarkupServices.CreateMarkupPointer(e, _ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd);
                if (p.IsLeftOfOrEqualTo(End))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
        
    }
}
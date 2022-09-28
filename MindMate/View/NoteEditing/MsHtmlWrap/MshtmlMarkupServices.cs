// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using mshtml;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    /// <summary>
    /// This class is a convenience wrapper for the MSHTML IMarkupServices interface.
    /// </summary>
    public class MshtmlMarkupServices
    {
        private readonly IMarkupServicesRaw MarkupServices;

        public MshtmlMarkupServices(IMarkupServicesRaw markupServices)
        {
            MarkupServices = markupServices;
        }

        /// <summary>
        /// Creates an instance of a MarkupPointer object.
        /// </summary>
        /// <returns></returns>
        public MarkupPointer CreateMarkupPointer()
        {
            IMarkupPointerRaw pointer;
            MarkupServices.CreateMarkupPointer(out pointer);
            return new MarkupPointer(this, pointer);
        }

        /// <summary>
        /// Creates an instance of the IMarkupPointer object with an initial position
        /// at the same location as another pointer.
        /// </summary>
        /// <param name="initialPosition"></param>
        /// <returns></returns>
        public MarkupPointer CreateMarkupPointer(MarkupPointer initialPosition)
        {
            MarkupPointer pointer = CreateMarkupPointer();
            pointer.MoveToPointer(initialPosition);
            return pointer;
        }

        /// <summary>
        /// Create an unpositioned MarkupRange.
        /// </summary>
        /// <returns></returns>
        public MarkupRange CreateMarkupRange()
        {
            MarkupPointer start = CreateMarkupPointer();
            MarkupPointer end = CreateMarkupPointer();
            end.Gravity = _POINTER_GRAVITY.POINTER_GRAVITY_Right;
            return CreateMarkupRange(start, end);
        }

        /// <summary>
        /// Create a MarkupRange from a selection object.
        /// </summary>
        public MarkupRange CreateMarkupRange(IHTMLSelectionObject selection)
        {
            if (selection == null)
            {
                return null;
            }

            // see what type of range is selected
            object range = selection.createRange();
            if (range is IHTMLTxtRange)
            {
                return CreateMarkupRange(range as IHTMLTxtRange);
            }
            else if (range is IHTMLControlRange)
            {
                // we only support single-selection so a "control-range" can always
                // be converted into a single-element text range
                IHTMLControlRange controlRange = range as IHTMLControlRange;
                if (controlRange.length == 1)
                {
                    IHTMLElement selectedElement = controlRange.item(0);
                    MarkupRange markupRange = CreateMarkupRange(selectedElement);

                    //return the precisely positioned text range
                    return markupRange;
                }
                else
                {
                    Debug.Fail("Length of control range not equal to 1 (value was " + controlRange.length.ToString(CultureInfo.InvariantCulture));
                    return null;
                }
            }
            else // null or unexpected range type
            {
                return null;
            }

        }

        /// <summary>
        /// Create a MarkupRange from that surrounds an Element.
        /// </summary>
        /// <returns></returns>
        public MarkupRange CreateMarkupRange(IHTMLElement element)
        {
            return CreateMarkupRange(element, true);
        }

        /// <summary>
        /// Create a MarkupRange from that surrounds an Element.
        /// </summary>
        /// <returns></returns>
        public MarkupRange CreateMarkupRange(IHTMLElement element, bool outside)
        {
            _ELEMENT_ADJACENCY beginAdj = outside ? _ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin : _ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin;
            _ELEMENT_ADJACENCY endAdj = outside ? _ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd : _ELEMENT_ADJACENCY.ELEM_ADJ_BeforeEnd;
            MarkupPointer Begin = CreateMarkupPointer(element, beginAdj);
            MarkupPointer End = CreateMarkupPointer(element, endAdj);
            End.Gravity = _POINTER_GRAVITY.POINTER_GRAVITY_Right;
            MarkupRange markupRange = new MarkupRange(Begin, End, this);
            return markupRange;
        }

        /// <summary>
        /// Create a MarkupRange from a TextRange.
        /// </summary>
        /// <param name="textRange"></param>
        /// <returns></returns>
        public MarkupRange CreateMarkupRange(IHTMLTxtRange textRange)
        {
            MarkupPointer Begin = CreateMarkupPointer();
            MarkupPointer End = CreateMarkupPointer();
            End.Gravity = _POINTER_GRAVITY.POINTER_GRAVITY_Right;
            MovePointersToRange(textRange, Begin, End);
            MarkupRange markupRange = new MarkupRange(Begin, End, this);
            return markupRange;
        }

        /// <summary>
        /// Create a MarkupRange from a set of MarkupPointers.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public MarkupRange CreateMarkupRange(MarkupPointer start, MarkupPointer end)
        {
            MarkupRange markupRange = new MarkupRange(start, end, this);
            return markupRange;
        }

        /// <summary>
        /// Create a TextRange that spans a set of pointers.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IHTMLTxtRange CreateTextRange(MarkupPointer start, MarkupPointer end)
        {
            Debug.Assert(start.Positioned && end.Positioned, "pointers are not positioned");
            IHTMLTxtRange range = start.Container.CreateTextRange(start, end);
            return range;
        }

        /// <summary>
        /// Creates an instance of the IMarkupPointer object with an initial position
        /// adjacent to the specified HTML element.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="eAdj"></param>
        /// <returns></returns>
        public MarkupPointer CreateMarkupPointer(IHTMLElement e, _ELEMENT_ADJACENCY eAdj)
        {
            MarkupPointer pointer = CreateMarkupPointer();
            pointer.MoveAdjacentToElement(e, eAdj);
            return pointer;
        }

        /// <summary>
        /// Positions pointers at the edges of an existing range.
        /// </summary>
        /// <param name="range">the text range to move to</param>
        /// <param name="start">the pointer to position at the start of the range</param>
        /// <param name="end">the pointer to position at the end of the range</param>
        public void MovePointersToRange(IHTMLTxtRange range, MarkupPointer start, MarkupPointer end)
        {
            MarkupServices.MovePointersToRange(range, start.PointerRaw, end.PointerRaw);
        }

        /// <summary>
        /// Positions pointers at the edges of an existing range.
        /// </summary>
        /// <param name="start">the pointer positioned at the start of the range</param>
        /// <param name="end">the pointer position at the end of the range</param>
        /// <param name="range">the text range to move</param>
        public void MoveRangeToPointers(MarkupPointer start, MarkupPointer end, IHTMLTxtRange range)
        {
            MarkupServices.MoveRangeToPointers(start.PointerRaw, end.PointerRaw, range);
        }       
        
    }
}

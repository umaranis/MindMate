using System;
using System.Collections;
using System.Globalization;
using System.Text;
using mshtml;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    /// <summary>
    /// A pointer positioned within an HTML Document object.
    /// This class is a convenience wrapper for the MSHTML IMarkupPointer interface.
    /// </summary>
    public class MarkupPointer
    {
        private readonly IMarkupPointerRaw _pointerRaw;
        private readonly MshtmlMarkupServices MarkupServices;

        internal MarkupPointer(MshtmlMarkupServices markupServices, IMarkupPointerRaw pointer)
        {
            MarkupServices = markupServices;
            _pointerRaw = pointer;
        }

        public IMarkupPointerRaw PointerRaw
        {
            get
            {
                return _pointerRaw;
            }
        }

        /// <summary>
        /// Enable/Disable the cling attribute for this markup pointer.
        /// </summary>
        public bool Cling
        {
            get
            {
                bool b;
                PointerRaw.Cling(out b);
                return b;
            }
            set
            {
                PointerRaw.SetCling(value);
            }
        }
                
        /// <summary>
        /// Retrieves the IHTMLElement positioned in this pointer.
        /// </summary>
        public IHTMLElement CurrentScope
        {
            get
            {
                IHTMLElement currentScope;
                PointerRaw.CurrentScope(out currentScope);
                return currentScope;
            }
        }

        /// <summary>
        /// Retrieves the container associated with this markup pointer.
        /// </summary>
        public MarkupContainer Container
        {
            get
            {
                IMarkupContainerRaw container;
                PointerRaw.GetContainer(out container);
                return new MarkupContainer(MarkupServices, container);
            }
        }
                        
        /// <summary>
        /// Get/Set the gravity attribute of this pointer.
        /// Pointer gravity determines whether a markup pointer will stay with the markup
        /// to its right or left when markup is inserted at the pointer's location.
        /// By default, markup pointers have "left gravity": that is, they stay with the
        /// markup to their left when text is inserted at the pointer's location.
        /// </summary>
        public _POINTER_GRAVITY Gravity
        {
            get
            {
                _POINTER_GRAVITY gravity;
                PointerRaw.Gravity(out gravity);
                return gravity;
            }
            set
            {
                PointerRaw.SetGravity(value);
            }
        }

        /// <summary>
        /// Checks to see whether this pointer's position is equal to another pointer's position.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsEqualTo(MarkupPointer p)
        {
            bool b;
            PointerRaw.IsEqualTo(p.PointerRaw, out b);
            return b;
        }

        /// <summary>
        /// Checks to see whether this pointer's position is to the left of another pointer's position.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsLeftOf(MarkupPointer p)
        {
            bool b;
            PointerRaw.IsLeftOf(p.PointerRaw, out b);
            return b;
        }

        /// <summary>
        /// Checks to see whether this pointer's position is to the left of or is equal to another pointer's position.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsLeftOfOrEqualTo(MarkupPointer p)
        {
            bool b;
            PointerRaw.IsLeftOfOrEqualTo(p.PointerRaw, out b);
            return b;
        }

        /// <summary>
        /// Checks to see whether this pointer's position is to the right of another pointer's position.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsRightOf(MarkupPointer p)
        {
            bool b;
            PointerRaw.IsRightOf(p.PointerRaw, out b);
            return b;
        }

        /// <summary>
        /// Checks to see whether this pointer's position is to the right of or is equal to another pointer's position.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsRightOfOrEqualTo(MarkupPointer p)
        {
            bool b;
            PointerRaw.IsRightOfOrEqualTo(p.PointerRaw, out b);
            return b;
        }

        /// <summary>
        /// Moves the pointer adjacent to an element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="eAdj"></param>
        public void MoveAdjacentToElement(IHTMLElement element, _ELEMENT_ADJACENCY eAdj)
        {
            PointerRaw.MoveAdjacentToElement(element, eAdj);
        }

        /// <summary>
        /// Moves the pointer to a markup container.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="atStart">bool that specifies whether to position the pointer at the beginning of the container's content.</param>
        public void MoveToContainer(MarkupContainer container, bool atStart)
        {
            PointerRaw.MoveToContainer(container.Container, atStart);
        }

        /// <summary>
        /// Moves this pointer to another pointer's location.
        /// </summary>
        /// <param name="p"></param>
        public void MoveToPointer(MarkupPointer p)
        {
            PointerRaw.MoveToPointer(p.PointerRaw);
        }
                
        /// <summary>
        /// Returns true if this pointer is currently positioned.
        /// </summary>
        /// <returns></returns>
        public bool Positioned
        {
            get
            {
                bool isPositioned;
                PointerRaw.IsPositioned(out isPositioned);
                return isPositioned;
            }
        }

        /// <summary>
        /// Inspects the content of the container to the right of the markup pointer and optionally moves
        /// the pointer one position to the right.
        /// </summary>
        /// <param name="move">TRUE if the pointer is to move past the content to the right, or FALSE otherwise.
        /// If TRUE, the pointer will move either to the other side of the tag or text to its right, depending on
        /// the CONTEXT_TYPE to the pointer's right.
        /// </summary>
        /// <param name="move"></param>
        /// <returns>A MarkupContext object describing the content positioned to the pointer's left</returns>
        public MarkupContext Right(bool move)
        {
            MarkupContext context = new MarkupContext();
            Right(move, context);
            return context;
        }

        /// <summary>
        /// Inspects the content of the container to the right of the markup pointer and optionally moves
        /// the pointer one position to the right.
        /// </summary>
        /// <param name="move">TRUE if the pointer is to move past the content to the right, or FALSE otherwise.
        /// If TRUE, the pointer will move either to the other side of the tag or text to its right, depending on
        /// the CONTEXT_TYPE to the pointer's right.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="context">context object to populate with the context information</param>
        public void Right(bool move, MarkupContext context)
        {
            PointerRaw.Right(move, out context.Context, out context.Element, IntPtr.Zero, IntPtr.Zero);
        }       
              
    }

    /// <summary>
    /// Describes the HTML content that a MarkupPointer is positioned next to.
    /// </summary>
    public class MarkupContext
    {
        /// <summary>
        /// Enumeration value that describes the content to the next to the markup pointer
        /// </summary>
        public _MARKUP_CONTEXT_TYPE Context;

        /// <summary>
        /// The element, if any, that is coming into scope, is exiting scope, or is a no-scope
        /// element (such as a br element), as specified by pContext
        /// </summary>
        public IHTMLElement Element;

        /// <summary>
        /// The text that is coming into scope or null if there is no text coming into scope.
        /// </summary>
        //public string Text;

        public MarkupContext()
        {
        }
    }
}
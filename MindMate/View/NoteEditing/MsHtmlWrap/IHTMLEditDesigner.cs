using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    [Guid("3050f662-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IHTMLEditDesigner
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int PreHandleEvent(
            [In] int dispId,
            [In, MarshalAs(UnmanagedType.Interface)]
                mshtml.IHTMLEventObj eventObj
            );

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int PostHandleEvent(
            [In] int dispId,
            [In, MarshalAs(UnmanagedType.Interface)]
                mshtml.IHTMLEventObj eventObj
            );

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int TranslateAccelerator(
            [In] int dispId,
            [In, MarshalAs(UnmanagedType.Interface)]
                mshtml.IHTMLEventObj eventObj
            );

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int PostEditorEventNotify(
            [In] int dispId,
            [In, MarshalAs(UnmanagedType.Interface)]
                mshtml.IHTMLEventObj eventObj
            );

    }

    [Guid("3050f663-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IHTMLEditServices
    {

        [return: MarshalAs(UnmanagedType.I4)]
        int AddDesigner(
            [In, MarshalAs(UnmanagedType.Interface)]
                IHTMLEditDesigner designer);

        [return: MarshalAs(UnmanagedType.Interface)]
        int GetSelectionServices(
           [In, MarshalAs(UnmanagedType.Interface)]
                mshtml.IMarkupContainer markupContainer,
           [Out, MarshalAs(UnmanagedType.Interface)]
                out IntPtr ss);

        [return: MarshalAs(UnmanagedType.I4)]
        int MoveToSelectionAnchor(
            [In, MarshalAs(UnmanagedType.Interface)]
                object markupPointer);

        [return: MarshalAs(UnmanagedType.I4)]
        int MoveToSelectionEnd(
            [In, MarshalAs(UnmanagedType.Interface)]
                object markupPointer);

        [return: MarshalAs(UnmanagedType.I4)]
        int RemoveDesigner(
            [In, MarshalAs(UnmanagedType.Interface)]
                IHTMLEditDesigner designer);
    }
}

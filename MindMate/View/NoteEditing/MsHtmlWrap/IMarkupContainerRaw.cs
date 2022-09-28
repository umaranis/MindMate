using System.Runtime.InteropServices;
using mshtml;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    /// <summary>
    /// Interface used for customizing editing behavior of MSHTML
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050f5f9-98b5-11cf-bb82-00aa00bdce0b")]
    public interface IMarkupContainerRaw
    {
        void OwningDoc(
            [Out] out IHTMLDocument2 ppDoc);
    }
}
using System;
using System.Runtime.InteropServices;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    [ComVisible(true), ComImport(), Guid("6d5140c1-7436-11ce-8034-00aa006009fa"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface UCOMIServiceProvider
    {
        IntPtr QueryService(ref Guid guidService, ref Guid riid);
    }
        
    [Guid("D001F200-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleUndoManager
    {
        void Open(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleParentUndoUnit parentUndo);

        void Close(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleParentUndoUnit parentUndo,
            [In, MarshalAs(UnmanagedType.Bool)]
                bool fCommit);

        void Add(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleUndoUnit undoUnit);

        [return: MarshalAs(UnmanagedType.I8)]
        long GetOpenParentState();

        void DiscardFrom(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleUndoUnit undoUnit);

        void UndoTo(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleUndoUnit undoUnit);

        void RedoTo(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleUndoUnit undoUnit);

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumOleUndoUnits EnumUndoable();

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumOleUndoUnits EnumRedoable();

        [return: MarshalAs(UnmanagedType.BStr)]
        string GetLastUndoDescription();

        [return: MarshalAs(UnmanagedType.BStr)]
        string GetLastRedoDescription();

        void Enable(
            [In, MarshalAs(UnmanagedType.Bool)]
                bool fEnable);

    }

    [Guid("894AD3B0-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleUndoUnit
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Do(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleUndoManager undoManager);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetDescription(
            [Out, MarshalAs(UnmanagedType.BStr)]
                out string bStr);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetUnitType(
            [Out, MarshalAs(UnmanagedType.I4)]
                out int clsid,
            [Out, MarshalAs(UnmanagedType.I4)]
                out int plID);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int OnNextAdd();
    }

    [Guid("B3E7C340-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumOleUndoUnits
    {

        //[PreserveSig]
        void Next(
            [In, MarshalAs(UnmanagedType.U4)] int numDesired,
            [Out, MarshalAs(UnmanagedType.Interface)] out IOleUndoUnit unit,
            [Out, MarshalAs(UnmanagedType.U4)] out int numReceived);

        //void Bogus();

        //[PreserveSig]
        void Skip(
            [In, MarshalAs(UnmanagedType.I4)] int numToSkip);

        //[PreserveSig]
        void Reset();

        //[PreserveSig]
        void Clone(
            [Out, MarshalAs(UnmanagedType.Interface)] IEnumOleUndoUnits enumerator);
    }

    [Guid("A1FAF330-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleParentUndoUnit //: IOleUndoUnit 
    {
        #region IOleUndoUnit
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Do(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleUndoManager undoManager);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetDescription(
            [Out, MarshalAs(UnmanagedType.BStr)]
                out string bStr);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetUnitType(
            [Out, MarshalAs(UnmanagedType.I4)]
                out int clsid,
            [Out, MarshalAs(UnmanagedType.I4)]
                out int plID);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int OnNextAdd();
        #endregion
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Open(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleParentUndoUnit parentUnit);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Close(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleParentUndoUnit parentUnit,
            [In, MarshalAs(UnmanagedType.Bool)]
                bool fCommit);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Add(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleUndoUnit undoUnit);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int FindUnit(
            [In, MarshalAs(UnmanagedType.Interface)]
                IOleUndoUnit undoUnit);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetParentState(
            [Out, MarshalAs(UnmanagedType.I8)]
                out long state
            );
    }

    [ComImport(), Guid("6D5140C1-7436-11CE-8034-00AA006009FA"),
            InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleServiceProvider
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryService(
            [In] ref Guid guidService,
            [In] ref Guid riid,
            out IntPtr ppvObject);
    }
    
}

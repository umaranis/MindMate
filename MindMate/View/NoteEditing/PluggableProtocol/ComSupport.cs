using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MindMate.View.NoteEditing.PluggableProtocol
{

    public struct _LARGE_INTEGER
    {
        public Int64 QuadPart;
    }

    public struct _ULARGE_INTEGER
    {
        public UInt64 QuadPart;
    }

    public struct _tagPROTOCOLDATA
    {
        public uint grfFlags;
        public uint dwState;
        public IntPtr pData;
        public uint cbData;
    }

    public struct BINDINFO
    {
        public UInt32 cbSize;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szExtraInfo;
        public STGMEDIUM stgmedData;
        public UInt32 grfBindInfoF;
        [MarshalAs(UnmanagedType.U4)]
        public BINDVERB dwBindVerb;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szCustomVerb;
        public UInt32 cbStgmedData;
        public UInt32 dwOptions;
        public UInt32 dwOptionsFlags;
        public UInt32 dwCodePage;
        public SECURITY_ATTRIBUTES securityAttributes;
        public Guid iid;
        [MarshalAs(UnmanagedType.IUnknown)]
        public object pUnk;
        public UInt32 dwReserved;
    }

    public struct STGMEDIUM
    {
        [MarshalAs(UnmanagedType.U4)]
        public TYMED enumType;
        public IntPtr u;
        [MarshalAs(UnmanagedType.IUnknown)]
        public object pUnkForRelease;
    }

    public struct SECURITY_ATTRIBUTES
    {
        public UInt32 nLength;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }

    public enum TYMED : uint //Should be UInt32 but you can only use C# keywords here.
    {
        TYMED_HGLOBAL = 1,
        TYMED_FILE = 2,
        TYMED_ISTREAM = 4,
        TYMED_ISTORAGE = 8,
        TYMED_GDI = 16,
        TYMED_MFPICT = 32,
        TYMED_ENHMF = 64,
        TYMED_NULL = 0
    }

    public enum BINDVERB : uint
    {
        BINDVERB_GET = 0,
        BINDVERB_POST = 1,
        BINDVERB_PUT = 2,
        BINDVERB_CUSTOM = 3,
    }


    public enum PARSEACTION
    {
        PARSE_CANONICALIZE = 1,
        PARSE_FRIENDLY = PARSE_CANONICALIZE + 1,
        PARSE_SECURITY_URL = PARSE_FRIENDLY + 1,
        PARSE_ROOTDOCUMENT = PARSE_SECURITY_URL + 1,
        PARSE_DOCUMENT = PARSE_ROOTDOCUMENT + 1,
        PARSE_ANCHOR = PARSE_DOCUMENT + 1,
        PARSE_ENCODE = PARSE_ANCHOR + 1,
        PARSE_DECODE = PARSE_ENCODE + 1,
        PARSE_PATH_FROM_URL = PARSE_DECODE + 1,
        PARSE_URL_FROM_PATH = PARSE_PATH_FROM_URL + 1,
        PARSE_MIME = PARSE_URL_FROM_PATH + 1,
        PARSE_SERVER = PARSE_MIME + 1,
        PARSE_SCHEMA = PARSE_SERVER + 1,
        PARSE_SITE = PARSE_SCHEMA + 1,
        PARSE_DOMAIN = PARSE_SITE + 1,
        PARSE_LOCATION = PARSE_DOMAIN + 1,
        PARSE_SECURITY_DOMAIN = PARSE_LOCATION + 1,
        PARSE_ESCAPE = PARSE_SECURITY_DOMAIN + 1,
        PARSE_UNESCAPE = PARSE_ESCAPE + 1,
    }

    public enum QUERYOPTION
    {
        QUERY_EXPIRATION_DATE = 1,
        QUERY_TIME_OF_LAST_CHANGE = QUERY_EXPIRATION_DATE + 1,
        QUERY_CONTENT_ENCODING = QUERY_TIME_OF_LAST_CHANGE + 1,
        QUERY_CONTENT_TYPE = QUERY_CONTENT_ENCODING + 1,
        QUERY_REFRESH = QUERY_CONTENT_TYPE + 1,
        QUERY_RECOMBINE = QUERY_REFRESH + 1,
        QUERY_CAN_NAVIGATE = QUERY_RECOMBINE + 1,
        QUERY_USES_NETWORK = QUERY_CAN_NAVIGATE + 1,
        QUERY_IS_CACHED = QUERY_USES_NETWORK + 1,
        QUERY_IS_INSTALLEDENTRY = QUERY_IS_CACHED + 1,
        QUERY_IS_CACHED_OR_MAPPED = QUERY_IS_INSTALLEDENTRY + 1,
        QUERY_USES_CACHE = QUERY_IS_CACHED_OR_MAPPED + 1,
        QUERY_IS_SECURE = QUERY_USES_CACHE + 1,
        QUERY_IS_SAFE = QUERY_IS_SECURE + 1,
    }

    public enum BSCF : uint
    {
        BSCF_FIRSTDATANOTIFICATION = 0,
        BSCF_INTERMEDIATEDATANOTIFICATION = 1,
        BSCF_LASTDATANOTIFICATION = 2,
        BSCF_DATAFULLYAVAILABLE = 3,
        BSCF_AVAILABLEDATASIZEUNKNOWN = 4,
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("79EAC9E1-BAF9-11CE-8C82-00AA004BA90B")]
    public interface IInternetBindInfo
    {
        void GetBindInfo(out UInt32 grfBINDF, [In, Out] ref BINDINFO pbindinfo);
        void GetBindString(UInt32 ulStringType, [MarshalAs(UnmanagedType.LPWStr)] ref string ppwzStr, UInt32 cEl, ref UInt32 pcElFetched);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("79EAC9E5-BAF9-11CE-8C82-00AA004BA90B")]
    public interface IInternetProtocolSink
    {
        void Switch(ref _tagPROTOCOLDATA pProtocolData);
        void ReportProgress(UInt32 ulStatusCode, [MarshalAs(UnmanagedType.LPWStr)] string szStatusText);
        void ReportData(BSCF grfBSCF, UInt32 ulProgress, UInt32 ulProgressMax);
        void ReportResult(Int32 hrResult, UInt32 dwError, [MarshalAs(UnmanagedType.LPWStr)] string szResult);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(Guids.IID_IInternetProtocol)]
    [ComVisible(true)]
    public interface IInternetProtocol
    {
        //IInternetProtcolRoot
        void Start(
            [MarshalAs(UnmanagedType.LPWStr)] string url,
           IInternetProtocolSink Sink,
           IInternetBindInfo pOIBindInfo,
           UInt32 grfPI,
           UInt32 dwReserved);
        void Continue(ref _tagPROTOCOLDATA pProtocolData);
        void Abort(Int32 hrReason, UInt32 dwOptions);
        void Terminate(UInt32 dwOptions);
        void Suspend();
        void Resume();
        //IInternetProtocol
        [PreserveSig()]
        UInt32 Read(IntPtr pv, UInt32 cb, out UInt32 pcbRead);
        void Seek(_LARGE_INTEGER dlibMove, UInt32 dwOrigin, out _ULARGE_INTEGER plibNewPosition);
        void LockRequest(UInt32 dwOptions);
        void UnlockRequest();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(Guids.IID_IInternetProtocolRoot)]
    public interface IInternetProtocolRoot
    {
        void Start(
            [MarshalAs(UnmanagedType.LPWStr)]string url,
           IInternetProtocolSink Sink,
           IInternetBindInfo pOIBindInfo,
           UInt32 grfPI,
           UInt32 dwReserved);
        void Continue(ref _tagPROTOCOLDATA pProtocolData);
        void Abort(Int32 hrReason, UInt32 dwOptions);
        void Terminate(UInt32 dwOptions);
        void Suspend();
        void Resume();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("79eac9d2-baf9-11ce-8c82-00aa004ba90b")]
    public interface IHttpNegotiate
    {
        void BeginningTransaction(
            [MarshalAs(UnmanagedType.LPWStr)]string szURL,
           [MarshalAs(UnmanagedType.LPWStr)]string szHeaders,
          UInt32 dwReserved,
          [MarshalAs(UnmanagedType.LPWStr)] out string szAdditionalHeaders);
        int OnResponse(
            UInt32 dwResponseCode,
            [MarshalAs(UnmanagedType.LPWStr)]string szResponseHeaders,
           [MarshalAs(UnmanagedType.LPWStr)]string szRequestHeaders,
          [MarshalAs(UnmanagedType.LPWStr)]out string szAdditionalRequestHeaders);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
    public interface IServiceProvider
    {
        void QueryService(
            [In] ref System.Guid guidService,
            [In] ref System.Guid guidType,
            [Out, MarshalAs(UnmanagedType.Interface)] out object Object);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(Guids.IID_IInternetProtocolInfo)]
    [ComVisible(true)]
    public interface IInternetProtocolInfo
    {
        [PreserveSig()]
        UInt32 ParseUrl(
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
            /* [in] */ PARSEACTION ParseAction,
               UInt32 dwParseFlags,
               IntPtr pwzResult,
               UInt32 cchResult,
               out UInt32 pcchResult,
               UInt32 dwReserved);

        [PreserveSig()]
        UInt32 CombineUrl(
            [MarshalAs(UnmanagedType.LPWStr)] string pwzBaseUrl,
           [MarshalAs(UnmanagedType.LPWStr)] string pwzRelativeUrl,
          UInt32 dwCombineFlags,
          IntPtr pwzResult,
          UInt32 cchResult,
          out UInt32 pcchResult,
          UInt32 dwReserved);

        [PreserveSig()]
        UInt32 CompareUrl(
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl1,
           [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl2,
          UInt32 dwCompareFlags);

        [PreserveSig()]
        UInt32 QueryInfo(
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
           QUERYOPTION OueryOption,
           UInt32 dwQueryFlags,
           IntPtr pBuffer,
           UInt32 cbBuffer,
           ref UInt32 pcbBuf,
           UInt32 dwReserved);
    };

    public class HRESULT
    {
        public static UInt32 S_OK = 0;
        public static UInt32 S_FALSE = 1;
        public static UInt32 INET_E_DEFAULT_ACTION = 0x800C0011;
    }

    [ComVisible(false)]
    public class Guids
    {
        public static Guid IID_IHttpNegotiate = new Guid("79eac9d2-baf9-11ce-8c82-00aa004ba90b");
        public const string IID_IInternetProtocolInfo = "79eac9ec-baf9-11ce-8c82-00aa004ba90b";
        public const string IID_IInternetProtocolRoot = "79EAC9E3-BAF9-11CE-8C82-00AA004BA90B";
        public const string IID_IInternetProtocol = "79eac9e4-baf9-11ce-8c82-00aa004ba90b";
        public const string IID_IUnknown = "00000000-0000-0000-C000-000000000046";
    }

    /* Make these constants */
    public enum MIIM : uint
    {
        STATE = 0x00000001,
        ID = 0x00000002,
        SUBMENU = 0x00000004,
        CHECKMARKS = 0x00000008,
        TYPE = 0x00000010,
        DATA = 0x00000020,
        STRING = 0x00000040,
        BITMAP = 0x00000080,
        FTYPE = 0x00000100
    }


    // GetCommandString uFlags
    public enum GCS : uint
    {
        VERBA = 0x00000000,     // canonical verb
        HELPTEXTA = 0x00000001,     // help text (for status bar)
        VALIDATEA = 0x00000002,     // validate command exists
        VERBW = 0x00000004,     // canonical verb (unicode)
        HELPTEXTW = 0x00000005,     // help text (unicode version)
        VALIDATEW = 0x00000006,     // validate command exists (unicode)
        UNICODE = 0x00000004,     // for bit testing - Unicode string
        VERB = GCS.VERBA,
        HELPTEXT = GCS.HELPTEXTA,
        VALIDATE = GCS.VALIDATEA
    }


    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("79eac9e7-baf9-11ce-8c82-00aa004ba90b")]
    public interface IInternetSession
    {
        int RegisterNameSpace([MarshalAs(UnmanagedType.Interface)] IClassFactory pCF,
                               ref Guid rclsid,
                               [MarshalAs(UnmanagedType.LPWStr)] string pwzProtocol,
                               uint cPatterns,
                               [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[]
                                   ppwzPatterns,
                               uint dwReserved);

        void UnregisterNameSpace(IntPtr pCF, [MarshalAs(UnmanagedType.LPWStr)] string pszProtocol);
        void RegisterMimeFilter(IntPtr pCF, ref Guid rclsid, [MarshalAs(UnmanagedType.LPWStr)] string pwzType);
        void UnregisterMimeFilter(IntPtr pCF, [MarshalAs(UnmanagedType.LPWStr)] string pwzType);
        void CreateBinding(IntPtr pBC, [MarshalAs(UnmanagedType.LPWStr)] string szUrl, IntPtr pUnkOuter, IntPtr ppUnk, IInternetProtocol ppOInetProt, uint dwOption);
        void SetSessionOption(uint dwOption, IntPtr pBuffer, uint dwBufferLength, uint dwReserved);
        void GetSessionOption(uint dwOption, IntPtr pBuffer, uint pdwBufferLength, uint dwReserved);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000001-0000-0000-C000-000000000046")]
    public interface IClassFactory
    {
        void CreateInstance(IntPtr pUnkOuter,
                             ref Guid riid,
                             out IntPtr ppvObject);
        void LockServer(bool fLock);
    }

}
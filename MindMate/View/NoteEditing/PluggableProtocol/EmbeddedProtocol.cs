using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MindMate.View.NoteEditing.PluggableProtocol
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.None)]
    public abstract class EmbeddedProtocol :
        ProtocolBase,
        IInternetProtocol,
        IInternetProtocolRoot,
        IInternetProtocolInfo
    {
        public string ProtocolPrefix { get; private set; }
        public string GUID { get; set; }
        public string RootPath { get; set; }

        public EmbeddedProtocol(string protocolPrefix, string rootPath, string guid = null)
        {
            ProtocolPrefix = protocolPrefix;
            GUID = guid ?? EmbeddedProtocolFactory.GetClassGuid(this.GetType());
            RootPath = rootPath;
        }

        public abstract byte[] GetUrlData(string url, out string contentType);

        #region Start(string url, IInternetProtocolSink sink, IInternetBindInfo pOIBindInfo, uint grfPI, uint dwReserved)
        public void Start(string url, IInternetProtocolSink sink, IInternetBindInfo pOIBindInfo, uint grfPI, uint dwReserved)
        {
            //var bindInfo = GetBindInfo(pOIBindInfo);
            var neg = GetHttpNegotiate(sink);
            string requestHeaders;
            neg.BeginningTransaction(url, string.Empty, 0, out requestHeaders);
            Debug.WriteLine(requestHeaders);

            string contentType;
            var data = this.GetUrlData(url, out contentType);
            if (data != null)
            {
                //var buff = new byte[responseStream.Length];
                //responseStream.Read(buff, 0, buff.Length);
                //this.Stream.Position = 0;
                //this.Stream.Write(buff, 0, buff.Length);
                this.Data = data;
            }
            else
            {
                Data = Encoding.UTF8.GetBytes("Page not found!");                
            }
            
            var responseHeaders = string.Format("HTTP/1.1 200 OK\r\n" +
                                                "Content-Type: {0}\r\n" +
                                                "Content-Length:{1}\r\n\r\n", contentType, Data.Length);
            string newResponseHeaders;
            var hresult = neg.OnResponse(200, responseHeaders, requestHeaders, out newResponseHeaders);
            if (hresult != 0)
            {
                Debug.Write("IHttpNegotiate::OnResponse result = " + hresult);
            }
            sink.ReportData(BSCF.BSCF_LASTDATANOTIFICATION, (uint)Data.Length, (uint)Data.Length);
            sink.ReportResult(0, 200, null);
        }
        #endregion

        #region ParseUrl(string pwzUrl, PARSEACTION ParseAction, uint dwParseFlags, IntPtr pwzResult, uint cchResult, out uint pcchResult, uint dwReserved)
        public uint ParseUrl(string pwzUrl, PARSEACTION ParseAction, uint dwParseFlags, IntPtr pwzResult, uint cchResult, out uint pcchResult, uint dwReserved)
        {
            pcchResult = 0;
            return HRESULT.INET_E_DEFAULT_ACTION;
        }
        #endregion

        public string MakeUrlAbsolute(string baseUrl, string relativeUrl)
        {
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            string temp;
            if (relativeUrl.IndexOf(":") > 0)
            {
                temp = relativeUrl;
            }
            else if (relativeUrl.StartsWith("/"))
            {
                temp = ProtocolPrefix + ":" + this.RootPath + relativeUrl.Substring(1);
            }
            else if (relativeUrl.StartsWith("#"))
            {
                var sharpPos = baseUrl.IndexOf("#");
                temp = (sharpPos >= 0
                            ? baseUrl.Substring(sharpPos + 1)
                            : baseUrl)
                       + relativeUrl;
            }
            else if (!baseUrl.Contains("/"))
            {
                temp = ProtocolPrefix + ":" + relativeUrl;
            }
            else
            {
                int fileNameFromIndex = baseUrl.LastIndexOf("/");
                temp = baseUrl.Substring(0, fileNameFromIndex + 1) + relativeUrl;
            }

            // normalize url
            int pos;
            while ((pos = temp.IndexOf("/../")) > 0)
            {
                var before = temp.LastIndexOf('/', pos - 1);
                if (before <= 0) before = pos;
                temp = temp.Substring(0, before) + temp.Substring(pos + 3);
            }
            return temp;
        }

        public uint CombineUrl(string baseUrl, string relativeUrl, uint dwCombineFlags, IntPtr pwzResult, uint cchResult, out uint pcchResult, uint dwReserved)
        {
            var temp = MakeUrlAbsolute(baseUrl, relativeUrl);
            if (temp.Length > cchResult)
            {
                pcchResult = 0;
                return HRESULT.S_FALSE;
            }
            Marshal.Copy(temp.ToCharArray(), 0, pwzResult, temp.Length);
            Marshal.WriteInt32(pwzResult, temp.Length * 2, 0);
            pcchResult = (UInt32)temp.Length + 1;
            return HRESULT.S_OK;
        }

        #region CompareUrl(string pwzUrl1, string pwzUrl2, uint dwCompareFlags)
        public uint CompareUrl(string pwzUrl1, string pwzUrl2, uint dwCompareFlags)
        {
            return (UInt32)pwzUrl1.CompareTo(pwzUrl2);
        }
        #endregion

        #region QueryInfo(string pwzUrl, QUERYOPTION OueryOption, uint dwQueryFlags, IntPtr pBuffer, uint cbBuffer, ref uint pcbBuf, uint dwReserved)
        public uint QueryInfo(string pwzUrl, QUERYOPTION OueryOption, uint dwQueryFlags, IntPtr pBuffer, uint cbBuffer, ref uint pcbBuf, uint dwReserved)
        {
            return HRESULT.INET_E_DEFAULT_ACTION;
        }
        #endregion
    }

    public class EmbeddedProtocolFactory : IClassFactory
    {
        private const int CLASS_E_NOAGGREGATION = unchecked((int)0x80040110);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private Func<EmbeddedProtocol> createProtocolInstance;

        public EmbeddedProtocolFactory(Func<EmbeddedProtocol> createProtocolInstance)
        {
            this.createProtocolInstance = createProtocolInstance;
        }


#line hidden // avoid debugger to break on exception during Marshal.ThrowExceptionForHR

        public void CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
        {
            if (pUnkOuter != IntPtr.Zero)
            {
                Marshal.ThrowExceptionForHR(CLASS_E_NOAGGREGATION);
            }
            if (riid == new Guid(Guids.IID_IInternetProtocol))
            {
                var protocol = this.createProtocolInstance();
                ppvObject = Marshal.GetComInterfaceForObject(protocol, typeof(IInternetProtocol));
            }
            else if (riid == new Guid(Guids.IID_IInternetProtocolInfo))
            {
                var protocol = this.createProtocolInstance();
                ppvObject = Marshal.GetComInterfaceForObject(protocol, typeof(IInternetProtocolInfo));
            }
            else if (riid == new Guid(Guids.IID_IInternetProtocolRoot))
            {
                var protocol = this.createProtocolInstance();
                ppvObject = Marshal.GetComInterfaceForObject(protocol, typeof(IInternetProtocolRoot));
            }
            else
            {
                Marshal.ThrowExceptionForHR(E_NOINTERFACE);
                ppvObject = IntPtr.Zero;
            }
        }

#line default

        public void LockServer(bool fLock)
        {
        }

        public static void Register<T>(string prefix, Func<T> createProtocol) where T : EmbeddedProtocol
        {
            Register(prefix, () => createProtocol(), new Guid(GetClassGuid(typeof(T))));
        }

        public static void Register(string prefix, Func<EmbeddedProtocol> createProtocol, Guid guid)
        {
            IInternetSession session = null;
            CoInternetGetSession(0, ref session, 0);
            var hresult = session.RegisterNameSpace(new EmbeddedProtocolFactory(createProtocol), ref guid, prefix, 0, null, 0);
            if (hresult != 0)
            {
                throw new Win32Exception(hresult);
            }
        }

        public static string GetClassGuid(Type type)
        {
            var guidAttrs = type.GetCustomAttributes(typeof(GuidAttribute), false);
            if (guidAttrs.Length == 0)
            {
                throw new ApplicationException(string.Format("Plase add a Guid attribute to the class {0}!", type.FullName));
            }
            return ((GuidAttribute)guidAttrs[0]).Value;

        }

        [DllImport("urlmon.dll")]
        private static extern void CoInternetGetSession(uint dwSession, ref IInternetSession ifSession, uint dwReserved);

    }


}

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MindMate.View.NoteEditing.PluggableProtocol
{

    [ComVisible(false)]
    public class ProtocolBase
    {
        public void Resume()
        {
            Debug.WriteLine("Resume");
        }

        public void Terminate(uint dwOptions)
        {
            Debug.WriteLine("Terminate");
        }

        public void Seek(_LARGE_INTEGER dlibMove, uint dwOrigin, out _ULARGE_INTEGER plibNewPosition)
        {
            Debug.WriteLine("Seek");
            plibNewPosition = new _ULARGE_INTEGER();
        }

        public void LockRequest(uint dwOptions)
        {
            Debug.WriteLine("LockRequest");
        }

        public void UnlockRequest()
        {
            Debug.WriteLine("UnlockRequest");
        }

        public void Abort(int hrReason, uint dwOptions)
        {
            Debug.WriteLine("Abort");
        }

        public void Suspend()
        {
            Debug.WriteLine("Suspend");
        }

        public void Continue(ref _tagPROTOCOLDATA pProtocolData)
        {
            Debug.WriteLine("Continue");
        }

        const int S_OK = 0;
        const int S_FALSE = 1;

        public UInt32 Read(IntPtr pv, uint cb, out uint pcbRead)
        {
            pcbRead = (uint)Math.Min(cb, RemainingDataLength);
            Marshal.Copy(data, position, pv, (int)pcbRead);
            position += (int)pcbRead;

            UInt32 response = (pcbRead == 0) ? (UInt32)S_FALSE : (UInt32)S_OK;
            return response;
        }

        public static IHttpNegotiate GetHttpNegotiate(IInternetProtocolSink sink)
        {
            if ((sink is IServiceProvider) == false)
                throw new Exception("Error ProtocolSink does not support IServiceProvider.");
            Debug.WriteLine("ServiceProvider");

            var provider = (IServiceProvider)sink;
            object objNegotiate;
            provider.QueryService(ref Guids.IID_IHttpNegotiate, ref Guids.IID_IHttpNegotiate, out objNegotiate);
            return (IHttpNegotiate)objNegotiate;
        }

        public static BINDINFO GetBindInfo(IInternetBindInfo pOIBindInfo)
        {
            BINDINFO BindInfo = new BINDINFO();
            BindInfo.cbSize = (UInt32)Marshal.SizeOf(typeof(BINDINFO));
            UInt32 AsyncFlag;
            pOIBindInfo.GetBindInfo(out AsyncFlag, ref BindInfo);
            return BindInfo;
        }

        private byte[] data;
        protected byte[] Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                position = 0;
            }
        }

        private int position = 0;

        private int RemainingDataLength
        {
            get
            {
                return data.Length - position;
            }
        } 
    }


}

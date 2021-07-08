using System;
using System.Net;

namespace TCP
{
    public delegate void ReceiveHandler(object sender, TCPReceiveEventArgs e);

    public class TCPReceiveEventArgs : EventArgs
    {
        public object Content { get; set; }
        public string IP { get; private set; }
        public IPEndPoint RemotePoint { get; private set; }

        public TCPReceiveEventArgs(string ip)
        {
            this.IP = ip;
        }

        public TCPReceiveEventArgs(object content, string ip, IPEndPoint remotePoint)
        {
            this.Content = content;
            this.IP = ip;
            this.RemotePoint = remotePoint;
        }
    }
}
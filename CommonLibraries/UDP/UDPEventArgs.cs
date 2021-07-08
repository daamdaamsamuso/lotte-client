using System;

namespace UDP
{
    public delegate void ReceiveHandler(object sender, UDPEventArgs e);

    public class UDPEventArgs : EventArgs
    {
        public byte[] Bytes;

        public UDPEventArgs(byte[] bytes)
        {
            this.Bytes = bytes;
        }
    }
}
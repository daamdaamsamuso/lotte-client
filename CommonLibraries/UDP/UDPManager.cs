using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Serializer;

namespace UDP
{
    public class UDPManager
    {
        #region Variable

        private UdpClient _client;
        private UdpClient _server;
        private IPEndPoint _clientEndPoint;
        private IPEndPoint _serverEndPoint;
        private IPAddress _ipAddr;
        private Thread _serverThread;

        private bool _run;
        private bool _multicast;
         
        #endregion

        #region Event

        public event ReceiveHandler Received;

        #endregion

        #region Constuructor

        public UDPManager()
        {
            CreateClient();
        }

        public UDPManager(string serverIP, int serverPort, bool multicast)
        {
            this._ipAddr = IPAddress.Parse(serverIP);
            this._clientEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            this._multicast = multicast;

            CreateClient();
        }

        #endregion

        #region Public Method

        public int SendMulticast(object obj)
        {
            var bytes = ObjectSerializer.ObjectToByteArray(obj);
            return SendMulticast(bytes);
        }

        public int SendMulticast(byte[] bytes)
        {
            return this._client.Send(bytes, bytes.Length, this._clientEndPoint);
        }

        public int Send(object obj)
        {
            var bytes = ObjectSerializer.ObjectToByteArray(obj);
            return Send(bytes);
        }

        public int Send(string serverIP, int serverPort, object obj)
        {
            var bytes = ObjectSerializer.ObjectToByteArray(obj);
            return Send(serverIP, serverPort, bytes);
        }

        public int Send(byte[] bytes)
        {
            return this._client.Send(bytes, bytes.Length, this._clientEndPoint);
        }

        public int Send(string serverIP, int serverPort, byte[] bytes)
        {
            return this._client.Send(bytes, bytes.Length, serverIP, serverPort);
        }

        public bool StartServer(int receivePort)
        {
            if (this._serverThread != null) return false;

            this._server = new UdpClient(receivePort);
            this._serverEndPoint = new IPEndPoint(IPAddress.Loopback, receivePort);

            this._run = true;
            this._serverThread = new Thread(new ThreadStart(Receive));
            this._serverThread.IsBackground = true;
            this._serverThread.Start();

            return true;
        }

        public bool StartMulticastServer(string receiveIP, int receivePort)
        {
            if (this._serverThread != null) return false;

            this._ipAddr = IPAddress.Parse(receiveIP);

            this._server = new UdpClient();
            this._server.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this._server.Client.Bind(new IPEndPoint(IPAddress.Any, receivePort));
            this._server.JoinMulticastGroup(this._ipAddr);

            this._serverEndPoint = new IPEndPoint(IPAddress.Any, receivePort);

            this._run = true;
            this._serverThread = new Thread(new ThreadStart(Receive));
            this._serverThread.IsBackground = true;
            this._serverThread.Start();

            return true;
        }

        public void StopServer()
        {
            this._run = false;

            try
            {
                if (this._ipAddr != null && this._multicast) this._server.DropMulticastGroup(this._ipAddr);
                if (this._server != null) this._server.Close();
                if (this._serverThread != null) this._serverThread.Abort();
            }
            catch { }
            finally
            {
                this._ipAddr = null;
                this._server = null;
                this._serverThread = null;
            }
        }

        public void CreateClient()
        {
            if (this._client != null) return;

            this._client = new UdpClient();

            if (this._multicast && this._ipAddr != null)
            {
                this._client.JoinMulticastGroup(this._ipAddr);
            }
        }

        public void CloseClient()
        {
            try
            {
                if (this._client != null)
                {
                    if (this._multicast && this._ipAddr != null)
                    {
                        this._client.DropMulticastGroup(this._ipAddr);
                    }

                    this._client.Close();
                }
            }
            catch { }
            finally
            {
                this._client = null;
            }
        }

        public void Clean()
        {
            StopServer();
            CloseClient();
        }

        #endregion

        #region Private Method

        private void Receive()
        {
            try
            {
                while (this._run)
                {
                    var bytes = this._server.Receive(ref this._serverEndPoint);

                    var OnReceived = Received;
                    if (OnReceived != null)
                    {
                        OnReceived(this, new UDPEventArgs(bytes));
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#elif TRACE
                string message = string.Format("{0:yyyy-MM-dd HH:mm:ss}\t\t{1}", DateTime.Now, ex.ToString());
                System.Diagnostics.Trace.WriteLine(message);
#endif
            }

        }

        #endregion


    }
}
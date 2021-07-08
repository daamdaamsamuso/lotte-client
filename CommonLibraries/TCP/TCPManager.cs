using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Serializer;

namespace TCP
{
    public class TCPManager
    {
        #region Variable

        private TcpClient _client;
        private TcpListener _listener;
        private Thread _listenThread;
        private NetworkStream _networkStream;
        private BinaryWriter _binaryWriter;

        private static int _connectionCount;

        #endregion

        #region Property

        public string ServerIP { get; private set; }
        public int ServerPort { get; private set; }
        public int ReceivePort { get; private set; }
        public int ConnectionCount { get { return _connectionCount; } }

        #endregion

        #region Event

        public event ReceiveHandler Received;

        #endregion

        #region Constructor

        public TCPManager()
        {
            this._client = new TcpClient();
        }

        public TCPManager(string serverIP, int serverPort)
            : this()
        {
            this.ServerIP = serverIP;
            this.ServerPort = serverPort;
        }

        #endregion

        #region Public Method

        public void StartServer(int receivePort)
        {
            if (this._listenThread != null) return;

            this.ReceivePort = receivePort;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, receivePort);

            this._listener = new TcpListener(localEndPoint);
            this._listener.Start();

            this._listenThread = new Thread(new ThreadStart(Listen));
            this._listenThread.Start();
        }

        public void StopServer()
        {
            if (this._listener != null) this._listener.Stop();
            if (this._listenThread != null) this._listenThread.Abort();

            this._listener = null;
            this._listenThread = null;
        }

        public void Clean()
        {
            StopServer();
            Close();
        }

        public void Close()
        {
            if (this._binaryWriter != null) this._binaryWriter.Close();
            if (this._networkStream != null) this._networkStream.Close();
            if (this._client != null) this._client.Close();

            this._binaryWriter = null;
            this._networkStream = null;
            this._client = null;
        }

        public bool Send(object obj)
        {
            var bytes = ObjectSerializer.ObjectToByteArray(obj);
            return Send(bytes);
        }

        public bool Send(string serverIP, int serverPort, object obj)
        {
            var bytes = ObjectSerializer.ObjectToByteArray(obj);
            return Send(serverIP, serverPort, bytes);
        }

        public bool Send(byte[] bytes)
        {
            return Send(this.ServerIP, this.ServerPort, bytes);
        }

        public bool Send(string serverIP, int serverPort, byte[] bytes)
        {
            bool connected = false;

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var ar = client.BeginConnect(serverIP, serverPort, null, null);
                    connected = ar.AsyncWaitHandle.WaitOne(100);

                    if (connected && client.Client != null)
                    {
                        using (var stream = client.GetStream())
                        {
                            using (var binaryWriter = new BinaryWriter(stream))
                            {
                                binaryWriter.Write(bytes.Length);
                                binaryWriter.Write(bytes);
                                binaryWriter.Flush();
                                binaryWriter.Close();
                            }

                            stream.Close();
                        }
                    }
                    else
                    {
                        connected = false;
                    }

                    client.Close();
                }
            }
            catch { }

            return connected;
        }

        #endregion

        #region Private Method

        private void Listen()
        {
            while (true)
            {
                try
                {
                    var acceptClient = this._listener.AcceptTcpClient();
                    AcceptClientThread connection = new AcceptClientThread(this, acceptClient);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(connection.Run));
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
#endif
                    break;
                }
            }
        }

        #endregion

        private class AcceptClientThread
        {
            #region Property

            public TCPManager Manager { get; private set; }
            public TcpClient Client { get; private set; }

            #endregion

            #region Variable

            private NetworkStream _networkStream;
            private BinaryReader _binaryReader;

            #endregion

            public AcceptClientThread(TCPManager manager, TcpClient client)
            {
                this.Manager = manager;
                this.Client = client;
            }

            public void Run(object state)
            {
                TCPManager._connectionCount++;

                var endPoint = this.Client.Client.RemoteEndPoint as IPEndPoint;
                string clientIP = endPoint.Address.ToString();

#if DEBUG
                System.Diagnostics.Debug.WriteLine("connected : " + clientIP);
#endif

                this._networkStream = this.Client.GetStream();
                this._binaryReader = new BinaryReader(this._networkStream);

                byte[] buffer;
                int byteLength;
                object content;

                while (true)
                {
                    try
                    {
                        byteLength = this._binaryReader.ReadInt32();
                        buffer = this._binaryReader.ReadBytes(byteLength);

                        content = ObjectSerializer.ByteArrayToObject(buffer);

                        if(content == null)
                        {
                            content = Encoding.Default.GetString(buffer);
                        }

                        this.Manager.Received(this.Manager, new TCPReceiveEventArgs(content, clientIP, endPoint));
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine(ex.Message);
#endif
                        Close();
                        break;
                    }
                }
            }

            public void Close()
            {
                _connectionCount--;

                if (this._binaryReader != null) this._binaryReader.Close();
                if (this._networkStream != null) this._networkStream.Close();
                if (this.Client != null) this.Client.Close();

                this._binaryReader = null;
                this._networkStream = null;
                this.Client = null;
            }
        }
    }
}
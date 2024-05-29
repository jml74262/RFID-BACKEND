using System;
using System.Net.Sockets;

namespace PrinterBackEnd.Models
{
    public class SocketHelper : InterfaceHelper
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public SocketHelper(bool PermanentConnect) : base(PermanentConnect) { }

        public override void Open(string ipAddress, object port, int timeout, int whichInterface)
        {
            int portNumber = Convert.ToInt32(port);
            _client = new TcpClient(ipAddress, portNumber)
            {
                ReceiveTimeout = timeout,
                SendTimeout = timeout
            };
            _stream = _client.GetStream();
        }

        public override void Open(string ipAddress, object port, int timeout)
        {
            Open(ipAddress, port, timeout, 0);
        }

        public override void Close()
        {
            _stream?.Close();
            _client?.Close();
        }

        public override byte[] Send(byte[] data, int ReplyCnt)
        {
            if (_client == null || !_client.Connected)
            {
                throw new Exception("Socket is not connected.");
            }

            _stream.Write(data, 0, data.Length);

            if (ReplyCnt <= 0)
            {
                return null;
            }

            byte[] buffer = new byte[BufSize];
            int bytesRead = _stream.Read(buffer, 0, buffer.Length);
            byte[] responseData = new byte[bytesRead];
            Array.Copy(buffer, responseData, bytesRead);
            return responseData;
        }

        public override void CBReceive(IAsyncResult ar)
        {
            // Implementation for asynchronous receive callback if needed
        }
    }
}

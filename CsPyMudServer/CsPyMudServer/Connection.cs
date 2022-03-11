using System;
using System.Net;
using System.Net.Security;

namespace CsPyMudServer
{
    public class Connection
    {
        private SslStream stream;
        private IPAddress clientAddress;

        public Connection(SslStream _stream, IPAddress _clientAddress)
        {
            stream = _stream;
            clientAddress = _clientAddress;
        }

        public IPAddress ClientIPAddress {  get { return clientAddress; } }

        public void Close()
        {
            stream.Close();
        }
    }
}

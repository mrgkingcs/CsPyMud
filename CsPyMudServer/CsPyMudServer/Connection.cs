using System;
using System.Net;
using System.Net.Security;

namespace CsPyMudServer
{
    public class Connection
    {
        private SslStream stream;
        private IPAddress clientAddress;
        private Conversation currentConversation;

        public Connection(SslStream _stream, IPAddress _clientAddress)
        {
            stream = _stream;
            clientAddress = _clientAddress;
            currentConversation = null;
        }

        public IPAddress ClientIPAddress {  get { return clientAddress; } }

        public Conversation CurrentConversation
        {
            get { return currentConversation; }
            set { 
                currentConversation = value;
                currentConversation.ClientStream = stream;
                currentConversation.Start();
            }
        }

        public void Close()
        {
            stream.Close();
        }
    }
}

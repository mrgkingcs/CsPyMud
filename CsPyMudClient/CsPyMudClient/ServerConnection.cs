using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;

namespace CsPyMudClient
{
    public delegate void MessageHandler(string message);

    public class ServerConnection
    {
        private const int READ_BUFFER_SIZE = 4096;

        private string serverName;
        private int serverPort;

        private SslStream sslStream;
        private byte[] readBuffer = new byte[READ_BUFFER_SIZE];

        public ServerConnection(string _serverAddress, int _serverPort)
        {
            serverName = _serverAddress;
            serverPort = _serverPort;
            sslStream = null;
            MessageHandler = null;
        }

        /// <summary>
        /// Open the connection to the server.
        /// </summary>
        /// <returns>The open.</returns>
        public bool Open() 
        {
            TcpClient client = new TcpClient(serverName, serverPort);
            sslStream = new SslStream(client.GetStream(),
                false,
                (sender, certificate, chain, sslPolicyErrors) => true,
                null
                );

            try
            {
                sslStream.AuthenticateAsClient(serverName);
                sslStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, (asyncResult) => this.ForwardIncomingMessage(asyncResult), this);
            } catch(AuthenticationException)
            {
                client.Close();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Close the stream.
        /// </summary>
        public void Close()
        {
            if(sslStream != null)
            {
                sslStream.Close();
                sslStream = null;
            }
        }

        /// <summary>
        /// Set this property to the external method that handles incoming messages
        /// </summary>
        public MessageHandler MessageHandler;

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="message">Message.</param>
        public void SendMessage(string message)
        {
            sslStream.Write(Encoding.ASCII.GetBytes(message));
        }

        /// <summary>
        /// Internal callback for passing on stuff we receive from the server
        /// </summary>
        /// <param name="result">Result.</param>
        private void ForwardIncomingMessage(IAsyncResult result)
        {
            int byteCount = sslStream.EndRead(result);
            if (byteCount > 0 && MessageHandler != null)
            {
                MessageHandler(Encoding.ASCII.GetString(readBuffer, 0, byteCount));
            }
            sslStream.BeginRead(readBuffer, 0, 4096, (asyncResult) => this.ForwardIncomingMessage(asyncResult), this);
        }
    }
}

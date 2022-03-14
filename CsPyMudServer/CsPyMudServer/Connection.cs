using System;
using System.Net;
using System.Net.Security;
using System.Text;

namespace CsPyMudServer
{
    public delegate void MessageHandler(string message);

    /// <summary>
    /// Class to translate string messages into binary stream messages
    /// </summary>
    public class Connection
    {
        private const int READ_BUFFER_SIZE = 4096; 

        private SslStream sslStream;
        private IPAddress clientAddress;

        private byte[] readBuffer = new byte[READ_BUFFER_SIZE];

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsPyMudServer.Connection"/> class.
        /// </summary>
        /// <param name="_stream">Stream.</param>
        /// <param name="_clientAddress">Client address.</param>
        public Connection(SslStream _stream, IPAddress _clientAddress)
        {
            sslStream = _stream;
            clientAddress = _clientAddress;

            sslStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, (asyncResult) => this.ForwardIncomingMessage(asyncResult), this);
        }

        public IPAddress ClientIPAddress {  get { return clientAddress; } }

        /// <summary>
        /// Close the connection to the client.
        /// </summary>
        public void Close()
        {
            sslStream.Close();
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

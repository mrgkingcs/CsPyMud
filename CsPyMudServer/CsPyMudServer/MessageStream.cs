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
    public class MessageStream
    {
        private const int READ_BUFFER_SIZE = 4096; 

        private SslStream sslStream;
        private IPAddress clientAddress;

        private byte[] readBuffer = new byte[READ_BUFFER_SIZE];
        private int numReadBytes = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsPyMudServer.Connection"/> class.
        /// </summary>
        /// <param name="_stream">Stream.</param>
        /// <param name="_clientAddress">Client address.</param>
        public MessageStream(SslStream _stream, IPAddress _clientAddress)
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
            sslStream.Write(Encoding.ASCII.GetBytes(message+'\0'));
        }

        /// <summary>
        /// Internal callback for passing on stuff we receive from the server
        /// </summary>
        /// <param name="result">Result.</param>
        private void ForwardIncomingMessage(IAsyncResult result)
        {
            if (sslStream != null)
            {
                try
                {
                    int byteCount = sslStream.EndRead(result);
                    if (byteCount > 0 && MessageHandler != null)
                    {
                        numReadBytes += byteCount;

                        int messageEndIndex = FindMessageEndChar();

                        if (messageEndIndex != -1)
                        {
                            MessageHandler(Encoding.ASCII.GetString(readBuffer, 0, messageEndIndex));
                            int numToCopy = (numReadBytes - messageEndIndex) - 1;
                            if (numToCopy > 0)
                            {
                                int srcIndex = messageEndIndex + 1;
                                for (int dstIndex = 0; dstIndex < numToCopy; dstIndex++, srcIndex++)
                                {
                                    readBuffer[dstIndex] = readBuffer[srcIndex];
                                }
                            }
                            numReadBytes = numToCopy;
                        }
                        sslStream.BeginRead(
                                readBuffer,
                                numReadBytes,
                                4096 - numReadBytes,
                                (asyncResult) => this.ForwardIncomingMessage(asyncResult),
                                this
                            );
                    }
                }
                catch (Exception) { }
            }
        }

        private int FindMessageEndChar()
        {
            for (int index = 0; index < numReadBytes; index++)
            {
                if (readBuffer[index] == 0)
                {
                    return index;
                }
            }
            return -1;
        }
    }
}

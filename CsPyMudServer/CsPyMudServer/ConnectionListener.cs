using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace CsPyMudServer
{
    public class ConnectionListener
    {
        TcpListener listener;
        Thread listenerThread;
        X509Certificate serverCertificate = null;

        ConcurrentQueue<SslStream> newConnectionStreams;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsPyMudServer.ConnectionListener"/> class.
        /// </summary>
        /// <param name="_listenPort">Listen port.</param>
        public ConnectionListener(int _listenPort)
        {
            byte[] certData = File.ReadAllBytes("mudserver.pfx");
            serverCertificate = new X509Certificate(certData, "bumbum");

            newConnectionStreams = new ConcurrentQueue<SslStream>();

            listener = new TcpListener(IPAddress.Any, _listenPort);
            listenerThread = new Thread(this.ListenForConnections);
        }

        /// <summary>
        /// Startup this instance.
        /// </summary>
        public void Startup()
        {
            listener.Start();
            listenerThread.Start();
        }

        /// <summary>
        /// Shutdown this instance.
        /// </summary>
        public void Shutdown()
        {
            listenerThread.Abort();
            listener.Stop();

            SslStream stream = GetNewConnection();
            while(stream != null)
            {
                stream.Close();
                stream = GetNewConnection();
            }
        }

        public SslStream GetNewConnection()
        {
            SslStream result = null;

            newConnectionStreams.TryDequeue(out result);

            return result;
        }

        /// <summary>
        /// Runs in a separate thread to listen for connections
        /// </summary>
        private void ListenForConnections()
        {
            while(true)
            {
                // blocks waiting for connection
                TcpClient client = listener.AcceptTcpClient();

                // process connection
                SslStream sslStream = new SslStream(client.GetStream(), false);
                try
                {
                    sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);

                    // Set timeouts for the read and write to 5 seconds.
                    sslStream.ReadTimeout = 5000;
                    sslStream.WriteTimeout = 5000;

                    newConnectionStreams.Enqueue(sslStream);
                }
                catch (AuthenticationException e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                    if (e.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                    }
                    Console.WriteLine("Authentication failed - closing the connection.");
                    sslStream.Close();
                    client.Close();
                }
            }
        }
    }
}

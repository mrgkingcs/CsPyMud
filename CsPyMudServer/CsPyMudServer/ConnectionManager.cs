using System;
using System.Net.Security;

namespace CsPyMudServer
{
    public class ConnectionManager
    {
        private ConnectionListener connectionListener;

        public ConnectionManager(   ConnectionListener _connectionListener,
                                    WorldManager _worldManager
            )
        {
            connectionListener = _connectionListener;
        }

        public void Startup()
        {

        }

        public void Shutdown()
        {

        }

        public void Tick()
        {
            // get new connections from connectionListener
            Connection newConnection = connectionListener.GetNewConnection();
            while (newConnection != null)
            {
                Console.WriteLine("New connection from {0}", newConnection.ClientIPAddress);
                newConnection = connectionListener.GetNewConnection();
            }
        }
    }
}

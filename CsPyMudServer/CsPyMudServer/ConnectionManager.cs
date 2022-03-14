using System;
using System.Collections.Generic;
using System.Net.Security;

namespace CsPyMudServer
{
    public class ConnectionManager
    {
        private ConnectionListener connectionListener;
        private List<Connection> authenticatingConnections;
        private List<Connection> playingConnections;

        public ConnectionManager(   ConnectionListener _connectionListener,
                                    WorldManager _worldManager
            )
        {
            connectionListener = _connectionListener;
            authenticatingConnections = new List<Connection>();
            playingConnections = new List<Connection>();
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
                newConnection.CurrentConversation = new AuthenticationConversation();
                authenticatingConnections.Add(newConnection);
            }

            //foreach(Connection conn in authenticatingConnections)
            for(int connIndex = authenticatingConnections.Count-1; connIndex >= 0; connIndex--)
            {
                Connection conn = authenticatingConnections[connIndex];
                AuthenticationConversation conv = (AuthenticationConversation)conn.CurrentConversation;
                if (conv.IsAuthenticated())
                {
                    authenticatingConnections.RemoveAt(connIndex);

                    conn.CurrentConversation = new PlayingConversation();
                    playingConnections.Add(conn);
                }
            }
        }
    }
}

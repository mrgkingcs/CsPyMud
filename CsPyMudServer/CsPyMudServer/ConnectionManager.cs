using System;
using System.Collections.Generic;
using System.Net.Security;

namespace CsPyMudServer
{
    public class ConnectionManager
    {
        private ConnectionListener connectionListener;
        private WorldManager worldManager;
        private List<Connection> connectionList;

        public ConnectionManager(ConnectionListener _connectionListener,
                                    WorldManager _worldManager
            )
        {
            connectionListener = _connectionListener;
            worldManager = _worldManager;
            connectionList = new List<Connection>();
        }

        public void Startup()
        {

        }

        public void Shutdown()
        {

        }

        public void Tick()
        {
            // get new connections from connectionListener...
            // ...and kick off Authentication conversations
            MessageStream incomingStream = connectionListener.GetNewConnection();
            while (incomingStream != null)
            {
                Console.WriteLine("New connection from {0}", incomingStream.ClientIPAddress);

                // create new Connection instance to hold info about player
                Connection newConnection = new Connection(incomingStream);

                // create and start Authentication conversation to challenge
                // for username/password
                AuthenticationConversation conversation =
                    new AuthenticationConversation(
                        newConnection,
                        (conv) => this.HandleAuthenticationComplete(newConnection)
                    );
                newConnection.Conversation = conversation;

                // add connection to live list
                connectionList.Add(newConnection);

                // check if there's another incoming connection to process
                incomingStream = connectionListener.GetNewConnection();
            }
        }

        /// <summary>
        /// Handler for when the authentication process is complete
        /// (whether it passes or fails)
        /// </summary>
        /// <param name="connection">Connection.</param>
        private void HandleAuthenticationComplete(Connection connection)
        {
            AuthenticationConversation authConv = (AuthenticationConversation)connection.Conversation;
            if (authConv.IsAuthenticated)
            {
                CharacterSelectConversation newConv =
                    new CharacterSelectConversation(
                        connection,
                        (conv) => this.HandleCharacterSelectComplete(connection)
                    );

                connection.Conversation = newConv;
            }
            else
            {
                connection.Close();
                connectionList.Remove(connection);
            }
        }

        /// <summary>
        /// Handler for when the character selection conversation is complete
        /// </summary>
        /// <param name="connection">Connection.</param>
        private void HandleCharacterSelectComplete(Connection connection)
        {
            CharacterSelectConversation charSelConv =
                (CharacterSelectConversation)connection.Conversation;

            // for now, just create an empty character with the chosen name.
            // should really load character from some kind of database...
            Player currPlayer = new Player();
            currPlayer.character = new Character();
            currPlayer.character.name = charSelConv.GetCharName();
            connection.SetData("PLAYER", currPlayer);

            PlayingConversation newConv =
                new PlayingConversation(
                    connection,
                    (conv) => this.HandlePlayingComplete(connection),
                    worldManager
                );


            connection.Conversation = newConv;
        }

        /// <summary>
        /// Handler for when the user exits the game
        /// </summary>
        /// <param name="connection">Connection.</param>
        private void HandlePlayingComplete(Connection connection)
        {
            connection.Close();
            connectionList.Remove(connection);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Security;

namespace CsPyMudServer
{
    public class ConnectionManager
    {
        private ConnectionListener connectionListener;
        private List<AuthenticationConversation> authenticatingConversations;
        private List<CharacterSelectConversation> charSelectConversations;
        private List<PlayingConversation> playingConversations;

        public ConnectionManager(   ConnectionListener _connectionListener,
                                    WorldManager _worldManager
            )
        {
            connectionListener = _connectionListener;
            authenticatingConversations = new List<AuthenticationConversation>();
            charSelectConversations = new List<CharacterSelectConversation>();
            playingConversations = new List<PlayingConversation>();
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
            Connection newConnection = connectionListener.GetNewConnection();
            while (newConnection != null)
            {
                Console.WriteLine("New connection from {0}", newConnection.ClientIPAddress);

                AuthenticationConversation conversation = new AuthenticationConversation(newConnection);
                conversation.Start();
                authenticatingConversations.Add(conversation);

                newConnection = connectionListener.GetNewConnection();
            }

            // loop through all Authentication conversations...
            for (int connIndex = authenticatingConversations.Count-1; connIndex >= 0; connIndex--)
            {
                AuthenticationConversation conv = authenticatingConversations[connIndex];
                // ...and if authentication has finished...
                if (conv.IsAuthenticated)
                {
                    // ...transfer them over to Playing conversations
                    authenticatingConversations.RemoveAt(connIndex);

                    CharacterSelectConversation newConversation = new CharacterSelectConversation(conv.Connection);
                    newConversation.Start();
                    charSelectConversations.Add(newConversation);
                }
            }

            // loop through all Character Select conversations...
            // (feels like there should be a better way of doing this...)
            for (int connIndex = charSelectConversations.Count - 1; connIndex >= 0; connIndex--)
            {
                CharacterSelectConversation conv = charSelectConversations[connIndex];
                // ...and if authentication has finished...
                if (conv.chosenCharacter != CharacterSelectConversation.INVALID_CHAR)
                {
                    // ...transfer them over to Playing conversations
                    charSelectConversations.RemoveAt(connIndex);

                    PlayingConversation newConversation = new PlayingConversation(conv.Connection);
                    newConversation.player.CharName = conv.GetCharName();
                    newConversation.Start();
                    playingConversations.Add(newConversation);
                }
            }

        }
    }
}

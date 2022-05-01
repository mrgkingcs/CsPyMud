using System;
namespace CsPyMudServer
{

    /// <summary>
    /// Connection.
    /// Keeps track of everything associated with a player's connection to the
    /// server
    /// </summary>
    public class Connection
    {
        private MessageStream inStream;
        private Conversation currentConversation;

        public Connection(MessageStream _inStream)
        {
            inStream = _inStream;
        }

        public void Close()
        {
            inStream.SendMessage("Connection closed by server");
            inStream.Close();
        }

        public MessageStream MessageStream { get { return inStream; } }
        public Conversation Conversation
        {
            get { return currentConversation;  }
            set { currentConversation = value;  currentConversation.Start();  }
        }
    }
}

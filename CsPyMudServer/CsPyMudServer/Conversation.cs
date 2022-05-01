using System;
using System.IO;

namespace CsPyMudServer
{
    public abstract class Conversation
    {
        public delegate void CompleteHandler(Conversation conversation);

        protected Connection connection;
        protected CompleteHandler completeHandler;

        public Conversation(Connection _connection, CompleteHandler handler)
        {
            connection = _connection;
            completeHandler = handler;
        }

        public Connection Connection { get { return connection; } }
        public MessageStream Stream { get { return connection.MessageStream; } }


        public abstract void Start();
    }
}

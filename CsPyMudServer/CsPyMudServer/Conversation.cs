using System;
using System.IO;

namespace CsPyMudServer
{
    public abstract class Conversation
    {
        public delegate void CompleteHandler(Conversation conversation);

        protected MessageStream connection;
        protected CompleteHandler completeHandler;

        public Conversation(MessageStream _connection, CompleteHandler handler)
        {
            connection = _connection;
            completeHandler = handler;
        }

        public MessageStream Connection { get { return connection; } }
        public abstract void Start();
    }
}

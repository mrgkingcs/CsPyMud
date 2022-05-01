using System;
using System.IO;

namespace CsPyMudServer
{
    public abstract class Conversation
    {
        protected MessageStream connection;

        public Conversation(MessageStream _connection)
        {
            connection = _connection;
        }

        public MessageStream Connection { get { return connection; } }
        public abstract void Start();
    }
}

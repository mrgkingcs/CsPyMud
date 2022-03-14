using System;
using System.IO;

namespace CsPyMudServer
{
    public abstract class Conversation
    {
        protected Connection connection;

        public Conversation(Connection _connection)
        {
            connection = _connection;
        }

        public Connection Connection { get { return connection; } }
        public abstract void Start();
    }
}

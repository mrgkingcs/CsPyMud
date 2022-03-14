using System;
using System.IO;

namespace CsPyMudServer
{
    public abstract class Conversation
    {
        protected Stream clientStream;

        public Stream ClientStream
        {
            get { return clientStream; }
            set { clientStream = value; }
        }

        public Conversation()
        {
        }

        public abstract void Start();
    }
}

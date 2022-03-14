using System;
namespace CsPyMudClient
{
    public class ServerConnection
    {
        public ServerConnection(string serverName, int port)
        {
        }

        public bool Open() 
        {

            return false;
        }

        public void Close()
        {

        }

        public Func<string, int> MessageHandler;
    }
}

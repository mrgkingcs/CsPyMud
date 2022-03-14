using System;
namespace CsPyMudServer
{
    public class PlayingConversation : Conversation
    {
        public PlayingConversation(Connection _connection) : base(_connection)
        {
        }

        public override void Start()
        {
            connection.SendMessage("Welcome to the dummy server which does nothing!!");
            connection.MessageHandler = this.Parrot;
        }

        private void Parrot(string message)
        {
            connection.SendMessage(message);
        }
    }
}

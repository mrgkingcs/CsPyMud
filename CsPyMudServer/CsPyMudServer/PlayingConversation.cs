using System;
namespace CsPyMudServer
{
    public class PlayingConversation : Conversation
    {
        public PlayerStatus player;

        public PlayingConversation(MessageStream _connection, CompleteHandler _handler)
            : base(_connection, _handler)
        {
            player = new PlayerStatus();
        }

        public override void Start()
        {
            string message = String.Format("Welcome, {0}, to the dummy server which does nothing!!\n", player.CharName);
            connection.SendMessage(message);
            connection.MessageHandler = this.Parrot;
        }

        private void Parrot(string message)
        {
            connection.SendMessage(message);
            if(message == "exit")
            {
                completeHandler(this);
            }
        }
    }
}

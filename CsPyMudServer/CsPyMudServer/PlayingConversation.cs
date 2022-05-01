using System;
namespace CsPyMudServer
{
    public class PlayingConversation : Conversation
    {
        private Player player;

        public PlayingConversation(Connection _connection, CompleteHandler _handler)
            : base(_connection, _handler)
        {
            player = (Player)connection.GetData("PLAYER");
            if(player == null)
            {
                Console.WriteLine("ERROR: Failed to get player data");
            }
        }

        public override void Start()
        {
            string message = String.Format("Welcome, {0}, to the dummy server which does nothing!!\n", player.character.name);
            Stream.SendMessage(message);
            Stream.MessageHandler = this.Parrot;
        }

        private void Parrot(string message)
        {
            Stream.SendMessage(message);
            if(message == "exit")
            {
                completeHandler(this);
            }
        }
    }
}

using System;
namespace CsPyMudServer
{
    public class PlayingConversation : Conversation
    {
        private WorldManager worldManager;
        private Player player;
        private Room currentRoom;

        public PlayingConversation( Connection _connection, 
                                    CompleteHandler _handler,
                                    WorldManager _worldManager
            ) : base(_connection, _handler)
        {
            worldManager = _worldManager;
            player = (Player)connection.GetData("PLAYER");
            if(player == null)
            {
                Console.WriteLine("ERROR: Failed to get player data");
            }
            currentRoom = worldManager.GetRoom("StartRoom");
            if (currentRoom == null)
            {
                Console.WriteLine("ERROR: Failed to get start room");
            }
        }

        public override void Start()
        {
            string message = String.Format("Welcome, {0}!!\n", player.character.name);
            Stream.SendMessage(message);

            // send info about starting room
            message = GenerateRoomMessage();
            Stream.SendMessage(message);

            Stream.MessageHandler = this.HandleCommand;
        }

        private string GenerateRoomMessage()
        {
            string message = "You never saw me...right?  O_o";
            if (currentRoom != null)
            {
                message = currentRoom.Description + "\n";
                message += "Available Exits: ";
                foreach (string exitName in currentRoom.Exits.Keys)
                {
                    message += exitName + ", ";
                }
                message = message.Substring(0, message.Length - 2);
                message += "\n";
            }
            return message;
        }

        private void HandleCommand(string command)
        {
            //Stream.SendMessage(message);
            if (command == "exit")
            {
                completeHandler(this);
            }
            else
            {
                string commandUpper = command.ToUpper();
                if (currentRoom.ExitNames.Contains(commandUpper))
                {
                    // move to new room
                    string exitDestination = currentRoom.Exits[commandUpper];
                    currentRoom = worldManager.GetRoom(exitDestination);
                }
                else
                {
                    string errorMessage = String.Format("I don't know how to \'{0}\'\n", command);
                    Stream.SendMessage(errorMessage);
                }
                string message = GenerateRoomMessage();
                Stream.SendMessage(message);
            }
        }
    }
}

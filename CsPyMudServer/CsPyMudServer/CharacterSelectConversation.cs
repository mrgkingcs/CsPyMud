using System;
namespace CsPyMudServer
{
    public class CharacterSelectConversation : Conversation
    {
        static string[] charNames = { "Jeff the Magus", "Jaff the Megus", "Juff the Mugus" };
        public const int INVALID_CHAR = -1;

        public int chosenCharacter;

        public CharacterSelectConversation(Connection _connection, CompleteHandler _handler)
            : base(_connection, _handler)
        {
            chosenCharacter = INVALID_CHAR;
        }

        public override void Start()
        {
            AskWhichCharacter();
        }

        public string GetCharName()
        {
            string result = "";
            if(chosenCharacter != INVALID_CHAR)
            {
                result = charNames[chosenCharacter];
            }
            return result;
        }

        public void AskWhichCharacter() { 
            string message = "Select your character:\n\n";
            for(int index = 0; index < charNames.Length; index++)
            {
                message += String.Format("{0})\t{1}\n", (index + 1), charNames[index]);
            }
            message += "\nEnter your choice:\n";

            Stream.SendMessage(message);
            Stream.MessageHandler = this.CheckCharacterChoice;
        }

        public void CheckCharacterChoice(string message)
        {
            int choice = -1;
            if(!int.TryParse(message.Trim(), out choice)) {
                Stream.SendMessage("You must enter a number.\n");
                AskWhichCharacter();
            } else if (choice <= 0 || choice > charNames.Length)
            {
                Stream.SendMessage("That is not an allowed number.\n");
                AskWhichCharacter();
            }
            else
            {
                chosenCharacter = choice-1;
                Stream.SendMessage(String.Format("You selected {0}.", charNames[chosenCharacter]));
                completeHandler(this);
            }
        }
    }
}

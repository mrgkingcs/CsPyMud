using System;
using System.Collections.Concurrent;

namespace CsPyMudServer
{
    public class AuthenticationConversation : Conversation
    {
        //====================================================================
        // Static password dictionary
        // (i.e. all instances of AuthenticationConversation use the same
        //  pw dictionary)
        //====================================================================
        private class PwEntry
        {
            public string username;
            public string password;

            public PwEntry() { username = password = ""; }
        }
        private static ConcurrentDictionary<string, PwEntry> passwordDB 
            = new ConcurrentDictionary<string, PwEntry>();

        /// <summary>
        /// Loads the user/password file into the passwordDB dictionary.
        /// </summary>
        private static void LoadPwFile()
        { 
            passwordDB.Clear();

            // read password file into dictionary...
        }

        /// <summary>
        /// Writes the contents of passwordDB into the user/password file
        /// </summary>
        private static void SavePwFile()
        {
            // write password dictionary into file...
        }

        /// <summary>
        /// Gets the password from the passwordDB
        /// (to compare to what the client sends through)
        /// </summary>
        /// <returns>The hashed pw.</returns>
        /// <param name="username">Username.</param>
        private static string GetPw(string username)
        {
            return passwordDB[username].password;
        }


        //====================================================================
        // member methods
        //====================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsPyMudServer.AuthenticationConversation"/> class.
        /// </summary>
        /// <param name="_connection">Connection.</param>
        public AuthenticationConversation(MessageStream _connection, CompleteHandler _handler) 
            : base(_connection, _handler)
        {
            IsAuthenticated = false;
        }

        /// <summary>
        /// Called to start off the conversation
        /// </summary>
        public override void Start()
        {
            connection.MessageHandler = this.HandleUsername;
            connection.SendMessage("USERNAME: ");
        }

        /// <summary>
        /// Returns <see langword="true"/> if username/password was correct
        /// </summary>
        /// <returns><c>true</c>, if correct username/password has been received 
        /// <c>false</c> otherwise.</returns>
        public bool IsAuthenticated;

        ///// <summary>
        ///// Returns <see langword="true"/> if authentication has failed
        ///// (i.e. too many failed attempts)
        ///// </summary>
        ///// <returns><c>true</c>, if username/password authentication has failed,
        ///// <c>false</c> otherwise.</returns>
        //public bool IsFailed()
        //{
        //    return false;
        //}

        //====================================================================
        // Message handlers
        //====================================================================
        private void HandleUsername(string message)
        {
            // need to store the username from the message for checking
            // password in HandlePassword()

            connection.MessageHandler = this.HandlePassword;
            connection.SendMessage("PASSWORD: ");
        }

        private void HandlePassword(string message)
        {
            // should probably actually do some checking of the password here...
            connection.SendMessage("Login successful.");
            IsAuthenticated = true;
            completeHandler(this);
        }
    }
}

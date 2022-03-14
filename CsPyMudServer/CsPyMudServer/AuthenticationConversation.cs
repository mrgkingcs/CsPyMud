using System;
using System.Collections.Concurrent;

namespace CsPyMudServer
{
    public class AuthenticationConversation : Conversation
    {
        // Static password dictionary
        // (i.e. all instances of AuthenticationConversation use the same
        //  pw dictionary)
        private class PwEntry
        {
            public string username;
            public string salt;
            public string hashedPw;
        }
        private static ConcurrentDictionary<string, PwEntry> passwordDB 
            = new ConcurrentDictionary<string, PwEntry>();

        private static void LoadPwFile()
        { 
            passwordDB.Clear();

            // read password file into dictionary...
        }

        private static void SavePwFile()
        {
            // write password dictionary into file...
        }

        private static string GetSalt(string username)
        {
            return passwordDB[username].salt;    
        }

        private static string GetHashedPw(string username)
        {
            return passwordDB[username].hashedPw;
        }


        // conversation instance methods

        /// <summary>
        /// Called to start off the conversation
        /// </summary>
        public override void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns <see langword="true"/> if username/password was correct
        /// </summary>
        /// <returns><c>true</c>, if correct username/password has been received 
        /// <c>false</c> otherwise.</returns>
        public bool IsAuthenticated()
        {
            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if authentication has failed
        /// (i.e. too many failed attempts)
        /// </summary>
        /// <returns><c>true</c>, if username/password authentication has failed,
        /// <c>false</c> otherwise.</returns>
        public bool IsFailed()
        {
            return false;
        }

    }
}

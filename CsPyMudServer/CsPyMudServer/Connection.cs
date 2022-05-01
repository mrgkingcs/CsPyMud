using System;
using System.Collections.Generic;

namespace CsPyMudServer
{

    /// <summary>
    /// Connection.
    /// Keeps track of everything associated with a player's connection to the
    /// server
    /// </summary>
    public class Connection
    {
        private MessageStream inStream;
        private Conversation currentConversation;
        private Dictionary<string, object> dataDict;

        public Connection(MessageStream _inStream)
        {
            inStream = _inStream;
            dataDict = new Dictionary<string, object>();
        }

        public void Close()
        {
            inStream.SendMessage("Connection closed by server");
            inStream.Close();
        }

        public void SetData(string name, object datum)
        {
            if(dataDict.ContainsKey(name))
            {
                dataDict[name] = datum;
            }
            else
            {
                dataDict.Add(name, datum);
            }
        }
        public object GetData(string name)
        {
            object result = null;
            if(dataDict.ContainsKey(name)) {
                result = dataDict[name];
            }
            return result;
        }
        public void ClearData(string name)
        {
            if(dataDict.ContainsKey(name))
            {
                dataDict.Remove(name);
            }
        }

        public MessageStream MessageStream { get { return inStream; } }
        public Conversation Conversation
        {
            get { return currentConversation;  }
            set { currentConversation = value;  currentConversation.Start();  }
        }
    }
}

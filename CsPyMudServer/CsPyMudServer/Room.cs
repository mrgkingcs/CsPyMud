using System;
using System.Collections.Generic;
using System.Linq;

namespace CsPyMudServer
{
    public class Room
    {
        public String Description;
        public Dictionary<string, string> Exits;
        public ICollection<string> ExitNames { get { return Exits.Keys; } }

        public Room()
        {
            Exits = new Dictionary<string, string>();
        }
    }
}

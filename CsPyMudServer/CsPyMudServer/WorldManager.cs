using System;
using System.Collections.Generic;
using System.IO;
using System.Json;

namespace CsPyMudServer
{
    public class WorldManager
    {
        private Dictionary<string, Room> roomsByID;

        public WorldManager()
        {
            LoadRoomData();
        }

        public void Startup()
        {

        }

        public void Shutdown()
        {

        }

        public void Tick()
        {

        }

        public Room GetRoom(string ID)
        {
            Room result = null;
            if(roomsByID.ContainsKey(ID)) 
            { 
                result = roomsByID[ID]; 
            }
            else
            {
                Console.WriteLine("Failed to find room \'{0}\'", ID);
            }
            return result;
        }


        private void LoadRoomData()
        {
            roomsByID = new Dictionary<string, Room>();
            string roomData = File.ReadAllText("Rooms.json");
            JsonValue parsedData = JsonValue.Parse(roomData);
            foreach (KeyValuePair<string, JsonValue> keyValuePair in parsedData)
            {
                string ID = keyValuePair.Key;
                JsonValue newRoomFields = keyValuePair.Value;

                Room newRoom = ParseRoom(newRoomFields);

                Console.WriteLine("Loaded room \'{0}\'", ID);
                roomsByID.Add(ID, newRoom);
            }
        }

        private Room ParseRoom(JsonValue roomFields)
        {
            Room newRoom = new Room();
            newRoom.Description = roomFields["Description"];
            newRoom.Exits = ParseExits(roomFields["Exits"]);
            return newRoom;
        }

        private Dictionary<string, string> ParseExits(JsonValue exitDict)
        {
            Dictionary<string, string> exits = new Dictionary<string, string>();
            foreach(KeyValuePair<string,JsonValue> keyValuePair in exitDict)
            {
                exits.Add(keyValuePair.Key.ToUpper(), (string)(keyValuePair.Value));
            }
            return exits;
        }
    }
}

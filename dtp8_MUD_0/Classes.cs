using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dtp8_MUD_0
{
    internal class Player
    {
        public string name = "Mario";
        public List<string> keys = new();
    }
    internal class Door
    {
        public int id;
        Room[] room = new Room[2];
        string key;
        bool open;
        public Door(int id, Room roomA, Room roomB, string key = "", bool open = true)
        {
            this.id = id;
            room[0] = roomA;
            room[1] = roomB;
            this.key = key;
            this.open = open;
        }
        public int MoveFrom(Room R)
        //Call using current room as parameter in order to move through door.
        {
            if(!open) return -1;
            if(open)
            {
                if (room[0] == R) return room[1].number;
                if (room[1] == R) return room[0].number;
            }
            return -2;
        }
        public string Unlock(Player creature)
        {
            if (open) return "Dörren är inte låst.";
            if(!creature.keys.Contains(key)) return "Du behöver en " + key + ".";
            open = true;
            return "Du låser upp dörren med en " + key + "!";
        }
    }
    internal class Room
    {
        //TODO: Make list of exits instead, default setting up n/w/e/s. Each exit is a ref to a door.
        //TODO: Make a method that returns a string of available exits.
        // Constants:
        public const int North = 0;
        public const int East = 1;
        public const int South = 2;
        public const int West = 3;
        public const int NoDoor = -1;

        //TBD: Privatisera variablar
        // Object attributes:
        public int number;
        public string roomname = "";
        public string story = "";
        public string imageFile = "";
        public int[] adjacent = new int[4]; // adjacent[Room.North] etc.
        public List<string> keys = new();
        public Room(int num, string name)
        {
            number = num; roomname = name;
        }
        public void SetStory(string theStory)
        {
            story = theStory;
        }
        public void SetImage(string theImage)
        {
            imageFile = theImage;
        }
        public void SetDirections(int N, int E, int S, int W)
        {
            adjacent[North] = N; adjacent[East] = E; adjacent[South] = S; adjacent[West] = W;
        }
        //NYI: Metoder för att byta rum med dörrar.
        public int Move(Key k)
        {
            switch(k)
            {
                case Key.Left:
                    return adjacent[West];
                case Key.Right:
                    return adjacent[East];
                case Key.Up:
                    return adjacent[North];
                case Key.Down:
                    return adjacent[South];
            }
            return -1;
        }
        public int GetNorth() => adjacent[North];
        public int GetEast() => adjacent[East];
        public int GetSouth() => adjacent[South];
        public int GetWest() => adjacent[West];
    }

}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokeBasic.Handler.PokeFinder;

namespace PokeBasic.Entities
{
    [Serializable]
    class Poke
    {
        public Coords Coords { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Id { get; set; }
        public int Id_Pokemon { get; set; }
        public int Movement { get; set; }
        public int OriginalMovement { get; set; }
        public int DistanceMoved { get; set; }
        public List<Move> Moves { get; set; }
        public Teams Team { get; set; }
        public bool CanMove { get { return (DistanceMoved < Movement); } }

        public Poke()
        {
            Name = string.Empty;
            Url = string.Empty;
            Id = -1;
            Movement = -1;
            OriginalMovement = -1;
            Id_Pokemon = -1;
            Moves = new List<Move>();
            Team = Teams.Undefined;
            DistanceMoved = 0;
        }

        public Poke(string name, string url, int id, int movement)
        {
            Name = name.Replace("'", "''");
            Url = url.Replace("'", "''");
            Id = id;
            Movement = movement;
            OriginalMovement = movement;
            Id_Pokemon = -1;
            Moves = new List<Move>();
            Team = Teams.Undefined;
            DistanceMoved = 0;
        }

        public Poke(string name, string url, int id, int movement, List<JToken> moves)
        {
            Name = name.Replace("'", "''");
            Url = url.Replace("'", "''");
            Id = id;
            Movement = movement;
            OriginalMovement = movement;
            Id_Pokemon = -1;
            Moves = new List<Move>();
            foreach (var move in moves)
            {
                Moves.Add(new Move((string)move["Name"], (string)move["Notes"], (string)move["Type"], (int)move["BaseWheelSize"], (string)move["Damage"]));
            }
            Team = Teams.Undefined;
            DistanceMoved = 0;
        }

        public Poke(string name, int x, int y)
        {
            this.Name = name;
            Coords = new Coords(x, y);
            Team = Teams.Undefined;
            DistanceMoved = 0;
        }


    }
}

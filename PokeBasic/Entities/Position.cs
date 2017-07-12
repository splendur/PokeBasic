using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBasic.Entities
{
    [Serializable]
    class Position
    {
        public readonly Guid guid = new Guid();
        public Poke Occupant { get; set; }
        public bool Active { get; set; }
        public List<Coords> Links { get; set; }
        public Coords Coords { get; set; }
        public Coords SelfCoords { get; set; }

        public Position()
        {
            Active = false;
            Links = new List<Coords>();
            Coords = new Coords(0,0);
            SelfCoords = new Coords(0, 0);
            guid = Guid.NewGuid();
        }

        public Position(int x, int y)
        {
            Active = true;
            Links = new List<Coords>();
            Coords = new Coords(x, y);
            SelfCoords = new Coords(0, 0);
            guid = Guid.NewGuid();
        }

        public Position(List<Coords> links, int x, int y)
        {
            Active = true;
            Links = links;
            Coords = new Coords(x, y);
            SelfCoords = new Coords(0, 0);
            guid = Guid.NewGuid();
        }

        public Position(List<Coords> links, int x, int y, int selfx)
        {
            Active = true;
            Links = links;
            Coords = new Coords(x, y);
            SelfCoords = new Coords(selfx, -1);
            guid = Guid.NewGuid();
        }

        public string getOcupantName()
        {
            return (Occupant != null) ? Occupant.Name.Substring(0, 3) : " ¤ ";
        }
    }

    [Serializable]
    internal class Coords
    {
        public int x, y;

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public bool IsCoordInRange(Coords position, int range)
        {
            return ((Math.Pow(x - position.x, 2) + Math.Pow(y - position.y, 2)) < (range * range));
        }

        public override string ToString()
        {
            return string.Format("({0};{1})", x, y);
        }
    }
}

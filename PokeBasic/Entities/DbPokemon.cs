using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBasic.Entities
{
    [Serializable]
    class DbPokemon
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Id { get; set; }
        public int Id_Pokemon { get; set; }
        public int Movement { get; set; }
        public int Encounters { get; set; }

        public DbPokemon()
        {
            Name = string.Empty;
            Url = string.Empty;
            Id = -1;
            Movement = -1;
            Id_Pokemon = -1;
            Encounters = -1;
        }

        public DbPokemon(string name, string url, int id, int movement)
        {
            Name = name.Replace("'", "''");
            Url = url.Replace("'", "''");
            Id = id;
            Movement = movement;
            Id_Pokemon = -1;
            Encounters = -1;
        }

        public DbPokemon(int Id, int Id_Pokemon, System.String Name, int Movement, System.String Url, int Encounters)
        {
            this.Name = Name;
            this.Id = Id_Pokemon;
            this.Movement = Movement;
            this.Url = Url;
            this.Id_Pokemon = Id;
            this.Encounters = Encounters;
        }
    }
}

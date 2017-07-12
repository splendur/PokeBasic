using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBasic.Entities
{
    [Serializable]
    class Move
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int BaseWheelSize { get; set; }
        public int Damage { get; set; }

        public Move()
        {
            Id = -1;
            Name = string.Empty;
            Description = string.Empty;
            Type = string.Empty;
            BaseWheelSize = -1;
            Damage = -1;
        }

        public Move(string name, string descritpion, string type)
        {
            Id = -1;
            Name = name.Replace("'", "''");
            Description = descritpion.Replace("'", "''");
            Type = type;
            BaseWheelSize = -1;
            Damage = -1;
        }

        public Move(string name, string descritpion, string type, int baseWheelSize, string damage)
        {
            Id = -1;
            Name = name.Replace("'", "''");
            Description = descritpion.Replace("'", "''");
            Type = type;
            BaseWheelSize = baseWheelSize;
            int dmg = 0;
            int.TryParse(damage, out dmg);
            Damage = dmg;
        }
    }
}

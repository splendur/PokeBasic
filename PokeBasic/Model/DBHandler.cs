using Dapper;
using Newtonsoft.Json.Linq;
using PokeBasic.Entities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBasic.Model
{
    static class DBHandler
    { 
        public static string filePath = "Poke.sqlite";

        public static bool InitializeSQLiteDB ()
        {
            var result = false;

            if (!File.Exists(filePath))
            {
                createDB(filePath);
                using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", filePath)))
                {
                    List<Move> allMoves = new List<Move>();
                    List<Poke> allPoke = new List<Poke>();
                    JArray o1 = JArray.Parse(File.ReadAllText(@"C:\Users\FrIcK\Documents\Visual Studio 2017\Projects\PokeBasic\PokeBasic\JSON\pokeExtract_NoStarsStrt.json"));
                    foreach (var poke in o1)
                    {
                        Console.WriteLine(poke["Name"]);
                        Poke pok = new Poke((string)poke["Name"], (string)poke["Url"], (int)poke["Id"], (int)poke["Movement"], poke["Moves"].ToList());
                        allPoke.Add(pok);
                        foreach (var move in poke["Moves"])
                        {
                            Move newMove = new Move((string)move["Name"], (string)move["Notes"], (string)move["Type"]);
                            if (!allMoves.Any(x => x.Name.Equals(newMove.Name)))
                                allMoves.Add(newMove);
                        }
                    }
                    string sqlIns = string.Empty;
                    foreach (var move in allMoves)
                    {
                        m_dbConnection.Execute(string.Format("insert into Moves (Name, Description, Type) values ('{0}', '{1}', '{2}')", move.Name, move.Description, move.Type));
                    }
                    foreach (var poke in allPoke)
                    {
                        m_dbConnection.Execute(string.Format("insert into Pokemons (Name, Url, Id_Pokemon, Movement) values ('{0}', '{1}', {2}, {3})", poke.Name, poke.Url, poke.Id, poke.Movement));

                        DbPokemon Pokemon = m_dbConnection.Query<DbPokemon>(String.Format("Select * from Pokemons where Id_Pokemon = {0} LIMIT 1", poke.Id)).FirstOrDefault();
                        foreach (var move in poke.Moves)
                        {
                            Move dbMove = m_dbConnection.Query<Move>(String.Format("Select * from Moves where Name = '{0}' LIMIT 1", move.Name)).FirstOrDefault();
                            move.Id = dbMove.Id;
                            m_dbConnection.Execute(string.Format("insert into PokemonMoves(Id_Pokemon, Id_Moves, BaseWheelSize, Damage) Values({0}, {1}, {2}, {3})", Pokemon.Id, dbMove.Id, move.BaseWheelSize, move.Damage));
                        }


                    }
                }
            }
            return result;
        }

        private static void createDB(string filePath)
        {
            SQLiteConnection.CreateFile(filePath);
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", filePath)))
            {
                m_dbConnection.Execute("CREATE TABLE 'Pokemons' ( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 'Id_Pokemon' INTEGER NOT NULL UNIQUE, 'Name' TEXT NOT NULL, 'Movement' INTEGER NOT NULL, 'Url' TEXT NOT NULL, 'Encouters' INTEGER NOT NULL DEFAULT 0 )");
                m_dbConnection.Execute("CREATE TABLE 'Moves' ( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 'Name' TEXT NOT NULL UNIQUE, 'Description' TEXT, 'Type' TEXT NOT NULL)");
                m_dbConnection.Execute("CREATE TABLE 'PokemonMoves' ( 'Id_Pokemon' INTEGER NOT NULL, 'Id_Moves' INTEGER NOT NULL, 'BaseWheelSize' INTEGER NOT NULL, 'Damage' INTEGER NOT NULL, FOREIGN KEY('Id_Pokemon') REFERENCES 'Pokemons'('Id'), FOREIGN KEY('Id_Moves') REFERENCES 'Moves'('Id') )");
            }
        }

        public static Poke getPoke(Poke poke)
        {
            Poke Pokemon = null;
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", filePath)))
            {
                Pokemon = m_dbConnection.Query<Poke>(String.Format("Select * from Pokemons where Name = '{0}' LIMIT 1", poke.Name)).FirstOrDefault();
                List<Move> pokeMoves = m_dbConnection.Query<Move>(String.Format("Select Moves.Id, Moves.Name, Moves.Type, PokemonMoves.Damage ,PokemonMoves.BaseWheelSize, Moves.Description From PokemonMoves inner join Pokemons on PokemonMoves.Id_Pokemon = Pokemons.Id and Pokemons.Id = {0} inner join Moves on Moves.Id = PokemonMoves.Id_Moves", Pokemon.Id)).ToList();
                Pokemon.Moves = pokeMoves;
                Pokemon.OriginalMovement = Pokemon.Movement;
            }

            return Pokemon;
        }
    }
}

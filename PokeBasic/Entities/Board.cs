using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PokeBasic.Entities
{
    [Serializable]
    class Board
    {
        public Position[,] _Board { get; set; }
        public Position[] _MyBench { get; set; }
        public Position[] _OpponentBench { get; set; }
        private List<int> RowsY = new List<int>();
        public string Signature
        {
            get
            {
                var signature = new StringBuilder();
                var allPokePos = _OpponentBench.Where(ob => (ob.Occupant != null)).ToList();
                allPokePos.AddRange(_MyBench.Where(mb => (mb.Occupant != null)).ToList());
                foreach (var boardPos in _Board)
                {
                    if (boardPos != null && boardPos.Active && boardPos.Occupant != null)
                    {
                        allPokePos.Add(boardPos);
                    }
                }
                foreach (var pokem in allPokePos)
                {
                    signature.AppendFormat("[{0}{1}{2}]", pokem.Occupant.Id, pokem.Occupant.Name, pokem.SelfCoords.ToString());
                }
                return signature.ToString();
            }
        }
        public enum BoardType { Board, MyBench, OpponentBench};

        public Board()
        {
            RowsY.Add(104);
            RowsY.Add(145);
            RowsY.Add(193);
            RowsY.Add(246);
            RowsY.Add(303);
            #region Board Setup
            _Board = new Position[5, 7];
            _Board[0, 0] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 1), new Coords(1, 0), new Coords(1, 3) },
                Coords = new Coords(48, RowsY[0]),
                SelfCoords = new Coords(0,0)
            };
            _Board[0, 1] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 0), new Coords(0, 2) },
                Coords = new Coords(93, RowsY[0]),
                SelfCoords = new Coords(0, 1)
            };
            _Board[0, 2] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 1), new Coords(0, 3), new Coords(1, 3) },
                Coords = new Coords(137, RowsY[0]),
                SelfCoords = new Coords(0, 2)
            };
            _Board[0, 3] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 2), new Coords(0, 4) },
                Coords = new Coords(179, RowsY[0]),
                SelfCoords = new Coords(0, 3)
            };
            _Board[0, 4] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 3), new Coords(0, 5) },
                Coords = new Coords(223, RowsY[0]),
                SelfCoords = new Coords(0, 4)
            };
            _Board[0, 5] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 4), new Coords(0, 6) },
                Coords = new Coords(265, RowsY[0]),
                SelfCoords = new Coords(0, 5)
            };
            _Board[0, 6] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 5), new Coords(1, 4), new Coords(1, 6) },
                Coords = new Coords(314, RowsY[0]),
                SelfCoords = new Coords(0, 6)
            };
            _Board[1, 0] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 0), new Coords(2, 0) },
                Coords = new Coords(46, RowsY[1]),
                SelfCoords = new Coords(1, 0)
            };
            _Board[1, 2] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 0), new Coords(1, 3), new Coords(3, 2) },
                Coords = new Coords(113, RowsY[1]),
                SelfCoords = new Coords(1, 2)
            };
            _Board[1, 3] = new Position()
            {
                Active = true,
                Links = { new Coords(1, 2), new Coords(0, 2), new Coords(1, 4) },
                Coords = new Coords(179, RowsY[1]),
                SelfCoords = new Coords(1, 3)
            };
            _Board[1, 4] = new Position()
            {
                Active = true,
                Links = { new Coords(1, 3), new Coords(0, 6), new Coords(2, 4) },
                Coords = new Coords(245, RowsY[1]),
                SelfCoords = new Coords(1, 4)
            };
            _Board[1, 6] = new Position()
            {
                Active = true,
                Links = { new Coords(0, 6), new Coords(2, 6) },
                Coords = new Coords(312, RowsY[1]),
                SelfCoords = new Coords(1, 6)
            };
            _Board[2, 0] = new Position()
            {
                Active = true,
                Links = { new Coords(1, 0), new Coords(2, 0) },
                Coords = new Coords(43, RowsY[2]),
                SelfCoords = new Coords(2, 0)
            };
            _Board[2, 2] = new Position()
            {
                Active = true,
                Links = { new Coords(1, 2), new Coords(3, 2) },
                Coords = new Coords(113, RowsY[2]),
                SelfCoords = new Coords(2, 2)
            };
            _Board[2, 4] = new Position()
            {
                Active = true,
                Links = { new Coords(1, 4), new Coords(3, 4) },
                Coords = new Coords(245, RowsY[2]),
                SelfCoords = new Coords(2, 4)
            };
            _Board[2, 6] = new Position()
            {
                Active = true,
                Links = { new Coords(1, 6), new Coords(3, 6) },
                Coords = new Coords(316, RowsY[2]),
                SelfCoords = new Coords(2, 6)
            };
            _Board[3, 0] = new Position()
            {
                Active = true,
                Links = { new Coords(2, 0), new Coords(4, 0) },
                Coords = new Coords(40, RowsY[3]),
                SelfCoords = new Coords(3, 0)
            };
            _Board[3, 2] = new Position()
            {
                Active = true,
                Links = { new Coords(2, 2), new Coords(4, 0), new Coords(3, 3) },
                Coords = new Coords(109, RowsY[3]),
                SelfCoords = new Coords(3, 2)
            };
            _Board[3, 3] = new Position()
            {
                Active = true,
                Links = { new Coords(3, 2), new Coords(4, 4), new Coords(3, 4) },
                Coords = new Coords(179, RowsY[3]),
                SelfCoords = new Coords(3, 3)
            };
            _Board[3, 4] = new Position()
            {
                Active = true,
                Links = { new Coords(3, 3), new Coords(2, 4), new Coords(4, 6) },
                Coords = new Coords(249, RowsY[3]),
                SelfCoords = new Coords(3, 4)
            };
            _Board[3, 6] = new Position()
            {
                Active = true,
                Links = { new Coords(2, 6), new Coords(4, 6) },
                Coords = new Coords(319, RowsY[3]),
                SelfCoords = new Coords(3, 6)
            };
            _Board[4, 0] = new Position()
            {
                Active = true,
                Links = { new Coords(3, 0), new Coords(3, 2), new Coords(4, 1) },
                Coords = new Coords(35, RowsY[4]),
                SelfCoords = new Coords(4, 0)
            };
            _Board[4, 1] = new Position()
            {
                Active = true,
                Links = { new Coords(4, 0), new Coords(4, 2) },
                Coords = new Coords(83, RowsY[4]),
                SelfCoords = new Coords(4, 1)
            };
            _Board[4, 2] = new Position()
            {
                Active = true,
                Links = { new Coords(4, 1), new Coords(4, 3) },
                Coords = new Coords(132, RowsY[4]),
                SelfCoords = new Coords(4, 2)
            };
            _Board[4, 3] = new Position()
            {
                Active = true,
                Links = { new Coords(4, 2), new Coords(4, 4) },
                Coords = new Coords(179, RowsY[4]),
                SelfCoords = new Coords(4, 3)
            };
            _Board[4, 4] = new Position()
            {
                Active = true,
                Links = { new Coords(3, 3), new Coords(4, 3), new Coords(4, 5) },
                Coords = new Coords(227, RowsY[4]),
                SelfCoords = new Coords(4, 4)
            };
            _Board[4, 5] = new Position()
            {
                Active = true,
                Links = { new Coords(4, 4), new Coords(4, 6) },
                Coords = new Coords(275, RowsY[4]),
                SelfCoords = new Coords(4, 5)
            };
            _Board[4, 6] = new Position()
            {
                Active = true,
                Links = { new Coords(3, 4), new Coords(3, 6), new Coords(4, 5) },
                Coords = new Coords(344, RowsY[4]),
                SelfCoords = new Coords(4, 6)
            };
            
            #endregion

            #region MyBench Setup
            _MyBench = new Position[8];
            _MyBench[0] = new Position(null, 35,365, 0);
            _MyBench[1] = new Position(null, 73, 399, 1);
            _MyBench[2] = new Position(null, 108, 365, 2);
            _MyBench[3] = new Position(null, 143, 399, 3);
            _MyBench[4] = new Position(null, 178, 365, 4);
            _MyBench[5] = new Position(null, 218, 399, 5);
            //Dead                                    
            _MyBench[6] = new Position(null, 254, 365, 6);
            _MyBench[7] = new Position(null, 308, 365, 7);
            #endregion

            #region OpponentBench Setup
            _OpponentBench = new Position[8];
            _OpponentBench[0] = new Position(null, 121, 30, 0);
            _OpponentBench[1] = new Position(null, 151, 49, 1);
            _OpponentBench[2] = new Position(null, 183, 30, 2);
            _OpponentBench[3] = new Position(null, 213, 49, 3);
            _OpponentBench[4] = new Position(null, 243, 30, 4);
            _OpponentBench[5] = new Position(null, 274, 49, 5);
            //Dead                                        
            _OpponentBench[6] = new Position(null, 39, 49, 6);
            _OpponentBench[7] = new Position(null, 83, 49, 7);
            #endregion

        }

        public void movePokemon(Guid SourcePosition, Guid DestinationPosition)
        {
            //here
            Position source = _MyBench.FirstOrDefault(mb => (mb.guid.Equals(SourcePosition)));
            if (source == null)
            {
                source = _OpponentBench.FirstOrDefault(ob => (ob.guid.Equals(SourcePosition)));
            }
            if (source == null)
            {
                foreach (var b in _Board)
                {
                    if (b != null && b.guid.Equals(SourcePosition))
                    {
                        source = b;
                        break;
                    }
                }
            }

            Position dest = _MyBench.FirstOrDefault(mb => (mb.guid.Equals(DestinationPosition)));
            if (dest == null)
            {
                dest = _OpponentBench.FirstOrDefault(ob => (ob.guid.Equals(DestinationPosition)));
            }
            if (dest == null)
            {
                foreach (var b in _Board)
                {
                    if (b != null && b.guid.Equals(DestinationPosition))
                    {
                        dest = b;
                        break;
                    }
                }
            }
            
            dest.Occupant = source.Occupant;
            source.Occupant = null;
        }

        public List<Position> GetOccupiedBoard()
        {
            var boardPokes = new List<Position>();
            foreach (var pos in _Board)
            {
                if (pos != null && pos.Active && pos.Occupant != null)
                {
                    boardPokes.Add(pos);
                }
            }
            return boardPokes;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("         {0} {1} {2}\n", _OpponentBench[0].getOcupantName(), _OpponentBench[2].getOcupantName(), _OpponentBench[4].getOcupantName());
            result.AppendFormat("{3} {4} | {0} {1} {2}\n", _OpponentBench[1].getOcupantName(), _OpponentBench[3].getOcupantName(), _OpponentBench[5].getOcupantName(), _OpponentBench[6].getOcupantName(), _OpponentBench[7].getOcupantName());
            result.AppendFormat("--------------------------------\n");
            result.AppendFormat("{0}-{1}-{2}-{3}-{4}-{5}-{6}\n", _Board[0, 0].getOcupantName(), _Board[0, 1].getOcupantName(), _Board[0, 2].getOcupantName(), _Board[0, 3].getOcupantName(), _Board[0, 4].getOcupantName(), _Board[0, 5].getOcupantName(), _Board[0, 6].getOcupantName());
            result.AppendFormat(" | \\       \\           / |\n");
            result.AppendFormat(" |  \\       \\         /  |\n");
            result.AppendFormat("{0} {1}-----{2}-----{3} {4}\n", _Board[1, 0].getOcupantName(), _Board[1, 2].getOcupantName(), _Board[1, 3].getOcupantName(), _Board[1, 4].getOcupantName(), _Board[1, 6].getOcupantName());
            result.AppendFormat(" |   |               |   |\n");
            result.AppendFormat("{0} {1}             {2} {3}\n", _Board[2, 0].getOcupantName(), _Board[2, 2].getOcupantName(), _Board[2, 4].getOcupantName(), _Board[2, 6].getOcupantName());
            result.AppendFormat(" |   |               |   |\n");
            result.AppendFormat("{0} {1}-----{2}-----{3} {4}\n", _Board[3, 0].getOcupantName(), _Board[3, 2].getOcupantName(), _Board[3, 3].getOcupantName(), _Board[3, 4].getOcupantName(), _Board[3, 6].getOcupantName());
            result.AppendFormat(" |  /         \\       \\  |\n");
            result.AppendFormat(" | /           \\       \\ |\n");
            result.AppendFormat("{0}-{1}-{2}-{3}-{4}-{5}-{6}\n", _Board[4, 0].getOcupantName(), _Board[4, 1].getOcupantName(), _Board[4, 2].getOcupantName(), _Board[4, 3].getOcupantName(), _Board[4, 4].getOcupantName(), _Board[4, 5].getOcupantName(), _Board[4, 6].getOcupantName());
            result.AppendFormat("--------------------------------\n");
            result.AppendFormat("{0} {1} {2}\n", _MyBench[0].getOcupantName(), _MyBench[2].getOcupantName(), _MyBench[4].getOcupantName());
            result.AppendFormat(" {0} {1} {2} | {3} {4}\n", _MyBench[1].getOcupantName(), _MyBench[3].getOcupantName(), _MyBench[5].getOcupantName(), _MyBench[6].getOcupantName(), _MyBench[7].getOcupantName());

            return result.ToString();
        }
    }

    
}

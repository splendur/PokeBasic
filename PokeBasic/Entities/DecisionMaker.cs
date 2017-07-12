using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PokeBasic.Entities
{
    class DecisionMaker
    {
        BoardTreeNode rootBoardTree { get; set; }

        public DecisionMaker(Board board)
        {
            Console.WriteLine("Initializing Decision Maker");
            rootBoardTree = new BoardTreeNode(board);
        }

        public string ReactorDepth(int depth)
        {
            Console.WriteLine("Reactor depth {0}", depth);
            var boardPokes = rootBoardTree.Board.GetOccupiedBoard();
            foreach (var occupier in boardPokes)
            {
                moveToAllLinks(rootBoardTree, rootBoardTree.Board._Board[occupier.SelfCoords.x, occupier.SelfCoords.y], occupier.Occupant.Movement);
            }

            //foreach (var child in rootBoardTree._children.Values)
            //{
            //    Console.Write(child.Board.ToString());
            //}
            rootBoardTree.TreeToString();
            return string.Empty;
        }

        public void moveToAllLinks(BoardTreeNode boardNode, Position SourcePosition, int range)
        {
            foreach (var link in SourcePosition.Links)
            {
                var cloneBoard = DeepClone(boardNode.Board);
                cloneBoard.movePokemon(SourcePosition.guid, cloneBoard._Board[link.x, link.y].guid);
                boardNode.Add(new BoardTreeNode(cloneBoard));
                //Console.WriteLine("Generated {0}", cloneBoard.Signature);

                if (range > 1)
                {
                    //if (boardNode._children.ContainsKey(cloneBoard.Signature))
                        moveToAllLinks(boardNode.GetChild(cloneBoard.Signature), boardNode.Board._Board[link.x, link.y], range - 1);
                }
            }
        }

        public static Board DeepClone<Board>(Board obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (Board)formatter.Deserialize(ms);
            }
        }
    }
}

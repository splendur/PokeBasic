using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static PokeBasic.Handler.PokeFinder;

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
            var boardPokes = rootBoardTree.Board.GetOccupiedBoard().Where(oc => (oc.Occupant.Team.Equals(Teams.Own) && oc.Occupant.CanMove));
            foreach (var occupier in boardPokes)
            {
                MoveToLinks(occupier);
            }

            var movablePokes = rootBoardTree._children.Values.Where(cv => (cv.PokeMoved.CanMove && !cv.HasBeenLinked));
            while (movablePokes.Count() > 0)
            {
                for (int i = 0; i < movablePokes.Count(); i++)
                {
                    var movablePoke = movablePokes.ElementAt(i);
                    var pokeToMove = movablePoke.Board.GetPokePosition(movablePoke.PokeMoved);
                    MoveToLinks(pokeToMove, movablePoke.Board, rootBoardTree);
                }

                //foreach (var movablePoke in movablePokes)
                //{
                    
                //}
                movablePokes = rootBoardTree._children.Values.Where(cv => (cv.PokeMoved.CanMove && !cv.HasBeenLinked));
            } 

            //foreach (var child in rootBoardTree._children.Values)
            //{
            //    Console.Write(child.Board.ToString());
            //}
            rootBoardTree.TreeToString();
            return string.Empty;
        }

        private void MoveToLinks(Position occupier, Board board, BoardTreeNode boardNode)
        {
            foreach (var link in occupier.Links)
            {
                if (occupier.Occupant.CanMove && board._Board[link.x, link.y].Occupant == null)
                {
                    var cloneBoard = DeepClone(board);
                    cloneBoard.movePokemon(occupier.guid, cloneBoard._Board[link.x, link.y].guid);
                    var treeNode = new BoardTreeNode(cloneBoard);
                    treeNode.PokeMoved = treeNode.Board._Board[link.x, link.y].Occupant;
                    treeNode.PokeDistanceMoved++;
                    boardNode.Add(treeNode);
                }
            }
            boardNode.GetChild(board.Signature).HasBeenLinked = true;
        }

        private void MoveToLinks(Position occupier)
        {
            foreach (var link in occupier.Links)
            {
                if (occupier.Occupant.CanMove && rootBoardTree.Board._Board[link.x, link.y].Occupant == null)
                {
                    var cloneBoard = DeepClone(rootBoardTree.Board);
                    cloneBoard.movePokemon(occupier.guid, cloneBoard._Board[link.x, link.y].guid);
                    var treeNode = new BoardTreeNode(cloneBoard);
                    treeNode.PokeMoved = treeNode.Board._Board[link.x, link.y].Occupant;
                    treeNode.PokeDistanceMoved++;
                    rootBoardTree.Add(treeNode);
                }
            }
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
                    if (boardNode._children.ContainsKey(cloneBoard.Signature))
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

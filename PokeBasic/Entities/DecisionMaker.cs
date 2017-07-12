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
            var currentTeam = Teams.Own;
            var currentTeamAlternator = true;
            for (int i = 0; i < depth; i++)
            {
                currentTeam = currentTeamAlternator ? Teams.Own : Teams.Opponent;
                currentTeamAlternator = !currentTeamAlternator;
                List<BoardTreeNode> nodesAtDepth = rootBoardTree.GetNodesAtLevel(i);
                
                Console.WriteLine("Reactor depth {0}, team {1}", i, currentTeam.ToString());
                foreach (var nodeAtDepth in nodesAtDepth)
                {
                    if (nodeAtDepth.PokeMoved != null)
                    {
                        nodeAtDepth.Board.GetPokePosition(nodeAtDepth.PokeMoved).Occupant.DistanceMoved = 0;
                        nodeAtDepth.PokeDistanceMoved = 0;
                    }
                    var benchPokes = new List<Position>();
                    if (currentTeam.Equals(Teams.Own))
                    {
                        benchPokes = nodeAtDepth.Board._MyBench.Where(mb => (mb.Occupant != null && mb.SelfCoords.x < 6)).ToList();
                    }
                    else
                    {
                        benchPokes = nodeAtDepth.Board._OpponentBench.Where(mb => (mb.Occupant != null && mb.SelfCoords.x < 6)).ToList();
                    }
                    foreach (var benchPoke in benchPokes)
                    {
                        benchPoke.Occupant.Movement--;
                        MoveToBoard(benchPoke, nodeAtDepth.Board, nodeAtDepth, true);
                    }

                    var boardPokes = nodeAtDepth.Board.GetOccupiedBoard().Where(oc => (oc.Occupant.Team.Equals(currentTeam) && oc.Occupant.CanMove));
                    foreach (var occupier in boardPokes)
                    {
                        MoveToLinks(occupier, nodeAtDepth.Board, nodeAtDepth, true);
                    }

                    var movablePokes = nodeAtDepth._children.Values.Where(cv => (cv.PokeMoved.CanMove && !cv.HasBeenLinked));
                    while (movablePokes.Count() > 0)
                    {
                        for (int k = 0; k < movablePokes.Count(); k++)
                        {
                            var movablePoke = movablePokes.ElementAt(k);
                            var pokeToMove = movablePoke.Board.GetPokePosition(movablePoke.PokeMoved);
                            MoveToLinks(pokeToMove, movablePoke.Board, nodeAtDepth);
                        }
                        movablePokes = nodeAtDepth._children.Values.Where(cv => (cv.PokeMoved.CanMove && !cv.HasBeenLinked));
                    }

                }
            }
            //TODO distance move reset
            //Console.Write(rootBoardTree.TreeToString());

            return string.Empty;
        }

        private void MoveToLinks(Position occupier, Board board, BoardTreeNode boardNode, bool first = false)
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
            if (!first) boardNode.GetChild(board.Signature).HasBeenLinked = true;
        }

        private void MoveToBoard(Position occupier, Board board, BoardTreeNode boardNode, bool first = false)
        {
            var sourceBench = occupier.Team.Equals(Teams.Own) ? board._MyBench : board._OpponentBench;
            var links = new List<Coords>();
            if (occupier.Team.Equals(Teams.Own))
            {
                links.Add(new Coords(4, 0));
                links.Add(new Coords(4, 6));
            }
            else
            {
                links.Add(new Coords(0, 0));
                links.Add(new Coords(0, 6));
            }
            foreach (var link in links)
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
            if (!first) boardNode.GetChild(board.Signature).HasBeenLinked = true;
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

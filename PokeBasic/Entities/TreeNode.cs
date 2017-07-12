using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBasic.Entities
{
    class BoardTreeNode : IEnumerable<BoardTreeNode>
    {
        public readonly Dictionary<string, BoardTreeNode> _children =
                                            new Dictionary<string, BoardTreeNode>();

        public string Id;
        public Board Board;
        public BoardTreeNode Parent { get; private set; }
        public int PokeDistanceMoved { get; set; }
        public Poke PokeMoved { get; set; }
        public bool HasBeenLinked { get; set; }
        public int Depth { get; set; }

        public BoardTreeNode(Board Board)
        {
            this.Board = Board;
            this.Id = Board.Signature;
            this.HasBeenLinked = false;
            this.Depth = 0;
        }

        public BoardTreeNode GetChild(string BoardId)
        {
            return this._children[BoardId];
        }

        public void Add(BoardTreeNode item)
        {
            if (item.Parent != null)
            {
                item.Parent._children.Remove(item.Id);
            }

            //if (!IdExistsInTree(item.Id))
            //{
            if ( item.Id != this.Id && !this._children.Values.Any(c => (c.Id.Equals(item.Id))))
            {
                item.Parent = this;
                item.Depth = this.Depth + 1;
                this._children.Add(item.Id, item);
            }
                //Console.WriteLine("----------------------------------------------------");
                //this.TreeToString();
                //Console.WriteLine("----------------------------------------------------");
            //}
        }

        public IEnumerator<BoardTreeNode> GetEnumerator()
        {
            return this._children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this._children.Count; }
        }

        public BoardTreeNode GetRoot()
        {
            BoardTreeNode result = this;
            while (result.Parent != null)
            {
                result = result.Parent;
            }
            return result;
        }

        public List<BoardTreeNode> GetNodesAtLevel(int level)
        {
            var result= new List<BoardTreeNode>();

            if (this.Depth == level)
            {
                result.Add(this);
            }
            else if (this._children.Count > 0 && this._children.ElementAt(0).Value.Depth == level)
            {
                result = this._children.Values.ToList();
            }
            else
            {
                foreach (var child in _children.Values)
                {
                    result.AddRange(child.GetNodesAtLevel(level));
                }
            }
            return result;
        }

        public bool IdExistsInTree(string Id)
        {
            BoardTreeNode root = this.GetRoot();
            return IdExistsInTreeRecursive(Id);
        }
        
        public bool IdExistsInTreeRecursive(string Id)
        {
            if (this._children.Count > 0)
            {
                var found = this._children.Values.Where(c => (c.Id.Equals(Id))).ToList();
                if (found.Count > 0)
                {
                    return true;
                }
                else
                {
                    foreach (var child in this._children.Values)
                    {
                        var res = child.IdExistsInTreeRecursive(Id);
                        if (res)
                            return true;
                    }
                }
            }
            return false;

        }

        public string ToString(int tabs)
        {
            var tostr = new StringBuilder();
            while (tabs > 0)
            {
                tostr.AppendFormat("\t");
                tabs--;
            }
            return tostr.AppendFormat(this.ToString()).ToString();
        }

        public override string ToString()
        {
            return Board.Signature;
        }

        public string LevelToString(string indent, bool last)
        {
            var res = new StringBuilder();
            res.Append(indent);
            if (last)
            {
                res.Append("\\-");
                indent += "  ";
            }
            else
            {
                res.Append("|-");
                indent += "| ";
            }
            res.AppendFormat("{0}:{1}\n", this.Depth, this.Board.Signature);

            //for (int i = 0; i < this._children.Count; i++)

            int i = 0;
            foreach (var ch in this._children.Values)
            {
                res.Append(ch.LevelToString(indent, i == this._children.Count - 1));
            }
            return res.ToString();
        }

        public string TreeToString()
        {
            var result = new StringBuilder();
            var root = this.GetRoot();
            return root.LevelToString("", true);
        }
    }
}

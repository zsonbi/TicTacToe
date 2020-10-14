using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Node
    {
        private Node Parent;

        //Determines if the Node is a Leaf
        public bool IsLeaf { get; private set; }

        //Gets the MeanValue
        public float MeanValue { get; private set; }

        //The number of visits of this node and its children's
        public int NumberOfVisits { get; private set; }

        //The Value of the Node (how many score it gotten for the games)
        public short value { get; private set; }

        //The depth of the node in the tree (0 is the first node's)
        public short depth { get; private set; }

        //The action we took to get to this State
        public byte[] Action { get; private set; }

        //The state of the board
        public byte[,] State { get; private set; }

        //Which Side put the node in this state
        public bool Side { get; private set; }

        //Constructor for the elements in the tree
        public Node(Node parent, byte[,] State, byte[] action)
        {
            this.value = 0;
            this.depth = (short)(parent.depth + 1);
            this.Action = action;
            IsLeaf = true;
            NumberOfVisits = 0;
            this.State = State;
            this.Side = !parent.Side;
            this.Parent = parent;
        }

        //----------------------------------------------------------------
        //Constructor for the first Node in the tree
        public Node(byte[,] State, byte[] action, bool aiSide)
        {
            this.value = 0;
            this.depth = 0;
            this.Action = action;
            IsLeaf = true;
            NumberOfVisits = 0;
            this.State = State;
            this.Side = aiSide;
        }

        //---------------------------------------------------------------
        //calculates the MeanValue
        public void CalcMeanValue(int numberOfTotalVisits)
        {
            if (NumberOfVisits == 0)
                this.MeanValue = float.MaxValue;
            else
                MeanValue = (float)(this.value + 4 * Math.Sqrt(Math.Log10(numberOfTotalVisits) / NumberOfVisits));
        }

        //---------------------------------------------------------
        //Updates the value of this node and it's parent's
        public void Update(sbyte value)
        {
            NumberOfVisits++;
            if (depth == 0)
            {
                return;
            }
            this.value += value;
            Parent.Update(value);
        }

        //------------------------------------------
        //changes IsLeaf to false that's it
        public void NotLeafAnyMore()
        {
            this.IsLeaf = false;
        }
    }
}
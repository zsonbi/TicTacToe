using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Node
    {
        //The parent of the node
        private Node parent;

        //The Value of the Node (How many score it has gotten)
        public int Value { get; private set; }

        //The MeanValue of the Node
        public double MeanValue { get; private set; }

        //The Node's Children
        public List<Node> Children { get; private set; }

        //How many times has this node been visite
        public int NumberOfVisits { get; private set; }

        //The depth of the Node in the tree
        public short Depth { get; private set; }

        //The Action we took to get here
        public IAction Action { get; private set; }

        //The State of the Node
        public IState State { get; private set; }

        //-----------------------------------------------------
        //Normal Constructor
        public Node(Node parent, IAction action, IState state)
        {
            NumberOfVisits = 0;
            this.parent = parent;
            Children = new List<Node>();
            Depth = (short)(parent.Depth + 1);
            this.Action = action;
            this.State = state;
        }

        //---------------------------------------------------------
        //Constructor for the root node
        public Node(IState state, IAction action)
        {
            State = state;
            NumberOfVisits = 0;
            Children = new List<Node>();
            parent = null;
            Depth = 0;
            this.Action = action;
        }

        //**************************************************************************
        //Public Methods
        /// <summary>
        ///Calculates the MeanValue if it's never been visited returns double.MaxValue
        /// </summary>
        /// <param name="numberOfTotalVisits"> The number of visits of the root node</param>
        /// <returns>The Calculated MeanValue</returns>
        public double CalcMeanValue(int numberOfTotalVisits)
        {
            if (NumberOfVisits == 0)
                MeanValue = double.MaxValue;
            else
                MeanValue = (double)(Value / NumberOfVisits) + 14.1 * Math.Sqrt(Math.Log(numberOfTotalVisits) / this.NumberOfVisits);

            return MeanValue;
        }

        //-------------------------------------------------------------------
        //Adds a new Child to the Node
        public void AddChild(Node Child)
        {
            Children.Add(Child);
        }

        //--------------------------------------------------------------------
        //Updates this Node and it's parents Value and number of visits
        public void Update(byte Winner)
        {
            if (Winner == 0)
            {
                Value += 5;
            }
            else if (Winner == Action.player)
                Value += 10;
            NumberOfVisits += 1;
            if (parent != null)
                parent.Update(Winner);
        }
    }
}
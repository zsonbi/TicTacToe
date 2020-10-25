using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class MCTree
    {
        private List<List<Node>> levels = new List<List<Node>>();//The levels of the tree

        //Returns the RootNode
        public Node Root { get => levels[0][0]; }

        public MCTree()
        {
        }

        //******************************************************
        //Private Mehods
        //Searches the tree recursively (maxDepth is the depth how far it should search in the tree)
        private Node Search(Node parent, short maxDepth = short.MaxValue)
        {
            if (parent.depth + 1 >= levels.Count() || maxDepth < parent.depth + 1)
                return parent;

            short highestMeanValueIndex = -1;

            for (short i = 0; i < levels[parent.depth + 1].Count; i++)
            {
                if (levels[parent.depth + 1][i].Parent == parent)
                {
                    if (highestMeanValueIndex == -1)
                    {
                        levels[parent.depth + 1][i].CalcMeanValue(MCTSAI.numberOfTotalVisits);
                        highestMeanValueIndex = i;
                    }
                    else if (levels[parent.depth + 1][highestMeanValueIndex].MeanValue < levels[parent.depth + 1][i].CalcMeanValue(MCTSAI.numberOfTotalVisits))
                    {
                        highestMeanValueIndex = i;
                    }//if
                }//if
            }//for

            return highestMeanValueIndex == -1 ? parent : Search(levels[parent.depth + 1][highestMeanValueIndex], maxDepth);
        }

        //----------------------------------------------------------
        //Updates the MeanValue of each item
        private void UpdateMeanValues()
        {
            foreach (var level in levels)
            {
                foreach (var node in level)
                {
                    node.CalcMeanValue(MCTSAI.numberOfTotalVisits);
                }//forea
            }//forea
        }

        //********************************************************
        //Public Methods
        //Selects the element with the highest meanValue
        public Node Select()
        {
            return Search(levels[0][0]);
        }

        //-------------------------------------------------------
        //Adds a Child element to the tree
        public void Add(Node Child)
        {
            if (Child.depth >= levels.Count)
            {
                levels.Add(new List<Node>());
            }//if
            levels[Child.depth].Add(Child);
        }

        //---------------------------------------------------------------
        //Clears the tree of all of it's elements
        public void Clear()
        {
            levels.Clear();
            GC.Collect();
        }

        //-----------------------------------------------------------------
        //Selects the best Node up to a certain level
        public Node SelectUpToDepth(short maxDepth)
        {
            return Search(levels[0][0], maxDepth);
        }

        //------------------------------------------------------------------
        //Searches for the best Node with the highest value/numberOfVisits
        public byte[] SearchForBestMove()
        {
            short Index = -1;
            float value = float.MinValue;
            for (short i = 0; i < levels[1].Count(); i++)
            {
                if ((float)levels[1][i].value / levels[1][i].NumberOfVisits > value)
                {
                    Index = i;
                    value = (float)levels[1][i].value / levels[1][i].NumberOfVisits;
                }
            }

            return levels[1][Index].Action;
        }
    }
}
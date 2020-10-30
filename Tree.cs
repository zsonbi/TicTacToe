using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Tree
    {
        //The root Node of the tree
        public Node root { get; private set; }

        //----------------------------------------------------
        //Constructor sets the root node
        public Tree(Node rootNode)
        {
            root = rootNode;
        }

        //*********************************************************************
        //Private Methods
        //Finds the Node with the highest MeanValue from the parent's Children
        private Node FindBiggestNode(Node parent)
        {
            Node output = parent.Children[0];
            output.CalcMeanValue(root.NumberOfVisits);
            foreach (var item in parent.Children)
            {
                if (output.MeanValue < item.CalcMeanValue(root.NumberOfVisits))
                {
                    output = item;
                }
            }
            return output;
        }

        //**********************************************************************
        //Public Methods
        //Selects the best Node from the tree
        public Node Select(Node rootNode)
        {
            Node Selected = rootNode;
            while (Selected.Children.Count != 0)
            {
                Selected = FindBiggestNode(Selected);
            }
            return Selected;
        }
    }
}
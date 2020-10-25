using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class MCTSAI : IAI
    {
        //Varriables

        private IState currentState;//The current state of the board
        private IState simulationState;
        private MCTree tree = new MCTree();
        private int itteration = 6000;
        private readonly Random rnd = new Random();

        //********************************************************************
        //Properties
        //Which side does the ai plays as
        public bool aiSide { get; private set; }

        //The number of total visits
        public static int numberOfTotalVisits { get; private set; }

        //**********************************************************************
        //Constructor
        public MCTSAI(bool aiSide, IState current)
        {
            this.currentState = current;
            this.aiSide = aiSide;
            simulationState = new State();
        }

        //************************************************************************
        //Private Methods
        //Expands the node so it will have children (only if the node is leaf)
        private void Expand(Node parent)
        {
            if (!parent.IsLeaf)
            {
                throw new Exception("Not Leaf");
            }//if
            parent.NotLeafAnyMore();
            simulationState.ImportState(parent.State);
            simulationState.Change(parent.Action[0], parent.Action[1], parent.Side);
            byte[][] possActions = simulationState.PossMoves();

            for (short i = 0; i < possActions.GetLength(0); i++)
            {
                Node child = new Node(parent, simulationState.ExportState(), possActions[i]);
                tree.Add(child);
            }//for
        }

        //------------------------------------------------------------
        //Simulate a game with random moves and return the outcome
        private void Simulate(Node input)
        {
            simulationState.ImportState(input.State);
            simulationState.Change(input.Action[0], input.Action[1], input.Side);
            bool tempSide = !input.Side;

            if (simulationState.isOver)
            {
                input.Update((sbyte)(simulationState.Draw ? 0 : simulationState.WhoWon == aiSide ? 100 : -100));
                return;
            }

            while (!simulationState.isOver)
            {
                byte[][] possActions = simulationState.PossMoves();
                short chosenMove = (short)rnd.Next(0, possActions.GetLength(0));
                simulationState.Change(possActions[chosenMove][0], possActions[chosenMove][1], tempSide);
                tempSide = !tempSide;
            }//while
            input.Update((sbyte)(simulationState.Draw ? 0 : simulationState.WhoWon == aiSide ? 1 : 0));
            numberOfTotalVisits++;
        }

        //-----------------------------------------------------------
        //After the tree search find the most promising move from the first layer
        private byte[] FindBestMove()
        {
            return tree.SelectUpToDepth(1).Action;
        }

        private void CreateBaseNodes()
        {
            byte[][] possActions = currentState.PossMoves();
            tree.Root.NotLeafAnyMore();

            for (short i = 0; i < possActions.GetLength(0); i++)
            {
                tree.Add(new Node(tree.Root, currentState.ExportState(), possActions[i]));
            }//for
        }

        //**********************************************************************
        //Public Methods
        //Finds the next possible move and returns it
        public async Task<byte[]> Next()
        {
            tree.Clear();
            tree.Add(new Node(currentState.ExportState(), new byte[] { 0, 0 }, !aiSide));
            CreateBaseNodes();

            numberOfTotalVisits = 0;

            for (int i = 0; i < itteration; i++)
            {
                Node selected = tree.Select();
                if (selected.NumberOfVisits != 1)
                    Simulate(selected);

                //if the node is a leaf node give him children
                else if (selected.IsLeaf)
                    Expand(selected);
                else
                    Simulate(selected);
            }//for

            return tree.SearchForBestMove();
        }
    }
}
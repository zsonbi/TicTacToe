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
        private List<IState> cloneStates = new List<IState>();//Clones of the original state each depth in the recursion function adds one more layer
        private IState simulationState;
        private List<Node> nodes = new List<Node>();
        private int itteration = 2000;
        private readonly Random rnd = new Random();
        private int numberOfTotalVisits = 0;

        //********************************************************************
        //Properties
        //Which side does the ai plays as
        public bool aiSide { get; private set; }

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
        //Selects the Node with the highest MeanValue
        private Node Select()
        {
            UpdateMeanValues();
            float biggestMeanValue = nodes.Where(x => x.IsLeaf).Max(j => j.MeanValue);
            return nodes.Where(x => x.IsLeaf).ToList().Find(x => x.MeanValue == biggestMeanValue);
        }

        //--------------------------------------------------------
        //Expands the node so it will have children (only if the node is leaf)
        private void Expand(Node parent)
        {
            if (!parent.IsLeaf)
            {
                throw new Exception("Not Leaf");
            }//if
            parent.NotLeafAnyMore();
            simulationState.ImportState(parent.State);
            byte[][] possActions = simulationState.PossMoves();

            for (short i = 0; i < possActions.GetLength(0); i++)
            {
                simulationState.ImportState(parent.State);
                simulationState.Change(possActions[i][0], possActions[i][1], !parent.Side);
                nodes.Add(new Node(parent, currentState.ExportState(), possActions[i]));
                if (simulationState.isOver)
                {
                    nodes.Last().Update((sbyte)(simulationState.Draw ? 1 : simulationState.WhoWon == aiSide ? 2 : -1));
                }
                else
                {
                    nodes.Last().Update(Simulate(!parent.Side));
                }
                numberOfTotalVisits++;
            }
        }

        //------------------------------------------------------------
        //Simulate a game with random moves and return the outcome
        private sbyte Simulate(bool side)
        {
            while (!simulationState.isOver)
            {
                byte[][] possActions = simulationState.PossMoves();
                short chosenMove = (short)rnd.Next(0, possActions.GetLength(0));
                simulationState.Change(possActions[chosenMove][0], possActions[chosenMove][1], side);
                side = !side;
            }//while
            return (sbyte)(simulationState.Draw ? 1 : simulationState.WhoWon == aiSide ? 2 : -1);
        }

        //-----------------------------------------------------------
        //After the tree search find the most promising move from the first layer
        private byte[] FindBestMove()
        {
            short biggestvalue = nodes.Where(x => x.depth == 1).Max(x => x.value);
            return nodes.Where(x => x.depth == 1).ToList().Find(x => x.value == biggestvalue).Action;
        }

        //--------------------------------------------------------
        //Update the MeanValues of every Node
        private void UpdateMeanValues()
        {
            foreach (var item in nodes)
            {
                item.CalcMeanValue(numberOfTotalVisits);
            }
        }

        //**********************************************************************
        //Public Methods
        //Finds the next possible move and returns it
        public async Task<byte[]> Next()
        {
            nodes.Clear();
            nodes.Add(new Node(currentState.ExportState(), new byte[] { 0, 0 }, aiSide));
            for (int i = 0; i < itteration; i++)
            {
                Node selected = Select();
                //if the node is a leaf node give him children
                if (selected.Equals(null))
                {
                    break;
                }
                Expand(selected);
            }//for

            return FindBestMove();
        }
    }
}
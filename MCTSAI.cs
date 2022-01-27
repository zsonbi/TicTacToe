using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class MCTSAI : IAI
    {
        private IState currState; //The real state of the game
        private IState simulationState;//The state of the game which we will use to test the moves
        private Random rnd = new Random();//Random -,-
        private Tree tree;//The tree
        private byte NumberOfPlayers = 2;//How many players are there
        private byte level = 4; //The level of the ai

        //The player as the AI plays
        public byte aiSide { get; private set; }

        //----------------------------------------------------------------
        //Constructor
        public MCTSAI(byte aiSide, IState currentState)
        {
            this.currState = currentState;
            this.aiSide = aiSide;
            simulationState = new State();
        }

        //**********************************************************************
        //Private Methods
        //Returns the next player
        private byte NextPlayer(byte current)
        {
            return (byte)(current - 1 == 0 ? NumberOfPlayers : current - 1);
        }

        //---------------------------------------------------------------------
        //returns the previous player
        private byte PrevPlayer(byte current)
        {
            return (byte)(current + 1 > NumberOfPlayers ? 1 : current + 1);
        }

        //-------------------------------------------------------------
        //Finds the best move which has the highest winrate
        private IAction FindBestMove()
        {
            Node best = tree.root.Children[0];
            double bestvalue = best.Value / best.NumberOfVisits;
            foreach (var item in tree.root.Children)
            {
                if (bestvalue < item.Value / item.NumberOfVisits)
                {
                    best = item;
                    bestvalue = item.Value / item.NumberOfVisits;
                }
            }
            return best.Action;
        }

        //----------------------------------------------------------------
        //Expands the Node so it will have children
        private void Expand(Node parent)
        {
            simulationState.ImportState(parent.State);

            if (parent.Action.Move != null) //If it's not the root node
                simulationState.Change(parent.Action);
            if (simulationState.isOver)
            {
                return;
            }

            //Gets the possible Actions
            IAction[] possActions = simulationState.PossMoves(NextPlayer(parent.Action.player));
            foreach (var item in possActions)
            {
                State tempState = new State();
                tempState.ImportState(simulationState);
                Node Child = new Node(parent, item, tempState);
                parent.AddChild(Child);
            }
        }

        //--------------------------------------------------------------------
        //Make a random simulation of the game
        private void Simulate(Node selected)
        {
            simulationState.ImportState(selected.State);

            if (selected.Action.Move != null)
                simulationState.Change(selected.Action);
            //So that we know which player comes next
            byte currPlayer = NextPlayer(selected.Action.player);
            while (!simulationState.isOver)
            {
                IAction[] possActions = simulationState.PossMoves(currPlayer);
                simulationState.Change(possActions[rnd.Next(0, possActions.Length)]);
                currPlayer = NextPlayer(currPlayer);
            }
            //Updates the Value of the Node
            selected.Update(simulationState.whoWon);
        }

        //**********************************************************
        //Public Methods
        //Finds and Returns the best possible move
        public async Task<IAction> Next()
        {
            Stopwatch stopwatch;//This will keep track of the elapsed time
            State tempState = new State();
            tempState.ImportState(currState);
            tree = new Tree(new Node(tempState, new Action(PrevPlayer(aiSide))));
            Expand(tree.root);//Expands the root node so that we know the poss moves
            double end = (level + 1) * 50; //The amount of time the algorithm spends on each poss move

            for (int i = 0; i < tree.root.Children.Count; i++)
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();

                while (stopwatch.ElapsedMilliseconds <= end)
                {
                    Node selected = tree.Select(tree.root.Children[i]);
                    if (selected.NumberOfVisits == 0)
                        Expand(selected);

                    Simulate(selected);
                }
            }
            return FindBestMove();
        }
    }
}
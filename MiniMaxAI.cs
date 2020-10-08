using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class MiniMaxAI : IAI
    {
        //Properties
        public bool aiSide { get; private set; }

        private IState currentState;
        private List<IState> previousStates = new List<IState>();

        public MiniMaxAI(bool aiSide, IState current)
        {
            this.aiSide = aiSide;
            this.currentState = current;
        }

        //***************************************************************
        //Private Methods
        //Recursive call go through actions and returns their score(win 1, draw 0, lose -1)
        private sbyte Recursion(short depth, bool Side)
        {
            //If the program needs more layers make
            if (previousStates.Count == depth)
            {
                previousStates.Add(new State());
            }//if
            previousStates[depth].ImportState(previousStates[depth - 1].ExportState());//imports the previous layer's state(when it still got the move it made)
            byte[][] possActions = previousStates[depth].PossMoves();//Gets the possible actions
            sbyte[] values = new sbyte[possActions.Length];//The values of the moves
            for (int i = 0; i < possActions.Length; i++)
            {
                //reset the "depth" layer state
                previousStates[depth].ImportState(previousStates[depth - 1].ExportState());
                //Make one of the possible moves
                previousStates[depth].Change(possActions[i][0], possActions[i][1], Side);
                //Check if the game ended with the move
                if (previousStates[depth].isOver)
                {
                    if (previousStates[depth].Draw)
                        values[i] = 0; //draw 0 -,-
                    else
                        //If the winner matches the side which the ai controls get 1 else -1
                        values[i] = (sbyte)(previousStates[depth].WhoWon == aiSide ? 1 : -1);
                    continue;
                }//if
                //Call the Recursion Function which will return a value 1 win 0 draw -1 lose
                values[i] = Recursion((short)(depth + 1), !Side);
            }//for
            //if the Side matches the aiSide find the most optimal move for him else find the worst
            return Side == aiSide ? FindLargestValue(values) : FindSmallestValue(values);
        }

        //-------------------------------------------------------------------
        //Returns the index of the largest element in the array
        private short FindLargestIndex(sbyte[] input)
        {
            short LargestIndex = 0;
            for (short i = 1; i < input.Length; i++)
            {
                if (input[LargestIndex] < input[i])
                {
                    LargestIndex = i;
                }//if
            }//for
            return LargestIndex;
        }

        //-------------------------------------------------------
        //Returns the index of the smallest element in the array
        private short FindSmallestIndex(sbyte[] input)
        {
            short SmallestIndex = 0;
            for (short i = 1; i < input.Length; i++)
            {
                if (input[SmallestIndex] > input[i])
                {
                    SmallestIndex = i;
                }//if
            }//for
            return SmallestIndex;
        }

        //---------------------------------------------------------
        //Returns the Smallest value from the array
        private sbyte FindSmallestValue(sbyte[] input)
        {
            return input[FindSmallestIndex(input)];
        }

        //-------------------------------------------------------------
        //Returns the biggest value from the array
        private sbyte FindLargestValue(sbyte[] input)
        {
            return input[FindLargestIndex(input)];
        }

        //*******************************************************************
        //Public Methods
        //Determines the Next best move
        public async Task<byte[]> Next()
        {
            //So we don't leave junk here
            previousStates.Clear();
            byte[][] possActions = currentState.PossMoves();
            //The score for the moves
            sbyte[] score = new sbyte[possActions.Length];
            //Add the 0. layer State
            previousStates.Add(new State());
            for (int i = 0; i < possActions.Length; i++)
            {
                //Reset the 0 layer state
                previousStates[0].ImportState(currentState.ExportState());
                //We make one of the possible moves
                previousStates[0].Change(possActions[i][0], possActions[i][1], aiSide);
                //Call the Recursion Function which will return a value 1 win 0 draw -1 lose
                score[i] = Recursion(1, !aiSide);
            }//for

            return possActions[FindLargestIndex(score)];
        }
    }
}
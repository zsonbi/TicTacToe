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
        private sbyte Recursion(short depth, bool Side)
        {
            if (previousStates.Count == depth)
            {
                previousStates.Add(new State());
            }
            previousStates[depth].ImportState(previousStates[depth - 1].ExportState());
            byte[][] possActions = previousStates[depth].PossMoves();
            sbyte[] values = new sbyte[possActions.Length];
            for (int i = 0; i < possActions.Length; i++)
            {
                previousStates[depth].ImportState(previousStates[depth - 1].ExportState());
                previousStates[depth].Change(possActions[i][0], possActions[i][1], Side);
                if (previousStates[depth].isOver)
                {
                    if (previousStates[depth].Draw)
                        values[i] = 0;
                    else
                        values[i] = (sbyte)(previousStates[depth].WhoWon == aiSide ? 1 : -1);
                    continue;
                }

                values[i] = Recursion((short)(depth + 1), !Side);
            }//for

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
            previousStates.Clear();
            byte[] output = new byte[2];
            byte[][] possActions = currentState.PossMoves();
            sbyte[] score = new sbyte[possActions.Length];
            previousStates.Add(new State());
            previousStates[0].ImportState(currentState.ExportState());
            for (int i = 0; i < possActions.Length; i++)
            {
                previousStates[0].Change(possActions[i][0], possActions[i][1], aiSide);
                score[i] = Recursion(1, !aiSide);
                previousStates[0].ImportState(currentState.ExportState());
            }//for

            return possActions[FindLargestIndex(score)];
        }
    }
}
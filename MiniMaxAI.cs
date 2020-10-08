using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class MiniMaxAI
    {

        //Properties
        public bool aiSide { get; private set; }
        State currentState;
        List<State> previousStates= new List<State>();


        public MiniMaxAI(bool aiSide,State current)
        {
            this.aiSide = aiSide;
            this.currentState = current;
        }

        //***************************************************************
        //Private Methods
        private sbyte Recursion(byte y, byte x,short depth,bool Side)
        {
            if(previousStates.Count == depth)
            {
                previousStates.Add(new State());
            }
            byte[][] possActions = previousStates[depth].PossMoves();
            sbyte[] values = new sbyte[possActions.Length];
            for (int i = 0; i < possActions.Length; i++)
            {
                values[i] = Recursion(y, x, depth, !Side);
            }//for

            return Side == aiSide ? FindSmallestValue(values) : FindLargestValue(values);

        }

        private short FindLargestIndex(sbyte[] input)
        {
            short LargestIndex = 0;
            for (short i = 1; i < input.Length; i++)
            {
                if(input[LargestIndex]< input[i])
                {
                    LargestIndex = i;
                }
            }
            return LargestIndex;
        }

        private sbyte FindSmallestValue(sbyte[] input)
        {
            short SmallestIndex = 0;
            for (short i = 1; i < input.Length; i++)
            {
                if (input[SmallestIndex] > input[i])
                {
                    SmallestIndex = i;
                }
            }
            return input[SmallestIndex];
        }

        private sbyte FindLargestValue(sbyte[] input)
        {
            return input[FindLargestIndex(input)];
        }

        //*******************************************************************
        //Public Methods
        //Determines the Next best move
        public async Task<byte[]> Next()
        {
            byte[] output=new byte[2];
            byte[][] possmoves = currentState.PossMoves();
            sbyte[] score = new sbyte[possmoves.Length];
            for (int i = 0; i < possmoves.Length; i++)
            {
                score[i] = Recursion(possmoves[i][0], possmoves[i][1], 0,aiSide);
            }//for


            return possmoves[FindLargestIndex(score)];
        }

    }
}

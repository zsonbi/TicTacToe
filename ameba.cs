﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class ameba
    {
        //Varriables
        private State gameState;
        private bool isAiControlled;
        MiniMaxAI ai;

        //**********************************************************
        //Properties

        public bool isOver { get => gameState.isOver; }
        public bool whoWon { get => gameState.WhoWon; }
        public byte[][] winnerCells { get => gameState.WinnerCells.ToArray(); }

        //Static Properties

        public static byte X { get; private set; }//Alapértelmezetten x tengely méret (3)
        public static byte Y { get; private set; }//Alapértelmezetten y tengely méret (3)
        public static byte Checksize { get; private set; }//Alapértelmezetten mennyinek kell kigyülnie a győzelemhez (3)
        public static bool Side { get; private set; }//the current playing side true 'X' false 'O'

        //Constructor
        public ameba(byte x = 3, byte y = 3, byte checksize = 3,bool aiControlled=false)
        {
            X = x;
            Y = y;
            Checksize = checksize;
            Side = true;
            gameState = new State();
            this.isAiControlled = aiControlled;
            if (aiControlled)
            {
                ai = new MiniMaxAI(true, gameState);
            }//if
        }

        //**************************************************************************
        //Public method
        //Changes the indexed element to the side which comes next
        public void Change(byte y, byte x)
        {
            gameState.Change(y, x);
            Side = !Side;
        }
    }
}
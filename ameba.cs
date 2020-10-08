using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class ameba
    {
        //Varriables

        private State gameState; //The gameboard basicly, but it can also check if someone won

        private IAI ai; //ai WOW it must be smart, no it's not

        //**********************************************************
        //Properties
        //Returns if the game is over
        public bool isOver { get => gameState.isOver; }

        //Get the cells which won the game
        public byte[][] winnerCells { get => gameState.WinnerCells.ToArray(); }

        //Static Properties
        //x size of the board default 3
        public static byte X { get; private set; }

        //y size of the board default 3
        public static byte Y { get; private set; }

        //How many 'X' or 'O' do the players need to win
        public static byte Checksize { get; private set; }

        //the current playing side true 'X' false 'O'
        public static bool Side { get; private set; }

        //Constructor
        public ameba(byte x = 3, byte y = 3, byte checksize = 3, bool aiControlled = false)
        {
            X = x;
            Y = y;
            Checksize = checksize;
            Side = true;
            gameState = new State();
            if (aiControlled)
            {
                ai = new MiniMaxAI(false, gameState);
            }//if
        }

        //**************************************************************************
        //Public method
        //Changes the indexed element to the side which comes next
        public async Task Change(byte y, byte x)
        {
            gameState.Change(y, x, ameba.Side);

            Side = !Side;
        }

        //------------------------------------------------------------------
        //Gets the next move of the bot
        public async Task<byte[]> Next()
        {
            return await ai.Next();
        }
    }
}
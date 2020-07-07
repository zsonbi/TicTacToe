using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class AI
    {
        //Varriables

        private PlayField current;
        private PlayField nextState;
        private int[,] importancechart;
        private byte x;
        private byte y;
        private byte depth = 2;

        //Properties

        //--------------------------------------------------------------------------------
        //Konstruktor
        public AI(PlayField be)
        {
            this.x = be.X;
            this.y = be.Y;
            current = be;
            nextState = new PlayField(be);
            importancechart = new int[y, x];
        }

        //-----------------------------------------------------------------------------
        //Megkeresi a következő legjobb lépést
        public byte[] next()
        {
            return Findbiggest();
        }

        //--------------------------------------------------------------------------------
        //Megkeresi a legnagyobb értéket az importancechartban
        private byte[] Findbiggest()
        {
            byte indexx = 0;
            byte indexy = 0;
            int biggest = int.MinValue;

            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    if (biggest < importancechart[i, j])
                    {
                        indexy = i;
                        indexx = j;
                        biggest = importancechart[i, j];
                    }
                }
            }
            return new byte[2] { indexy, indexy };
        }
    }
}
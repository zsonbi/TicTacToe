using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Checker
    {
        //Protected Varriables
        protected cell[,] cells;

        protected byte x;
        protected byte y;

        //Varriables
        private byte Checksize;

        //-------------------------------------------
        //Konstruktor
        public Checker(byte y, byte x, byte Checksize)
        {
            this.x = x;
            this.y = y;
            this.Checksize = Checksize;
            cells = new cell[y, x];
            //Feltöltjük a cells-t 2d-s tömböt
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    cells[i, j] = new cell();
                }//for
            }//for
        }

        //--------------------------------------------
        protected void Check()
        {
        }
    }
}
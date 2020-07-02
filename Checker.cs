using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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
        private bool foundwinner = false;
        private bool winner;
        private byte xcord = 0;
        private byte ycord = 0;
        private byte counter = 0;
        private byte currentType = 0;

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
            //y tengelyen a vizsgálat
            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    if (InsideCheck(i, j))
                        break;
                }
            }

            currentType = 0;
            counter = 0;

            //x tengelyen a vizsgálat
            for (byte i = 0; i < x; i++)
            {
                for (byte j = 0; j < y; j++)
                {
                    if (InsideCheck(j, i))
                        break;
                }
            }
        }

        //----------------------------------------------------------------------
        //Az ismétlés csökkentése
        private bool InsideCheck(byte ycord, byte xcord)
        {
            if (cells[ycord, xcord].Type == currentType && currentType != 0)
            {
                counter++;
            }
            else
            {
                currentType = cells[ycord, xcord].Type;
                counter = 0;
            }
            if (counter == Checksize)
            {
                foundwinner = true;
                winner = currentType == 1;
                return true;
            }
            return false;
        }
    }
}
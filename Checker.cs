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

        private byte checksize;
        private bool foundwinner = false;
        private bool winner;
        private sbyte xcord = 0;
        private sbyte ycord = 0;
        private byte counter = 1;
        private byte currentType = 0;

        //-------------------------------------------
        //Konstruktor
        public Checker(byte y, byte x, byte Checksize)
        {
            this.x = x;
            this.y = y;
            this.checksize = Checksize;
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
        protected bool Check()
        {
            //y tengelyen a vizsgálat
            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    if (InsideCheck(i, j))
                        return true;
                }
                counter = 1;
                currentType = 0;
            }

            //x tengelyen a vizsgálat
            for (byte i = 0; i < x; i++)
            {
                for (byte j = 0; j < y; j++)
                {
                    if (InsideCheck(j, i))
                        return true;
                }
                counter = 1;
                currentType = 0;
            }

            //x tengely mentén vizsgálat jobbra
            for (sbyte i = 0; i < x; i++)
            {
                xcord = i;
                while (xcord < x && ycord < y)
                {
                    if (InsideCheck((byte)ycord, (byte)xcord))
                    {
                        return true;
                    }
                    xcord++;
                    ycord++;
                }
                ycord = 0;
                counter = 1;
                currentType = 0;
            }

            //x tengely mentén vizsgálat balra
            for (sbyte i = (sbyte)(x - 1); i >= 0; i--)
            {
                xcord = i;
                while (xcord >= 0 && ycord < y)
                {
                    if (InsideCheck((byte)ycord, (byte)xcord))
                    {
                        return true;
                    }
                    xcord--;
                    ycord++;
                }
                ycord = 0;
                counter = 1;
                currentType = 0;
            }
            xcord = 0;
            ycord = 0;
            counter = 0;
            currentType = 0;
            return false;
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
                counter = 1;
            }
            if (counter == checksize)
            {
                foundwinner = true;
                winner = currentType == 1;
                return true;
            }
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;

namespace TicTacToe
{
    internal class PlayField
    {
        private cell[,] cells;
        private byte x;
        private byte y;

        //Konstruktor
        public PlayField(byte y, byte x)
        {
            this.x = x;
            this.y = y;
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

        //Az állapot változtatása
        public void Change(byte ycord, byte xcord, bool state)
        {
            cells[ycord, xcord].Change(state);
        }
    }
}
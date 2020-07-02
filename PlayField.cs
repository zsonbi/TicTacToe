using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class PlayField
    {
        private cell[,] cells;
        private byte x;
        private byte y;

        public PlayField(byte y, byte x)
        {
            this.x = x;
            this.y = y;
            cells = new cell[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    cells[i, j] = new cell();
                }
            }
        }
    }
}
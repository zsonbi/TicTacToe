using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;

namespace TicTacToe
{
    internal class PlayField : Checker
    {
        //Konstruktor
        public PlayField(byte y, byte x, byte Checksize) : base(y, x, Checksize)
        {
        }

        //Az állapot változtatása
        public void Change(byte ycord, byte xcord, bool state)
        {
            cells[ycord, xcord].Change(state);
        }
    }
}
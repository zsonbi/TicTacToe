using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;

namespace TicTacToe
{
    internal class PlayField : Checker
    {
        public bool over { get => base.Check(); }

        //Konstruktor
        public PlayField(byte y, byte x, byte Checksize) : base(y, x, Checksize)
        {
        }

        public PlayField(PlayField be) : base(be.Y, be.X, be.Checksize)
        {
        }

        //Az állapot változtatása
        public void Change(byte ycord, byte xcord, bool state)
        {
            cells[ycord, xcord].Change(state);
        }
    }
}
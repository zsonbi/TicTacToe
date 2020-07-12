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
        private int counter;

        public bool over { get => base.Check(); }
        public int Counter { get => counter; }

        //Konstruktor
        public PlayField(byte y, byte x, byte Checksize) : base(y, x, Checksize)
        {
            counter = x * y;
        }

        //------------------------------------------------------------
        //Az állapot változtatása
        public void Change(byte ycord, byte xcord, bool state)
        {
            cells[ycord, xcord].Change(state);
            counter--;
        }

        //------------------------------------------------------------
        //Egy másolat készítése gyakorlatilag
        public void MakePrevState(PlayField prevState)
        {
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    //Deep copy
                    this.cells[i, j] = new cell(prevState.cells[i, j].Type);
                }//for
            }//for
            counter = prevState.Counter;
        }

        //-----------------------------------------------------------------
        //Üres-e a megadott cella
        public bool IsCellEmpty(byte bey, byte bex)
        {
            return cells[bey, bex].Type == 0;
        }

        //-------------------------------------------------------------------
        //Az indexelt cella tartalma
        public byte GetCellType(byte[] index)
        {
            return cells[index[0], index[1]].Type;
        }
    }
}
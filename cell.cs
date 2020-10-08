using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class cell
    {
        //Varriables
        private byte type; //1-es az 'X' 2-es az 'O'

        //-----------------------------------------------------------------------
        //Properties
        public byte Type { get => type; }

        //------------------------------------------------------------------------
        //Konstruktor
        //A basic
        public cell()
        {
            type = 0;
        }

        //Konstruktor
        //Ha másolni szeretnénk
        public cell(byte type)
        {
            this.type = type;
        }

        //------------------------------------------------------------------------
        //Az érték változtatása
        public void Change(bool be)
        {
            //Ne lehessen felülírni(ez alapból sem történhet meg, de just in case)
            if (type != 0)
            {
                return;
            }//if
            //A megfelelő érték megadása
            type = be ? (byte)1 : (byte)2;
        }
    }
}
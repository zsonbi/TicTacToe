using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class cell
    {
        private byte type;

        public cell()
        {
            type = 0;
        }

        public void Change(bool be)
        {
            if (type != 0)
            {
                return;
            }
            type = be ? (byte)1 : (byte)2;
        }
    }
}
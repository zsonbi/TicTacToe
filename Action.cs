using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Action : IAction
    {
        //The Move itself
        public byte[] Move { get; private set; }

        //The player which performs the move
        public byte player { get; private set; }

        //-------------------------------------------------------
        //Constructor if we got the move
        public Action(byte[] Move, byte player)
        {
            this.Move = Move;
            this.player = player;
        }

        //--------------------------------------------------------
        //Constructor if we only got the player
        public Action(byte player)
        {
            this.Move = null;
            this.player = player;
        }
    }
}
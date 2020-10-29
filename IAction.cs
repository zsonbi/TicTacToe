using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal interface IAction
    {
        //The move itself
        byte[] Move { get; }

        //The player which performs the move
        byte player { get; }
    }
}
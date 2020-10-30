using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal interface IAI
    {
        //The side which the player plays as
        byte aiSide { get; }

        //Gets the Next best move
        Task<IAction> Next();
    }
}
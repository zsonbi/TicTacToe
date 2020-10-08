using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal interface IState
    {
        //If the game is over
        bool isOver { get; }

        //Who won true(player who comes first) false(player who comes second)
        bool WhoWon { get; }

        //If the game ended in draw
        bool Draw { get; }

        //Gets the Possible spaces where we can move
        byte[][] PossMoves();

        //Changes the tile/cell at the index to the given Side
        void Change(byte y, byte x, bool Side);

        //Exports the current state
        byte[,] ExportState();

        //Imports the state from the array
        void ImportState(byte[,] state);
    }
}
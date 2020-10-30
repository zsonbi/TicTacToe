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

        //Who won (0 is a draw)
        byte whoWon { get; }

        //Gets the Possible spaces where we can move
        IAction[] PossMoves(byte player);

        //Changes the tile/cell at the index to the given Side
        void Change(IAction action);

        //Exports the current state
        byte[,] ExportBoard();

        //Imports the state from the array
        void ImportState(IState state);
    }
}
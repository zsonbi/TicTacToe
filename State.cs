using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class State
    {
        private byte[,] board = new byte[TicTacToe.Y, TicTacToe.X];

        //******************************************************************
        //Properties
        //The bool value if the game is still in progress
        public bool isOver { get; private set; }

        public bool WhoWon { get; private set; }
        public List<byte[]> winnerCells { get; private set; }

        //*******************************************************************
        //Private Methods
        //Checks Horizontally for the win
        private void HorizontalCheck(byte y, byte x)
        {
            byte selected = board[y, x];
            byte indexer = 0;
            for (byte i = (byte)(x - 1); i >= 0; i--)
            {
                if (board[y, i] == selected)
                {
                    indexer++;
                }//if
            }//for
            for (byte i = (byte)(x - indexer); i < TicTacToe.X; i++)
            {
                if (board[y, i] != selected)
                {
                    break;
                }//if
                if (i - indexer == TicTacToe.Checksize)
                {
                    isOver = true;
                    WhoWon = 1 == selected;
                    for (byte c = (byte)(x - indexer); c <= i; c++)
                    {
                        winnerCells.Add(new byte[] { y, c });
                    }//for
                    return;
                }//if
            }//for
        }

        //-------------------------------------------------------------------
        //Checks Vertically for the win
        private void VerticalCheck(byte y, byte x)
        {
            byte selected = board[y, x];
            byte indexer = 0;
            for (byte i = (byte)(y - 1); i >= 0; i--)
            {
                if (board[i, x] == selected)
                {
                    indexer++;
                }//if
            }//for
            for (byte i = (byte)(y - indexer); i < TicTacToe.Y; i++)
            {
                if (board[i, x] != selected)
                {
                    break;
                }//if
                if (i - indexer == TicTacToe.Checksize)
                {
                    isOver = true;
                    WhoWon = 1 == selected;
                    for (byte c = (byte)(x - indexer); c <= i; c++)
                    {
                        winnerCells.Add(new byte[] { c, x });
                    }//for
                    return;
                }//if
            }//for
        }

        //*********************************************************************
        //Public Methods
        public void Change(byte y, byte x)
        {
            if (board[y, x] != 0)
            {
                throw new Exception("The cell is already occupied");
            }

            board[y, x] = TicTacToe.Side ? (byte)1 : (byte)2;
            HorizontalCheck(y, x);
            VerticalCheck(y, x);
        }
    }
}
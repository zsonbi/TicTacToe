using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class State
    {
        private byte[,] board = new byte[ameba.Y, ameba.X];
        private List<byte[]> winnerCells = new List<byte[]>();

        //******************************************************************
        //Properties
        //The bool value if the game is still in progress
        public bool isOver { get; private set; }

        public bool WhoWon { get; private set; }

        public List<byte[]> WinnerCells { get => winnerCells; }

        //*******************************************************************
        //Private Methods
        //Checks Horizontally for the win
        private void HorizontalCheck(byte y, byte x)
        {
            byte selected = board[y, x];
            sbyte indexer = -1;
            sbyte counter = 0;
            for (sbyte i = (sbyte)(x); i >= 0; i--)
            {
                if (board[y, i] == selected)
                {
                    indexer++;
                }//if
            }//for
            for (byte i = (byte)(x - indexer); i < ameba.X; i++)
            {
                if (board[y, i] != selected)
                {
                    break;
                }//if
                counter++;
                if (counter == ameba.Checksize)
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
            sbyte indexer = -1;
            sbyte counter = 0;
            for (sbyte i = (sbyte)(y); i >= 0; i--)
            {
                if (board[i, x] == selected)
                {
                    indexer++;
                }//if
            }//for
            for (byte i = (byte)(y - indexer); i < ameba.Y; i++)
            {
                if (board[i, x] != selected)
                {
                    break;
                }//if
                counter++;
                if (counter == ameba.Checksize)
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

            board[y, x] = ameba.Side ? (byte)1 : (byte)2;
            HorizontalCheck(y, x);
            VerticalCheck(y, x);
        }
    }
}
using System;
using System.Collections.Generic;

namespace TicTacToe
{
    internal class State : IState
    {
        private byte[,] board = new byte[ameba.Y, ameba.X];//A 2d array which stores the value of each cell 0-empty 1-'X' 2-'O'
        private List<byte[]> winnerCells = new List<byte[]>();//The indexes of the cell which won
        private short counter = (short)(ameba.X * ameba.Y);//The number of empty cells left

        //******************************************************************
        //Properties
        //The bool value if the game is still in progress
        public bool isOver { get; private set; }

        //Determines who won true-'X' false-'O'
        public byte whoWon { get; private set; }

        //The indexes of the cell which won
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
                    whoWon = selected;

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
                    whoWon = selected;

                    for (byte c = (byte)(y - indexer); c <= i; c++)
                    {
                        winnerCells.Add(new byte[] { c, x });
                    }//for
                    return;
                }//if
            }//for
        }

        //-------------------------------------------------------------------
        //Checks diagonally right
        private void DiagonalLeftCheck(byte y, byte x)
        {
            byte selected = board[y, x];
            sbyte indexer = -1;
            sbyte counter = 0;
            sbyte tempy = (sbyte)y;

            for (sbyte i = (sbyte)(x); i >= 0 && tempy >= 0; i--)
            {
                if (board[tempy, i] == selected)
                {
                    indexer++;
                }//if
                tempy--;
            }//for

            tempy = (sbyte)(y - indexer);
            for (byte i = (byte)(x - indexer); i < ameba.X && tempy < ameba.Y; i++)
            {
                if (board[tempy, i] != selected)
                {
                    break;
                }//if
                tempy++;
                counter++;
                if (counter == ameba.Checksize)
                {
                    isOver = true;
                    whoWon = selected;

                    tempy = (sbyte)(y - indexer);
                    for (byte c = (byte)(x - indexer); c <= i; c++)
                    {
                        winnerCells.Add(new byte[] { (byte)tempy, c });
                        tempy++;
                    }//for
                    return;
                }//if
            }//for
        }

        //-------------------------------------------------------------------
        //Checks diagonally right
        private void DiagonalRightCheck(byte y, byte x)
        {
            byte selected = board[y, x];
            sbyte indexer = -1;
            sbyte counter = 0;
            sbyte tempy = (sbyte)y;

            for (sbyte i = (sbyte)(x); i < ameba.X && tempy >= 0; i++)
            {
                if (board[tempy, i] == selected)
                {
                    indexer++;
                }//if
                tempy--;
            }//for

            tempy = (sbyte)(y - indexer);
            for (sbyte i = (sbyte)(x + indexer); i >= 0 && tempy < ameba.Y; i--)
            {
                if (board[tempy, i] != selected)
                {
                    break;
                }//if
                tempy++;
                counter++;
                if (counter == ameba.Checksize)
                {
                    isOver = true;
                    whoWon = selected;

                    tempy = (sbyte)(y - indexer);
                    for (sbyte c = (sbyte)(x + indexer); c >= 0; c--)
                    {
                        winnerCells.Add(new byte[] { (byte)tempy, (byte)c });
                        tempy++;
                    }//for
                    return;
                }//if
            }//for
        }

        //*********************************************************************
        //Public Methods
        //Changes the cell at the cordinate to the side value which comes next
        public void Change(IAction action)
        {
            if (board[action.Move[0], action.Move[1]] != 0)
            {
                throw new Exception("The cell is already occupied");
            }

            board[action.Move[0], action.Move[1]] = action.player;
            counter--;
            if (counter == 0)
            {
                this.whoWon = 0;
                this.isOver = true;
            }

            HorizontalCheck(action.Move[0], action.Move[1]);
            VerticalCheck(action.Move[0], action.Move[1]);
            DiagonalLeftCheck(action.Move[0], action.Move[1]);
            DiagonalRightCheck(action.Move[0], action.Move[1]);
        }

        //---------------------------------------------------------
        //Returns Empty places on the board (where the user can move)
        public IAction[] PossMoves(byte player)
        {
            List<IAction> output = new List<IAction>();
            for (byte i = 0; i < ameba.Y; i++)
            {
                for (byte j = 0; j < ameba.X; j++)
                {
                    if (board[i, j] == 0)
                    {
                        output.Add(new Action(new byte[] { i, j }, player));
                    }//if
                }//for
            }//for

            return output.ToArray();
        }

        //------------------------------------------------------------------
        //Import the State
        public void ImportState(IState state)
        {
            this.isOver = state.isOver;
            this.whoWon = state.whoWon;
            board = state.ExportBoard();
            counter = (short)state.PossMoves(1).Length;
        }

        //------------------------------------------------------------------
        //Exports the state
        public byte[,] ExportBoard()
        {
            byte[,] output = new byte[ameba.Y, ameba.X];
            for (int i = 0; i < ameba.Y; i++)
            {
                for (int j = 0; j < ameba.X; j++)
                {
                    output[i, j] = board[i, j];
                }
            }
            return output;
        }
    }
}
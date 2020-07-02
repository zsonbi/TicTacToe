using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TicTacToe
{
    internal class Checker
    {
        //Protected Varriables

        protected cell[,] cells;
        protected byte x;
        protected byte y;

        //Varriables

        private byte checksize;//Mennyinek kell kigyülnie a győzelemhe
        private bool winner;//Ki győzött (true X : false O)
        private byte counter = 0;//Számláló, hogy hány ugyanolyan van egymás után
        private byte currentType = 0;//A jelenlegi sorozatban levő tipus
        private byte[] end = new byte[2];//Hol lett a sorozat vége
        private byte[] start = new byte[2];//Hol kezdődött a sorozat
        private bool wintype;

        //Properties

        public byte[] End { get => end; }
        public byte[] Start { get => start; }
        public bool Winner { get => winner; }
        public bool Wintype { get => wintype; }

        //-------------------------------------------
        //Konstruktor
        public Checker(byte y, byte x, byte Checksize)
        {
            this.x = x;
            this.y = y;
            this.checksize = Checksize;
            cells = new cell[y, x];
            //Feltöltjük a cells-t 2d-s tömböt
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    cells[i, j] = new cell();
                }//for
            }//for
        }

        //--------------------------------------------
        protected bool Check()
        {
            //Ideiglenes változók
            sbyte xcord = 0;
            sbyte ycord = 0;

            //y tengelyen a vizsgálat
            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    if (InsideCheck(i, j))
                    {
                        start[0] = i;
                        start[1] = (byte)(j - checksize);
                        wintype = false;
                        return true;
                    }//if
                }//for
                counter = 0;
                currentType = 0;
            }//for

            //x tengelyen a vizsgálat
            for (byte i = 0; i < x; i++)
            {
                for (byte j = 0; j < y; j++)
                {
                    if (InsideCheck(j, i))
                    {
                        start[0] = (byte)(j - checksize);
                        start[1] = i;
                        wintype = false;
                        return true;
                    }//if
                }//for
                counter = 0;
                currentType = 0;
            }//for

            //x tengely mentén vizsgálat jobbra
            for (sbyte i = 0; i < x; i++)
            {
                xcord = i;
                while (xcord < x && ycord < y)
                {
                    if (InsideCheck((byte)ycord, (byte)xcord))
                    {
                        wintype = true;
                        start[0] = (byte)(ycord - checksize);
                        start[1] = (byte)(xcord - checksize);
                        return true;
                    }//if
                    xcord++;
                    ycord++;
                }//while
                ycord = 0;
                counter = 0;
                currentType = 0;
            }//for

            xcord = 0;
            //y tengely mentén vizsgálat jobbra
            for (sbyte i = 0; i < y; i++)
            {
                ycord = i;
                while (xcord < x && ycord < y)
                {
                    if (InsideCheck((byte)ycord, (byte)xcord))
                    {
                        wintype = true;
                        start[0] = (byte)(ycord - checksize);
                        start[1] = (byte)(xcord - checksize);
                        return true;
                    }//if
                    xcord++;
                    ycord++;
                }//while
                xcord = 0;
                counter = 0;
                currentType = 0;
            }
            ycord = 0;
            //x tengely mentén vizsgálat balra
            for (sbyte i = (sbyte)(x - 1); i >= 0; i--)
            {
                xcord = i;
                while (xcord >= 0 && ycord < y)
                {
                    if (InsideCheck((byte)ycord, (byte)xcord))
                    {
                        wintype = true;
                        start[0] = (byte)(ycord - checksize);
                        start[1] = (byte)(xcord + checksize);
                        return true;
                    }//if
                    xcord--;
                    ycord++;
                }//while
                ycord = 0;
                counter = 0;
                currentType = 0;
            }//for
            xcord = 0;
            //y tengely mentén vizsgálat balra
            for (sbyte i = (sbyte)(x - 1); i >= 0; i--)
            {
                ycord = i;
                while (xcord >= 0 && ycord < y)
                {
                    if (InsideCheck((byte)ycord, (byte)xcord))
                    {
                        wintype = true;
                        start[0] = (byte)(ycord - checksize);
                        start[1] = (byte)(xcord + checksize);
                        return true;
                    }//if
                    xcord--;
                    ycord++;
                }//while
                xcord = 0;
                counter = 0;
                currentType = 0;
            }//for
            xcord = 0;
            ycord = 0;
            counter = 0;
            currentType = 0;
            return false;
        }

        //----------------------------------------------------------------------
        //Az ismétlés csökkentése
        private bool InsideCheck(byte ycord, byte xcord)
        {
            if (cells[ycord, xcord].Type == currentType && currentType != 0)
            {
                counter++;
            }
            else
            {
                currentType = cells[ycord, xcord].Type;
                counter = 0;
            }
            if (counter == checksize)
            {
                end[0] = ycord;
                end[1] = xcord;
                winner = currentType == 1;
                return true;
            }
            return false;
        }
    }
}
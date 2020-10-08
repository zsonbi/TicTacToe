namespace TicTacToe
{
    internal class Checker
    {
        //Protected Varriables

        protected cell[,] cells;

        //Varriables

        private readonly byte x;//A játéktér mérete x tengelyen
        private readonly byte y;//A játéktér mérete y tengelyen
        private readonly byte checksize;//Mennyinek kell kigyülnie a győzelemhe
        private bool winner;//Ki győzött (true X : false O)
        private byte counter = 0;//Számláló, hogy hány ugyanolyan van egymás után
        private byte currentType = 0;//A jelenlegi sorozatban levő tipus
        private byte[] end = new byte[2];//Hol lett a sorozat vége
        private byte[] start = new byte[2];//Hol kezdődött a sorozat
        private bool wintype; //Hogyan győzött (true a győztes cellák egymás mellett false pedig, ha srégen)

        //Properties

        public byte[] End { get => end; }
        public byte[] Start { get => start; }
        public bool Winner { get => winner; }
        public bool Wintype { get => wintype; }
        public byte Checksize { get => checksize; }
        public byte X { get => x; }
        public byte Y { get => y; }

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
            sbyte xcord;
            sbyte ycord = 0;
            counter = 0;
            currentType = 0;
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
            for (sbyte i = 0; i < x - checksize; i++)
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
            for (sbyte i = 0; i < y - checksize; i++)
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
            for (sbyte i = (sbyte)(x - 1); i >= checksize; i--)
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
            xcord = (sbyte)(x - 1);
            //y tengely mentén vizsgálat balra
            for (sbyte i = (sbyte)(y - 1 - checksize); i >= 0; i--)
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
                xcord = (sbyte)(x - 1);
                counter = 0;
                currentType = 0;
            }//for

            return false;
        }

        //----------------------------------------------------------------------
        //Az ismétlés csökkentése
        private bool InsideCheck(byte ycord, byte xcord)
        {
            //Ha ugyanaz a fajta a mostani mint az előző cellában levő és nem üres
            if (cells[ycord, xcord].Type == currentType && currentType != 0)
            {
                counter++;
            }//if
            //Egyébként resetáljuk a számlálót és az előző tipust frissítjük
            else
            {
                currentType = cells[ycord, xcord].Type;
                counter = 0;
            }//else
            //Ha kigyült sorozatba annyi, hogy eldőlt a győztes
            if (counter == checksize)
            {
                end[0] = ycord;
                end[1] = xcord;
                winner = currentType == 1;
                return true;
            }//if
            return false;
        }
    }
}
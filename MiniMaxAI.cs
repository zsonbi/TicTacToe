using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// Minimax algoritmus alapján készült https://en.wikipedia.org/wiki/Minimax, de nem működik nagyobb pályán mint 3x3 mivel túl sok számítást kell végeznie
    /// az algoritmus alapja, hogy megnézzük az összes lehetséges lépést és kiválasztjuk számunkra a legjobbat röviden ennyi
    /// </summary>
    internal class MiniMaxAI : IAI
    {
        //Varriables

        private readonly PlayField current; //Jelenlegi pálya állapot
        private List<PlayField> nextState; //Ez fog változni ez egy ideiglenes változó/pálya
        private byte x;//A játéktér mérete x tengelyen
        private byte y;//A játéktér mérete y tengelyen

        //Properties
        public bool Side { get; private set; }

        //--------------------------------------------------------------------------------
        //Konstruktor
        public MiniMaxAI(PlayField be, bool aiside)
        {
            this.x = be.X;
            this.y = be.Y;
            this.current = be;
            this.Side = aiside;
            //Megcsináljuk a listát
            nextState = new List<PlayField>();
        }

        //-----------------------------------------------------------------------------
        //Megkeresi a következő legjobb lépést
        public async Task<byte[]> next()
        {
            //Hozzáadjuk a 0. szinten levő pályát
            nextState.Add(new PlayField(y, x, current.Checksize));
            //A score ami alapján döntünk majd a nagyobb annál jobb (1 a legnagyobb my disappointment is immesurable)
            sbyte[,] minmax = new sbyte[y, x];
            //Végigmegyünk az összes legális lépésen
            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    //A jelenlegi játékról csinál egy másolatot
                    nextState[0].MakePrevState(current);
                    //Megnézzük, hogy ahova szeretnénk lépni az üres-e
                    if (!nextState[0].IsCellEmpty(i, j))
                    {
                        //Ha nem üres akkor ez a hely egyből kiesett :(    (ennyivel is kevesebbet kell számítani so that's good)
                        minmax[i, j] = sbyte.MinValue;
                        continue;
                    }//if
                    //Megcsináljuk a feltételezett lépést a másolt pályánkon
                    nextState[0].Change(i, j, Side);
                    //Ha egyből győznénk azzal a lépéssel
                    if (nextState[0].over)
                    {
                        return new byte[] { i, j };
                    }//if
                    //Meghívjuk a rekurzív függvényt ami az összes többi lépésen ami még jönni fog végigmegy(rekurzívan) és vissza ad egy sbyte-ot ami -1 = ha az ellenfél győzött
                    //0 ha döntetlen  és 1 ha győzött
                    minmax[i, j] = await Recursion(1, !Side);
                }//for
            }//for
            //A legjobb scoreal rendelkező lépést adjuk vissza
            return FindbiggestIndex(minmax);
        }

        //-----------------------------------------------------------------------------
        //Utálom magam, hogy ezt kell használnom nevéből értetődik rekurzió
        private async Task<sbyte> Recursion(byte deepness, bool whichside)
        {
            //Ha ezen a layeren még nincs egy pálya másolat csinálunk egyet
            if (nextState.Count <= deepness)
            {
                nextState.Add(new PlayField(y, x, current.Checksize));
            }//if
            //A score ami alapján döntünk majd a nagyobb annál jobb (1 a legnagyobb my disappointment is immesurable)
            sbyte[,] minmax = new sbyte[y, x];
            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    //A jelenlegi játékról csinál egy másolatot
                    nextState[deepness].MakePrevState(nextState[deepness - 1]);
                    //Megnézzük, hogy ahova szeretnénk lépni az üres-e
                    if (!nextState[deepness].IsCellEmpty(i, j))
                    {
                        minmax[i, j] = sbyte.MinValue;
                        continue;
                    }//if
                    //Megcsináljuk a feltételezett lépést a másolt pályánkon
                    nextState[deepness].Change(i, j, whichside);
                    //Megnézzük, hogy a lépés befejezte-e a játékot
                    if (nextState[deepness].over)
                    {
                        //Megnézzük melyik oldal győzött
                        if (nextState[deepness].Winner == Side)
                        {
                            //Ha annak az oldalnak ez a feltételezett legjobb lépése visszaadjuk az értéket
                            if (whichside == Side)
                                return 1;
                            minmax[i, j] = 1;
                        }//if
                        else
                        {
                            //Ha annak az oldalnak ez a feltételezett legjobb lépése visszaadjuk az értéket
                            if (whichside == !Side)
                                return -1;
                            minmax[i, j] = -1;
                        }//else
                    }//if
                    //Ha nem ért véget a játék meghívjuk a függvényt magát megint :(
                    else
                    {
                        //Ha nincs több hely ahova lehetne lépni akkor a játék döntetlen
                        if (nextState[deepness].Counter > 0)
                            minmax[i, j] = await Recursion((byte)(deepness + 1), !whichside);
                        else
                            return 0;
                    }//else
                }
            }
            //Visszaadjuk az aktuális oldalnak a legjobb lépésének értékét
            return whichside == Side ? Findbiggest(minmax) : FindSmallest(minmax);
        }

        //--------------------------------------------------------------------------------
        //Megkeresi a legnagyobb érték helyét a tömbben
        private byte[] FindbiggestIndex(sbyte[,] be)
        {
            sbyte biggest = sbyte.MinValue;
            byte indexy = 0;
            byte indexx = 0;

            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    if (biggest < be[i, j])
                    {
                        indexy = i;
                        indexx = j;
                        biggest = be[i, j];
                    }//if
                }//for
            }//for
            return new byte[2] { indexy, indexx };
        }

        //--------------------------------------------------------------------------------
        //Megkeresi a legnagyobb értéket a tömbben
        private sbyte Findbiggest(sbyte[,] be)
        {
            sbyte biggest = sbyte.MinValue;

            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    if (biggest < be[i, j])
                    {
                        biggest = be[i, j];
                    }//if
                }//for
            }//for
            return biggest;
        }

        //--------------------------------------------------------------------------------
        //Megkeresi a legnagyobb értéket a tömbben
        private sbyte FindSmallest(sbyte[,] be)
        {
            sbyte smallest = sbyte.MaxValue;

            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x; j++)
                {
                    if (be[i, j] == sbyte.MinValue)
                        continue;
                    if (smallest > be[i, j])
                    {
                        smallest = be[i, j];
                    }//if
                }//for
            }//for
            return smallest;
        }
    }
}
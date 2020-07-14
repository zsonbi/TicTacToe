using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class PatternAI : IAI
    {
        //Varriables

        private readonly PlayField current; //Jelenlegi pálya állapot
        private List<PlayField> nextState; //Ez fog változni ez egy ideiglenes változó/pálya
        private byte x;//A játéktér mérete x tengelyen
        private byte y;//A játéktér mérete y tengelyen
        private byte checkSize;
        private List<sbyte[]> Patterns = new List<sbyte[]>();
        private float[][] Policies;
        private Dictionary<string, float[]> stateToScore;

        //Properties
        public bool Side { get; private set; }

        //--------------------------------------------------------------------------------
        //Konstruktor
        public PatternAI(PlayField be, bool aiside)
        {
            this.x = be.X;
            this.y = be.Y;
            this.current = be;
            this.Side = aiside;
            //Megcsináljuk a listát
            nextState = new List<PlayField>();
            this.checkSize = current.Checksize;
            Setup();
        }

        //-----------------------------------------------------------------------------
        //A patternek és policyk megcsinálása és a Pattern átalakítása string-é, hogy azt használjuk majd a Dictionaryben
        private void Setup()
        {
            MakePatterns();
            MakePolicies();
            stateToScore = new Dictionary<string, float[]>();
            for (int i = 0; i < Patterns.Count; i++)
            {
                string temp = "";
                foreach (var item in Patterns[i])
                {
                    temp += item;
                }

                stateToScore.Add(temp, Policies[i]);
            }
        }

        //----------------------------------------------------------------------------
        //A következő lépés
        public async Task<byte[]> next()
        {
            return FindbiggestIndex(await Createimportance());
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //Csinálunk egy 2d-s tömböt ami minták ráhelyezése után megállapít egy pontot az adott üres helyeknek és az alapján döntünk majd
        private async Task<float[,]> Createimportance()
        {
            float[,] importance = new float[y, x];

            //Ideiglenes változók
            sbyte xcord;
            sbyte ycord = 0;
            string temp;
            //y tengelyen a vizsgálat
            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j < x - checkSize; j++)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k <= checkSize; k++)
                    {
                        temp += current.GetCellType(new byte[] { i, (byte)(j + k) });
                    }//for
                    //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        continue;
                    }//if
                    //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k <= checkSize; k++)
                    {
                        importance[i, j + k] += stateToScore[temp][k];
                    }//for
                }//for
            }//for

            //x tengelyen a vizsgálat
            for (byte i = 0; i < x; i++)
            {
                for (byte j = 0; j < y - checkSize; j++)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k <= checkSize; k++)
                    {
                        temp += current.GetCellType(new byte[] { (byte)(j + k), i });
                    }//for
                    //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        continue;
                    }//if
                    //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k <= checkSize; k++)
                    {
                        importance[j + k, i] += stateToScore[temp][k];
                    }//for
                }//for
            }//for

            //x tengely mentén vizsgálat jobbra
            for (sbyte i = 0; i < x - checkSize; i++)
            {
                xcord = i;
                while (xcord < x - checkSize && ycord < y - checkSize)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k <= checkSize; k++)
                    {
                        temp += current.GetCellType(new byte[] { (byte)(ycord + k), (byte)(xcord + k) });
                    }//for
                    //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        xcord++;
                        ycord++;
                        continue;
                    }//if
                    //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k <= checkSize; k++)
                    {
                        importance[ycord + k, xcord + k] += stateToScore[temp][k];
                    }//for
                    xcord++;
                    ycord++;
                }//while
                ycord = 0;
            }//for

            xcord = 0;
            //y tengely mentén vizsgálat jobbra
            for (sbyte i = 0; i < y - checkSize; i++)
            {
                ycord = i;
                while (xcord < x - checkSize && ycord < y - checkSize)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k <= checkSize; k++)
                    {
                        temp += current.GetCellType(new byte[] { (byte)(ycord + k), (byte)(xcord + k) });
                    }//for
                    //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        xcord++;
                        ycord++;
                        continue;
                    }//if
                    //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k <= checkSize; k++)
                    {
                        importance[ycord + k, xcord + k] += stateToScore[temp][k];
                    }//for
                    xcord++;
                    ycord++;
                }//while
                xcord = 0;
            }
            ycord = 0;

            //x tengely mentén vizsgálat balra
            for (sbyte i = (sbyte)(x - 1); i >= checkSize; i--)
            {
                xcord = i;
                while (xcord >= checkSize && ycord < y - checkSize)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k <= checkSize; k++)
                    {
                        temp += current.GetCellType(new byte[] { (byte)(ycord + k), (byte)(xcord - k) });
                    }//for
                    //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        xcord--;
                        ycord++;
                        continue;
                    }//if
                    //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k <= checkSize; k++)
                    {
                        importance[ycord + k, xcord - k] += stateToScore[temp][k];
                    }//for
                    xcord--;
                    ycord++;
                }//while
                ycord = 0;
            }//for
            xcord = (sbyte)(x - 1);
            //y tengely mentén vizsgálat balra
            for (sbyte i = (sbyte)(y - 1 - checkSize); i >= 0; i--)
            {
                ycord = i;
                while (xcord >= checkSize && ycord < y - checkSize)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k <= checkSize; k++)
                    {
                        temp += current.GetCellType(new byte[] { (byte)(ycord + k), (byte)(xcord - k) });
                    }//for
                    //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        xcord--;
                        ycord++;
                        continue;
                    }//if
                    //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k <= checkSize; k++)
                    {
                        importance[ycord + k, xcord - k] += stateToScore[temp][k];
                    }//for
                    xcord--;
                    ycord++;
                }//while
                xcord = (sbyte)(x - 1);
            }//for

            return importance;
        }

        //--------------------------------------------------------------------------------
        //Megkeresi a legnagyobb érték helyét a tömbben
        private byte[] FindbiggestIndex(float[,] be)
        {
            float biggest = float.MinValue;
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

        //--------------------------------------------------------------------
        //Megcsinálja a policyket
        private void MakePolicies()
        {
            //A mérete
            int NumberOfPolicies = Patterns.Count;
            Policies = new float[NumberOfPolicies][];
            float[] basic = Makebasic((byte)(checkSize + 1));

            for (int i = 0; i < NumberOfPolicies; i++)
            {
                float[] tempPolicy = new float[checkSize + 1];

                if (Patterns[i].Count(x => x == 1) == checkSize)
                {
                    tempPolicy[Patterns[i].ToList().FindIndex(x => x == 0)] += Side ? 1000f : 750f;
                }//if
                else if (Patterns[i].Count(x => x == 2) == checkSize)
                {
                    tempPolicy[Patterns[i].ToList().FindIndex(x => x == 0)] += Side ? 750f : 1000f;
                }//else if
                else if (Patterns[i].Count(x => x == 0) == checkSize + 1)
                {
                    tempPolicy = basic;
                }
                else
                {
                    for (int j = 0; j <= checkSize; j++)
                    {
                        float[] tempArray;
                        if (Patterns[i][j] == 1)
                        {
                            tempArray = CalculateScore(this.Side ? true : false, new int[] { i, j });
                            for (int k = 0; k <= checkSize; k++)
                            {
                                tempPolicy[k] += tempArray[k];
                            }//for
                        }//if
                        else if (Patterns[i][j] == 2)
                        {
                            tempArray = CalculateScore(this.Side ? false : true, new int[] { i, j });
                            for (int k = 0; k <= checkSize; k++)
                            {
                                tempPolicy[k] += tempArray[k];
                            }//for
                        }//else
                    }//for
                }//else
                //Ha betudja rakni insta win szituációba
                if (Patterns[i].Distinct().ToList().Count() == 2 && Patterns[i].Count(x => x == 1 || x == 2) == checkSize - 1)
                {
                    for (int k = 0; k <= checkSize; k++)
                    {
                        //Ez lehet overkill majd még tesztelgetem
                        tempPolicy[k] *= 20;
                    }
                }
                Policies[i] = tempPolicy;
            }
        }

        //--------------------------------------------------------------------------
        //Megcsinálja az alap mintát pl így kéne inéznie egy 5-ös méretűnél {5,10,20,10,5}
        //Erre a methodra nagyon nem vagyok büszke olyan szinten csúnya lett pls kill me
        //Ez ahoz kell, ha egy csak üres mintához érünk ez lesz az értéke
        private float[] Makebasic(byte size)
        {
            List<float> output = new List<float>();

            //Egy ideiglenes listát csinálunk ami 2 felé fogja osztani a kimenetet
            float[][] temp = new float[2][];
            for (byte i = 0; i < 2; i++)
            {
                float[] temparray = new float[size / 2 + (size % 2 == 0 ? 0 : i)];
                temp[i] = temparray;
            }//for

            //Ha páros akkor 20-as értéket kap egyébként pedig 10-est
            float basic = size % 2 == 0 ? 2f : 1f;
            //Végigmegyünk az első felén
            for (sbyte i = (sbyte)(temp[0].Length - 1); i >= 0; i--)
            {
                temp[0][i] = basic;
                basic /= 2;
            }//for
            //Resetelük az értékét
            basic = 2f;
            //Végigmegyünk a 2. felén
            for (byte i = 0; i < temp[1].Length; i++)
            {
                temp[1][i] = basic;
                basic /= 2;
            }//for
            //Hozzáadjuk a 2 ideiglenes listát a kimeneti listához
            output.AddRange(temp[0]);
            output.AddRange(temp[1]);

            return output.ToArray();
        }

        //----------------------------------------------------------------
        //Megcsinálja az összes lehetséges kombinációt
        private void MakePatterns()
        {
            short NumberofElements = (short)(Math.Pow(3, checkSize + 1));

            Recursion(1);
            Patterns.Where(x => !x.ToList().Contains(0)).ToList().ForEach(x => Patterns.Remove(x));
        }

        //-----------------------------------------------------------------------
        //Rekurzióval kezeljük le a kombinációkat mivel nem constans a Patternek mérete emiatt nem elegek csak a ciklusok
        private void Recursion(byte depth, params sbyte[] input)
        {
            //A lehetséges kombinációk száma
            for (sbyte i = 0; i < 3; i++)
            {
                //Ha nem értük még el azt a méretet amekkorára akarjuk a Patterneket akkor meghívjuk megint a Methodot és hozzáadjuk az inputhoz a jelenlegi ciklusváltozót is
                if (depth < checkSize + 1)
                    Recursion((byte)(depth + 1), AddOneNewElement(input, i));
                //Egyébként pedig azt jelenti, hogy megvan az egyik kombináció és azt be is rakhatjuk a Patternsbe
                else
                    Patterns.Add(AddOneNewElement(input, i).ToArray());
            }
        }

        //----------------------------------------------------------------------------------------------------
        //A bevitt tömbhöz hozzáad még 1 elemet amit szintén megadtunk paraméterben
        private sbyte[] AddOneNewElement(sbyte[] input, sbyte newElement)
        {
            sbyte[] output = new sbyte[input.Length + 1];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[i];
            }
            output[input.Length] = newElement;
            return output;
        }

        //---------------------------------------------------------------------------------------
        //Csak kiírjuk a paraméterként adott Listának a tartalmát a konzolra
        private static void WriteOutArray(float[][] be)
        {
            foreach (var item in be)
            {
                foreach (var insideitem in item)
                {
                    Console.Write("(" + insideitem + ")\t");
                }//forea
                Console.WriteLine();
            }//forea
        }

        //-----------------------------------------------------------------------------------
        //A kilyelölt indextől képest csinálunk egy felére csökkenő sorozatot balra és jobbra is (a tömb mérete a checksize+1)
        //Az Index 1. eleme az a Pattern indexe a 2. pedig a kiválasztott elemé
        //A kimenet ilyesmi lesz (Index{0,1}) {5,10,5,2.5,1.25}
        private float[] CalculateScore(bool side, int[] Index)
        {
            float[] ki = new float[checkSize + 1];
            float basevalue = 10f;
            for (int i = Index[1]; i >= 0; i--)
            {
                if (Patterns[Index[0]][i] == 0)
                    ki[i] = side == this.Side ? basevalue : basevalue / 2;

                basevalue /= 2;
            }
            basevalue = 10f;
            for (int i = Index[1]; i < checkSize + 1; i++)
            {
                if (Patterns[Index[0]][i] == 0)
                    ki[i] = side == this.Side ? basevalue : basevalue / 2;

                basevalue /= 2;
            }
            return ki;
        }
    }
}
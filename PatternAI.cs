using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class PatternAI : IAI
    {
        //Varriables

        private readonly IState current; //Jelenlegi pálya állapot
        private readonly byte x;//A játéktér mérete x tengelyen
        private readonly byte y;//A játéktér mérete y tengelyen
        private readonly byte checkSize; //Mennyinek kell kigyülnie a győzelemhez
        private List<sbyte[]> Patterns = new List<sbyte[]>(); //A minták
        private Dictionary<string, float[]> stateToScore; //Az adott mintához mennyi pont jár
        private bool Side;

        //Properties
        public byte aiSide { get => (byte)(this.Side ? 1 : 2); } //Az AI melyik oldalt játszik

        //--------------------------------------------------------------------------------
        //Konstruktor
        public PatternAI(byte aiside, IState be)
        {
            this.x = ameba.X;
            this.y = ameba.Y;
            this.current = be;
            this.Side = aiside == 1;
            this.checkSize = (byte)(ameba.Checksize);
            Setup();
        }

        //-----------------------------------------------------------------------------
        //A patternek és policyk megcsinálása és a Pattern átalakítása string-é, hogy azt használjuk majd a Dictionaryben
        private void Setup()
        {
            MakePatterns();

            stateToScore = new Dictionary<string, float[]>();
            float[][] Policies = MakePolicies();
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
        public async Task<IAction> Next()
        {
            return new Action(FindbiggestIndex(await Createimportance()), aiSide);
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //Csinálunk egy 2d-s tömböt ami minták ráhelyezése után megállapít egy pontot az adott üres helyeknek és az alapján döntünk majd
        private async Task<float[,]> Createimportance()
        {
            float[,] importance = new float[y, x];
            byte[,] cellTypes = current.ExportBoard();

            //Ideiglenes változók
            sbyte xcord;
            sbyte ycord = 0;
            string temp;
            //y tengelyen a vizsgálat
            for (byte i = 0; i < y; i++)
            {
                for (byte j = 0; j <= x - checkSize; j++)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k < checkSize; k++)
                    {
                        temp += cellTypes[i, j + k];
                    }//for
                     //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        continue;
                    }//if
                     //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k < checkSize; k++)
                    {
                        importance[i, j + k] += stateToScore[temp][k];
                    }//for
                }//for
            }//for

            //x tengelyen a vizsgálat
            for (byte i = 0; i < x; i++)
            {
                for (byte j = 0; j <= y - checkSize; j++)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k < checkSize; k++)
                    {
                        temp += cellTypes[j + k, i];
                    }//for
                     //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        continue;
                    }//if
                     //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k < checkSize; k++)
                    {
                        importance[j + k, i] += stateToScore[temp][k];
                    }//for
                }//for
            }//for

            //x tengely mentén vizsgálat jobbra
            for (sbyte i = 0; i <= x - checkSize; i++)
            {
                xcord = i;
                while (xcord <= x - checkSize && ycord <= y - checkSize)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k < checkSize; k++)
                    {
                        temp += cellTypes[ycord + k, xcord + k];
                    }//for
                     //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        xcord++;
                        ycord++;
                        continue;
                    }//if
                     //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k < checkSize; k++)
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
            for (sbyte i = 0; i <= y - checkSize; i++)
            {
                ycord = i;
                while (xcord <= x - checkSize && ycord <= y - checkSize)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k < checkSize; k++)
                    {
                        temp += cellTypes[ycord + k, xcord + k];
                    }//for
                     //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        xcord++;
                        ycord++;
                        continue;
                    }//if
                     //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k < checkSize; k++)
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
                while (xcord >= checkSize && ycord <= y - checkSize)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k < checkSize; k++)
                    {
                        temp += cellTypes[ycord + k, xcord - k];
                    }//for
                     //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        xcord--;
                        ycord++;
                        continue;
                    }//if
                     //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k < checkSize; k++)
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
                while (xcord >= checkSize && ycord <= y - checkSize)
                {
                    temp = "";
                    //Megcsináljuk a kulcsot amivel majd hozzátudunk férni a Policykhez
                    for (int k = 0; k < checkSize; k++)
                    {
                        temp += cellTypes[ycord + k, xcord - k];
                    }//for
                     //Ha nem tartalmaz olyan kulcsot (Ez amiatt történhet meg mert kiszedtem azokat a mintákat amik csak 1-est és 2-est tartalmaztak)
                    if (!stateToScore.ContainsKey(temp))
                    {
                        xcord--;
                        ycord++;
                        continue;
                    }//if
                     //Az importancehez hozzáadjuk a mintához tartozó Policy értékét
                    for (int k = 0; k < checkSize; k++)
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

        //--------------------------------------------------------------------
        //Megcsinálja a policyket
        private float[][] MakePolicies()
        {
            float[][] Policies;
            //A mérete
            int NumberOfPolicies = Patterns.Count;
            Policies = new float[NumberOfPolicies][];
            float[] basic = CreateFunction(false, new int[] { 0, (checkSize - 1) / 2 });

            for (int i = 0; i < NumberOfPolicies; i++)
            {
                float[] tempPolicy = new float[checkSize];

                if (Patterns[i].Count(x => x == 1) == checkSize - 1)
                {
                    tempPolicy[Patterns[i].ToList().FindIndex(x => x == 0)] += Side ? 1000f : 750f;
                }//if
                else if (Patterns[i].Count(x => x == 2) == checkSize - 1)
                {
                    tempPolicy[Patterns[i].ToList().FindIndex(x => x == 0)] += Side ? 750f : 1000f;
                }//else if
                else if (Patterns[i].Count(x => x == 0) == checkSize)
                {
                    tempPolicy = basic;
                }
                else
                {
                    for (int j = 0; j < checkSize; j++)
                    {
                        float[] tempArray;
                        if (Patterns[i][j] == 1)
                        {
                            tempArray = CreateFunction(this.Side ? true : false, new int[] { i, j });
                            // tempArray = CalculateScore(this.Side ? true : false, new int[] { i, j });
                            for (int k = 0; k < checkSize; k++)
                            {
                                tempPolicy[k] += tempArray[k];
                            }//for
                        }//if
                        else if (Patterns[i][j] == 2)
                        {
                            tempArray = CreateFunction(this.Side ? false : true, new int[] { i, j });
                            //  tempArray = CalculateScore(this.Side ? false : true, new int[] { i, j });
                            for (int k = 0; k < checkSize; k++)
                            {
                                tempPolicy[k] += tempArray[k];
                            }//for
                        }//else
                    }//for
                }//else

                //Szimplán, ha egy jó lépés megduplázzuk a pontját
                if (Patterns[i].Distinct().ToList().Count() == 2 && Patterns[i].Count(x => x == 1 || x == 2) == checkSize - 3)
                {
                    for (int k = 0; k < checkSize; k++)
                    {
                        //Ez lehet overkill majd még tesztelgetem
                        tempPolicy[k] *= 2;
                    }
                }
                //Ha betudja rakni insta win szituációba
                else if (Patterns[i].Distinct().ToList().Count() == 2 && Patterns[i].Count(x => x == 1 || x == 2) == checkSize - 1)
                {
                    for (int k = 0; k < checkSize; k++)
                    {
                        //Ez lehet overkill majd még tesztelgetem
                        tempPolicy[k] *= 20;
                    }
                }
                //Ha ezze a lépéssel a köv. körben nyerhet
                else if (Patterns[i].Distinct().ToList().Count() == 2 && Patterns[i].Count(x => x == 1 || x == 2) == checkSize - 2)
                {
                    for (int k = 0; k < checkSize; k++)
                    {
                        //Ez lehet overkill majd még tesztelgetem
                        tempPolicy[k] *= 10;
                    }
                }

                Policies[i] = tempPolicy;
            }
            return Policies;
        }

        //--------------------------------------------------------------------------
        //Ez fogja megcsinálni a függvényt f(x) = 10/2^|i-index[1]|
        //A kimenet ilyesmi lesz (Index{0,1}) {5,10,5,2.5,1.25}
        private float[] CreateFunction(bool side, int[] index)
        {
            float[] output = new float[checkSize];

            for (int i = 0; i < checkSize; i++)
            {
                if (Patterns[index[0]][i] == 0)
                {
                    //Ha az ellenfélé az elem akkor csak fele annyi pontot kap a melette levő helyekre
                    output[i] = 10 / (float)Math.Pow(2, Math.Abs(i - index[1]));
                }
            }
            return output;
        }

        //----------------------------------------------------------------
        //Megcsinálja az összes lehetséges kombinációt
        private void MakePatterns()
        {
            short NumberofElements = (short)(Math.Pow(3, checkSize));

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
                if (depth < checkSize)
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
    }
}
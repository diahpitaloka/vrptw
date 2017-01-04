using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project_vrptw
{
    class inisiasi
    {
        private string[,] parents;
        private int valPopSize;
        private int totKromosom;
        int x;
        string[] strKromosom;
        string tempKromosom;
        string px, py;
        int valRandom1, valRandom2;
        Random r;

    

        //Konstruktor untuk memgambil nilai popSize dan panjang kromosom
        public inisiasi(int valpopSize, int totKromosom)
        {
            this.valPopSize = valpopSize;
            this.totKromosom = totKromosom;
        }
        
       
        //Membangkitkan populasi awal
        public string[,] inisiasiPopulasi
        {
            get
            {
             
                parents = new string[valPopSize, totKromosom];
                r = new Random();

                for (int i = 0; i < valPopSize; i++)
                {
                    for (int j = 0; j < totKromosom; j++)
                    {
                        parents[i, j] = j.ToString();
                    }
                }

                for (int i = 0; i < valPopSize; i++)
                {
                    //for (int j = 0; j < totKromosom; j++)
                    //{
                        x=0;
                        while (x < 2)
                        {
                            valRandom1 = r.Next(1, 17);
                            valRandom2 = r.Next(1, 17);

                            string temp = "";
                            if (valRandom1 != valRandom2)
                            {
                                temp = parents[i, valRandom2];
                                parents[i, valRandom2] = parents[i, valRandom1];
                                parents[i, valRandom1] = temp;
                            }
                            x++;
                        //}

                    }
                }
                //Proses nilai random


                    return parents;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project_vrptw
{
    class reproduksi
    {
        string [] strParent1, strParent2, strChild;
        int valRandom;
        Random r;
        int lengtKromosom, tempVal, x, x1,x2, valRandom1, valRandom2;
        string tempString;

        public reproduksi(string[] strParent1, string[] strParent2, int valRandom1)
        {
            this.strParent1 =  strParent1;
            this.strParent2 = strParent2;
            this.valRandom1 = valRandom1;
        }

        public reproduksi(string [] strParent1, int valRandom1, int valRandom2)
        {
            this.strParent1 = strParent1;
            this.valRandom1 = valRandom1;
            this.valRandom2 = valRandom2;
        }



        public void reproduksiMutasi()
        {
            lengtKromosom = strParent1.Length;
            //r = new Random();
            //x1 = r.Next(1, lengtKromosom/2);
            //x2 = r.Next(lengtKromosom/2, lengtKromosom);

            strChild = new string[lengtKromosom];

            strChild = strParent1;
            tempString = child[valRandom1];
            child[valRandom1] = child[valRandom2];
            child[valRandom2] = tempString;

        }


        public void reproduksiCrossover()
        {
            lengtKromosom = strParent1.Length;
            //r = new Random();
            //valRandom = r.Next(3, lengtKromosom-2);

            strChild = new string[lengtKromosom];

            x=0;
            while (x < valRandom1)
            {
                strChild[x] = strParent1[x];
                x++;
            }

            for (int i = 0; i < lengtKromosom; i++)
            { 
                tempVal = 0;
                for (int j = 0; j < lengtKromosom; j++)
                {
                    if (strParent2[i] == strChild[j])
                    {
                        j = lengtKromosom;
                        tempVal = 1;
                    }
                }

                if (tempVal == 0)
                {
                    strChild[valRandom1] = strParent2[i];
                    valRandom1 += 1;
                }
            }
        }

        public string [] child
        {
            get { return strChild; }
            set { strChild = value; }
        }
    }
}

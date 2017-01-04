using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project_vrptw
{
    class seleksi
    {
        string[] individu, jadwalBuka, jadwalTutup;
        //int fitness;
        int totLength;
        int[,] matrixWaktu;
        TimeSpan waktuDatang, waktuBerangkat, tsJadwalBuka, tsJadwalTutup;
        //DateTime tempDateTime;
        int totalPenalti;
        //string tempString;
        TimeSpan ts;
        string[] temp1, temp2;
        int[] tempPenalti, tempWaktuTempuh;
        
        int awal, tujuan, waktuPerjalanan;


           
        public seleksi(string[] individu, int[,] matrixWaktu, string[] jadwalBuka, string[] jadwalTutup)
        {
            this.individu = individu;
            this.matrixWaktu = matrixWaktu;
            this.jadwalBuka = jadwalBuka;
            this.jadwalTutup = jadwalTutup;
        }

        //public seleksi(string[] jadwalBuka, string[] jadwalTutup, int totLength)
        //{
        //    this.totLength = totLength;
        //    this.jadwalBuka = jadwalBuka;
        //    this.jadwalTutup = jadwalTutup;
        //}

        //public void ambilJadwal()
        //{
        //    //totLength = individu.Length;
        //    temp1 = new string[totLength];
        //    temp2 = new string[totLength];

        //    for (int i=1;i<totLength;i++)
        //    {
        //        tsJadwalBuka = Convert.ToDateTime((jadwalBuka[i]).ToString()).TimeOfDay;
        //        tsJadwalTutup = Convert.ToDateTime((jadwalTutup[i]).ToString()).TimeOfDay;

        //        temp1[i] = tsJadwalBuka.ToString();
        //        temp2[i] = tsJadwalTutup.ToString();

        //    }
        //}

        public void cariNilaiFitness()
        {

            waktuBerangkat = Convert.ToDateTime("02:00").TimeOfDay;
            totalPenalti = 0;
            

            totLength = individu.Length;
            temp1 = new string[totLength];
            temp2 = new string[totLength];
            tempPenalti = new int[totLength];
            tempWaktuTempuh = new int[totLength];

            for (int i = 1; i < totLength; i++)
            {
                awal = Convert.ToInt32(individu[i - 1]);
                tujuan = Convert.ToInt32(individu[i]);

                waktuPerjalanan = matrixWaktu[awal, tujuan];
                tempWaktuTempuh[i] = waktuPerjalanan;
                //waktuDatang = waktuBerangkat.AddMinutes(waktuPerjalanan);
                //waktuDatang = (Convert.ToDateTime(waktuBerangkat).AddMinutes(waktuPerjalanan)).TimeOfDay;
                waktuDatang = waktuBerangkat.Add(new TimeSpan(0, waktuPerjalanan, 0));
                temp1[i] = waktuDatang.ToString();

                tsJadwalBuka = Convert.ToDateTime((jadwalBuka[tujuan]).ToString()).TimeOfDay;
                tsJadwalTutup = Convert.ToDateTime((jadwalTutup[tujuan]).ToString()).TimeOfDay;

                if (waktuDatang < tsJadwalTutup)
                {
                    if (waktuDatang > tsJadwalBuka)
                    {
                        //waktuBerangkat = (Convert.ToDateTime(waktuDatang).AddMinutes(5)).TimeOfDay;
                        waktuBerangkat = waktuDatang.Add(new TimeSpan(0, 5, 0));
                        totalPenalti += 0;
                        temp2[i] = waktuBerangkat.ToString();
                        tempPenalti[i] = 0;
                    }
                    else
                    {
                       //waktuBerangkat = (Convert.ToDateTime(tsJadwalBuka).AddMinutes(5)).TimeOfDay;
                        waktuBerangkat = tsJadwalBuka.Add(new TimeSpan(0,5,0));
                        totalPenalti += 0;
                        temp2[i] = waktuBerangkat.ToString();
                        tempPenalti[i] = 0;
                    }
                }

                else
                {
                    //waktuBerangkat = (Convert.ToDateTime(waktuDatang).AddMinutes(5)).TimeOfDay;
                    waktuBerangkat = waktuDatang.Add(new TimeSpan(0,5,0));
                    //penalti = waktuBerangkat + Convert.ToDateTime(jadwalTutup[tujuan]);
                    temp2[i] = waktuBerangkat.ToString();

                    ts = new TimeSpan();
                    ts = waktuBerangkat.Subtract(tsJadwalTutup);
                    tempPenalti[i] = Convert.ToInt32( ts.TotalMinutes);

                    totalPenalti += Convert.ToInt32(ts.TotalMinutes);
                }
            }
      
        }

        public int fitness
        {
            get { return totalPenalti; }
            set { totalPenalti = value; }
        }

        public string[] tempWaktuDatang
        {
            get { return temp1; }
            set { temp1 = value; }
        }

        public string[] tempWaktuBerangkat
        {
            get { return temp2; }
            set { temp2 = value; }
        }

        public int[] penalti
        {
            get { return tempPenalti; }
            set { tempPenalti = value; }
        }

        public int[] waktuTempuh
        {
            get { return tempWaktuTempuh; }
            set { tempWaktuTempuh = value; }
        }
      
    }
}

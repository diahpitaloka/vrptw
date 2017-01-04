using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace project_vrptw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] jadwalBuka;
        string[] jadwalTutup;
        string[] agen;
        int[,] matrixWaktu;
        int x;
        int valPopSize, totData;

        string [] tempParents1, tempParents2, tempChild, tempWaktuDatang, tempWaktuBerangkat;
        string[,] parents, childs, newParents;
        int[] penalti, tempWaktuTempuh;
        
        //anak
        double valMr, valCr, totFitness, prob, totProbCum, tempDouble, tempRata, tempMax;
        int totChildCrossOver, totChildMutation, totGenerasi;
        int noParent1, noParent2, totChild, generasi, valRandom1,valRandom2;
        Random r;
        double[] fitnessParents, fitnessChild, probCum, fitness, generasiRataRata, generasiFitnessTerbaik, fitnessTerpilih, penaltiTerpilih, pinalti, generasiPinalti;
        string[] generasiBestIndividu, strIndividu, individuTerpilih, row;
        string tempIndividu;
        //TimeSpan t1,t2;
        //DateTime dt1,dt2;
       

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtPath.Text = openFileDialog1.FileName;
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            try
            {


                string pathConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + txtPath.Text + ";Extended Properties=\"Excel 8.0;HDR=Yes;\";";
                OleDbConnection conn = new OleDbConnection(pathConn);

                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter("select * from [" + txtSheetJadwal.Text + "$]", conn);
                DataTable dt = new DataTable();
                myDataAdapter.Fill(dt);
                dgvJadwal.DataSource = dt;

                myDataAdapter = new OleDbDataAdapter("select * from [" + txtSheetWaktu.Text + "$]", conn);
                dt = new DataTable();
                myDataAdapter.Fill(dt);
                dgvWaktu.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                totData = dgvJadwal.Rows.Count - 1;
                agen = new string[totData];
                jadwalBuka = new string[totData];
                jadwalTutup = new string[totData];
   
                int x = 0;
                while (x < totData)
                {
                    agen[x] = dgvJadwal["Agen", x].Value.ToString();
                    jadwalBuka[x] = dgvJadwal["Jadwal Buka", x].Value.ToString();
                    jadwalTutup[x] = dgvJadwal["Jadwal Tutup", x].Value.ToString();
                    x++;
                }


                matrixWaktu = new int[totData, totData];
                for (int i = 0; i < totData; i++)
                {
                    for (int j = 0; j < totData; j++)
                    {
                        matrixWaktu[i,j] = Convert.ToInt32(dgvWaktu[j, i].Value);
                    }
                    Console.WriteLine();
                }

                tabControl1.SelectedIndex = 1;
               

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
               
                //Inisialisasi panjang array dan nilai awal
                valPopSize = Convert.ToInt32(txtPopSize.Text);
                valCr = Convert.ToDouble(txtCr.Text);
                valMr = Convert.ToDouble(txtMr.Text);

                totChildCrossOver = Convert.ToInt32(valCr * valPopSize);
                totChildMutation = Convert.ToInt32(valMr * valPopSize);
                totChild = totChildCrossOver + totChildMutation;

                generasi = Convert.ToInt32(txtGenerasi.Text.ToString());
                generasiFitnessTerbaik = new double[generasi];
                generasiBestIndividu = new string[generasi];
                generasiRataRata = new double[generasi];
                generasiPinalti = new double[generasi];

                //inisialisai parents awal
                inisiasi ind = new inisiasi(valPopSize, totData);
                parents = ind.inisiasiPopulasi;

                rtbSolusi.Text = "";
                dgvResult.Rows.Clear();

                int z =0;
                r = new Random();

                //lakukan perulangan sebanyak n-generasi
                while (z < generasi)
                {
                    totFitness = 0;
                    fitnessParents = new double[valPopSize];
                    fitness = new double[valPopSize + totChild];
                    pinalti = new double[valPopSize + totChild];

                    strIndividu = new string[valPopSize + totChild];
                    newParents = new string[valPopSize + totChild, totData];

                    if (z == 0)
                    {
                        rtbSolusi.Text += "Initial Population\n";
                    }
                    if (z < 3)
                    {
                        rtbSolusi.Text += "Generasi " + (z).ToString() + "\n";
                    }

                    //Inisialisasi fitness parents
                    for (int i = 0; i < valPopSize; i++)
                    {
                        if (z < 3)
                        {
                            rtbSolusi.Text += (i + 1).ToString() + "\t";
                        }
                        tempParents1 = new string[totData];

                        tempIndividu = "";


                        //Simpan kromosom kedalam array 1 dimensi
                        for (int j = 0; j < totData; j++)
                        {
                            if (z < 3)
                            {
                                rtbSolusi.Text += parents[i, j].ToString() + " ";
                            }

                            //simpan nilai parents dari hasil parents terbaik generasi sebelumnya
                            tempIndividu += parents[i, j].ToString() + " ";
                            tempParents1[j] = parents[i, j].ToString();
                            newParents[i, j] = parents[i, j];
                        }
                        strIndividu[i] = tempIndividu;
                        
                        //Perhitungan fitness parents dari kelas seleksi
                        seleksi s = new seleksi(tempParents1, matrixWaktu, jadwalBuka, jadwalTutup);
                        s.cariNilaiFitness();
                        pinalti[i] = s.fitness;

                        if (z < 3)
                        {
                            rtbSolusi.Text += "\tpenalti = " + s.fitness.ToString();
                        }

                        //Menentukan fitness parents
                        fitnessParents[i] = Math.Round(1/((Convert.ToDouble(s.fitness+1) / 60)), 4);
                        fitness[i] = fitnessParents[i];
                        if (z < 3)
                        {
                            rtbSolusi.Text += "\tFitness = " + fitnessParents[i].ToString();
                            rtbSolusi.Text += "\n";
                           
                        }
                        totFitness += fitnessParents[i];
                    }
                    


                    childs = new string[totChild, totData];
                    reproduksi rep;
                    
                    x = 0;

                    if (z < 3)
                    {
                        rtbSolusi.Text += "\n\n=========================================================================================\n\n";
                        rtbSolusi.Text += "Create Children From Crossover";
                    }

                   //Melakukan proses crossover
                    while (x < totChildCrossOver)
                    {
                        //Tentukan nilai random untuk one cut point
                        valRandom1 = r.Next(3, totData - 2);
                        noParent1 = r.Next(0, valPopSize);
                        noParent2 = r.Next(0, valPopSize);

                        tempParents1 = new string[totData];
                        tempParents2 = new string[totData];
                        tempChild = new string[totData];


                        if (noParent1 != noParent2)
                        {
                            if (z < 3)
                            {
                                rtbSolusi.Text += "\nCrossover " + (x + 1).ToString() + ": Parents " + (noParent1 + 1).ToString() + " + Parents " + (noParent2 + 1).ToString() + "\n";
                            }

                            for (int i = 0; i < totData; i++)
                            {
                                tempParents1[i] = parents[noParent1, i];
                                tempParents2[i] = parents[noParent2, i];
                            }
                            
                            //Proses crossover dilakukan dengan panggil kelas reproduksi
                            rep = new reproduksi(tempParents1, tempParents2, valRandom1);
                            rep.reproduksiCrossover();
                            tempChild = rep.child;

                            tempIndividu = "";
                            //Menyimpan kromosom child dari array 1 dimensi ke 2 dimensi
                            for (int i = 0; i < totData; i++)
                            {
                                childs[x, i] = tempChild[i];
                                if (z < 3)
                                {
                                    rtbSolusi.Text += tempChild[i].ToString() + " ";
                                }
                                tempIndividu += tempChild[i].ToString() + " ";
                            }
                            strIndividu[valPopSize + x] = tempIndividu;
                        }
                        else
                        {
                            x--;
                        }

                        x++;
                    }

                    if (z < 3)
                    {
                        rtbSolusi.Text += "\n\n=========================================================================================\n\n";
                        rtbSolusi.Text += "Create Children From Mutation";
                    }

                    x = 0;
                    //r = new Random();
                    //r = new Random();
                    
                    //Proses Mutasi
                    while (x < totChildMutation)
                    {
                        //Ambil nilai random untuk menentukan titik EXP 1 dan EXP2
                        valRandom1 = r.Next(1, totData / 2);
                        valRandom2 = r.Next(totData / 2, totData);

                        noParent1 = r.Next(0, valPopSize);
                        tempParents1 = new string[totData];
                        tempChild = new string[totData];

                        if (z < 3)
                        {
                            rtbSolusi.Text += "\nMutasi " + (x + 1).ToString() + ": Parents " + (noParent1 + 1).ToString() + "\n";
                        }

                        for (int i = 0; i < totData; i++)
                        {
                            tempParents1[i] = parents[noParent1, i];
                        }

                        //Melakukan proses mutasi dengan memanggil kelas reproduksi
                        rep = new reproduksi(tempParents1,valRandom1,valRandom2);
                        rep.reproduksiMutasi();
                        tempChild = rep.child;

                        //Menyimpan kromosom child dari array 1 dimensi ke 2 dimensi
                        tempIndividu = "";
                        
                        for (int i = 0; i < totData; i++)
                        {
                            childs[x + totChildCrossOver, i] = tempChild[i];
                            if (z < 3)
                            {
                                rtbSolusi.Text += tempChild[i].ToString() + " ";
                            }
                            tempIndividu += tempChild[i].ToString() + " ";
                        }
                        strIndividu[valPopSize + totChildCrossOver + x] = tempIndividu;

                        x++;
                    }

                    if (z < 3)
                    {
                        rtbSolusi.Text += "\n\n=========================================================================================\n\n";
                        rtbSolusi.Text += "Generasi " + (z + 1).ToString() + " : Hasil Reproduksi\n";
                    }

                    fitnessChild = new double[totChild];
                   
                    //Menampilkan semua fitness dari proses reproduksi
                    for (int i = 0; i < totChild; i++)
                    {
                        if (z < 3)
                        {
                            rtbSolusi.Text += (i + 1).ToString() + "\t";
                        }

                        tempChild = new string[totData];

                        for (int j = 0; j < totData; j++)
                        {
                            if (z < 3)
                            {
                                rtbSolusi.Text += childs[i, j].ToString() + " ";
                            }
                            tempChild[j] = childs[i, j].ToString();
                            newParents[valPopSize + i, j] = childs[i, j];
                        }
                        //Menghitung nilai fitness
                        seleksi s = new seleksi(tempChild, matrixWaktu, jadwalBuka, jadwalTutup);
                        s.cariNilaiFitness();
                        
                        pinalti[i+valPopSize] = s.fitness;
                        fitnessChild[i] = Math.Round(1/((Convert.ToDouble(s.fitness+1) / 60)), 4);
                        if (z < 3)
                        {
                            rtbSolusi.Text += "\tpenalti = " + s.fitness.ToString();
                            rtbSolusi.Text += "\tFitness = " + fitnessChild[i].ToString();
                            rtbSolusi.Text += "\n";
                        }
                        totFitness += fitnessChild[i];
                        fitness[valPopSize + i] = fitnessChild[i];
                    }

                    if (z < 3)
                    {
                        rtbSolusi.Text += "\n\n=========================================================================================\n\n";
                        rtbSolusi.Text += "Generasi " + (z + 1).ToString() + " : Seleksi individu menggunakan roulette wheel\n";
                    }

                    prob = 0;
                    totProbCum = 0;

                    totGenerasi = valPopSize + totChild;
                    probCum = new double[totGenerasi];

                    //Menghitung nilai probabilitas
                    for (int i = 0; i < totGenerasi; i++)
                    {
                        prob = fitness[i] / totFitness;
                        //prob = Math.Round(prob, 4);
                        totProbCum += prob;
                        probCum[i] = Math.Round(totProbCum, 4);
                        if (z < 3)
                        {
                            rtbSolusi.Text += (i + 1).ToString() + "\t" + strIndividu[i].ToString() + "\tPinalti = " + pinalti[i].ToString() + "\t" + "Fitness = " + fitness[i] + "\tProb Cum = " + probCum[i].ToString() + "\n";
                        }
                    }

                    if (z < 3)
                    {
                        rtbSolusi.Text += "\n\n=========================================================================================\n\n";
                        rtbSolusi.Text += "Individu Terpilih\n";
                    }


                    tempDouble = 0;
                    //r = new Random();

                    individuTerpilih = new string[valPopSize];
                    fitnessTerpilih = new double[valPopSize];
                    penaltiTerpilih = new double[valPopSize];

                    tempRata = 0;
                    int terbaik = 0;
                    
                    //Melakukan proses seleksi dengan proses roulette wheel
                    for (int i = 0; i < valPopSize; i++)
                    {
                        tempDouble = r.NextDouble();
                        x = 0;
                        int terpilih = 0;
                        while (probCum[x] < tempDouble)
                        {
                            terpilih = x + 1;
                            x++;
                        }

                        tempRata += fitness[terpilih];
                        individuTerpilih[i] = strIndividu[terpilih];
                        fitnessTerpilih[i] = fitness[terpilih];
                        penaltiTerpilih[i] = pinalti[terpilih];

                        if (z < 3)
                        {
                            rtbSolusi.Text += (i + 1).ToString() + "\tRandom = " + (Math.Round(tempDouble, 4)).ToString() + "\t\t" + strIndividu[terpilih].ToString() + "\tPenalti = " + pinalti[terpilih].ToString() + "\t" + (terpilih + 1).ToString() + "\tFitness =" + fitnessTerpilih[i].ToString() + "\n";
                        }
                        if (fitnessTerpilih[terbaik] > fitnessTerpilih[i])
                        {
                            terbaik = terbaik;
                        }
                        else
                        {
                            terbaik = i;
                        }

                        for (int j = 0; j < totData; j++)
                        { 
                            parents[i,j] = newParents[terpilih,j];
                        }
                    }

                    tempRata = tempRata / valPopSize;

                    if (z < 3)
                    {
                        rtbSolusi.Text += "Nilai Rata-rata fitness = " + tempRata.ToString();
                        rtbSolusi.Text += "\nIndividu terbaik adalah : " + individuTerpilih[terbaik].ToString() + "\t" + penaltiTerpilih[terbaik].ToString() + "\tFitness = " + fitnessTerpilih[terbaik].ToString() + "\n\n";
                    }
                    generasiBestIndividu[z] = individuTerpilih[terbaik].ToString();
                    generasiPinalti[z] = penaltiTerpilih[terbaik];
                    generasiRataRata[z] = tempRata;
                    generasiFitnessTerbaik[z] = fitnessTerpilih[terbaik];
                    z++;
                }

                //Memilih individu terbaik untuk generasi N
                int tempTerbaik = 0;
                for (int i = 0; i < generasi; i++)
                {
                    if (generasiFitnessTerbaik[tempTerbaik] >= generasiFitnessTerbaik[i])
                    {
                        tempTerbaik = tempTerbaik;
                    }
                    else
                    {
                        tempTerbaik = i;
                    }
                    row = new string[] { (i+1).ToString(), generasiBestIndividu[i].ToString(), generasiPinalti[i].ToString(), generasiFitnessTerbaik[i].ToString(), generasiRataRata[i].ToString()};
                    dgvResult.Rows.Add(row);
                }

                //Menampilkan individu terbaik dari semua generasi
                rtbSolusi.Text += "\n\n=========================================================================================\n\n";
                rtbSolusi.Text += "Generasi terbaik didapatkan pada" +
                "\ngenerasi = " + (tempTerbaik + 1).ToString() +
                "\nKromosom = " + generasiBestIndividu[tempTerbaik].ToString() +
                "\nPinalti = " + generasiPinalti[tempTerbaik].ToString() +
                "\nFitness = " + generasiFitnessTerbaik[tempTerbaik].ToString() +
                "\nRata-rata = " + generasiRataRata[tempTerbaik].ToString();
                rtbSolusi.Text += "\nDetail Solusi";
                rtbSolusi.Text += "\n=============================================================================================\n";


                string[] strind = generasiBestIndividu[tempTerbaik].Split(' ');
                string[] newStrind = new string[totData];

                for (int i = 0; i < totData; i++)
                {
                    newStrind[i] = strind[i];
                }

                //tempJadwalBuka = new string[totData];
                //tempJadwalTutup = new string[totData];
                //seleksi sel = new seleksi(jadwalBuka, jadwalTutup,totData);
                //sel.ambilJadwal();
                //tempJadwalBuka = sel.tempJadwalBuka;
                //tempJadwalTutup = sel.tempJadwalTutup;

                int node = 0;

                //string[] newJadwalBuka = new string[totData];
                //newJadwalBuka = newStrind;
                //rtbSolusi.Text += strind.Length.ToString();
                //t1 = new TimeSpan();
                //t2 = new TimeSpan();
                tempWaktuDatang = new string[totData];
                tempWaktuBerangkat = new string[totData];
                penalti = new int[totData];
                tempWaktuTempuh = new int[totData];

                //Menampilkan detail perhitungan untuk the best individu
                seleksi sel = new seleksi(newStrind,matrixWaktu,jadwalBuka,jadwalTutup);
                sel.cariNilaiFitness();
                tempWaktuDatang = sel.tempWaktuDatang;
                tempWaktuBerangkat = sel.tempWaktuBerangkat;
                penalti = sel.penalti;
                tempWaktuTempuh = sel.waktuTempuh;

                rtbSolusi.Text += "Berngkat dari node 0 pada pukul 2.00\n";

                for (int i = 1; i < newStrind.Length; i++)
                {
                    node = Convert.ToInt32(newStrind[i].ToString());
                    
                    rtbSolusi.Text += node.ToString() + "\tBuka = " + (jadwalBuka[node].ToString()).Substring(10,5) + "\tTutup = " + (jadwalTutup[node].ToString()).Substring(10,5) + "\tWaktu Tempuh = "+ tempWaktuTempuh[i].ToString()+
                        "\tDatang = "+ (tempWaktuDatang[i].ToString()).Substring(0,5) +"\tPelayanan = 5" +"\tWaktu Berangkat = "+(tempWaktuBerangkat[i].ToString()).Substring(0,5)+"\tPenalti = "+ penalti[i].ToString()+"\n";
                    //rtbSolusi.Text += strind[i].ToString() + ",";
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                //string time1 = "03:1";
                //int coba = 60;
                //TimeSpan t = new TimeSpan();
                //DateTime t1 = Convert.ToDateTime(time1);
                //DateTime tes = Convert.ToDateTime(time1);
                //tes = tes.AddMinutes(coba);
                //DateTime ts;

                //TimeSpan t2 = Convert.ToDateTime(time1).TimeOfDay;
                //TimeSpan t3 = t2.Add(new TimeSpan(0,coba,0));
                //TimeSpan tot;

                //tot = t3.Add(t3);
                //string tot;
                //tot = (t1.AddMinutes(90)).ToShortTimeString();
                //tot = tot.Substring(0, 5);
                //textBox1.Text = t3.ToString();

                ////tot = ((Convert.ToDateTime(time1)) + (Convert.ToDateTime(time2)));

                //if (t2 > t3)
                //{
                //    textBox1.Text = "TRUE";
                //}
                //else
                //{
                //    textBox1.Text = "FALSE";
                //}


                string[] contoh = new string[] { "0", "4", "1", "2", "3", "6", "7", "5", "13", "15", "12", "14", "16", "8", "10", "9", "11" };
                seleksi s = new seleksi(contoh, matrixWaktu, jadwalBuka, jadwalTutup);
                s.cariNilaiFitness();
                //textBox1.Text = (Convert.ToDouble(1/Convert.ToDouble(s.fitness))).ToString();
                textBox1.Text =s.fitness.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }
        }

       
    }
}

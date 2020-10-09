using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace Hill
{
    public partial class Form1 : Form
    {
        private Random ran = new Random();
        int L = 3;
        string[] str = new string[]
 {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v",
"w","x","y","z","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S",
"T","U","V","W","X","Y","Z"," ","~","`","!","@","#","$","%","^","&","*","(",")","-","_","+",
"=","|","\\","\"","\n","\t","{","[","]","}",":",";","'","<",",",".",">","?","/","0","1","2",
"3","4","5","6","7","8","9"};

        public Form1()
        {
            InitializeComponent();
            this.openFileDialog1.Filter = "(*.txt)|*txt";

        }
        private void FillToText(OpenFileDialog openFileDialog, TextBox textbox)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fileStream = this.openFileDialog1.OpenFile();
                StreamReader reader = new StreamReader(fileStream);
                if (reader != null)
                {
                    textbox.Text = reader.ReadToEnd();
                    reader.Close();
                }
            }

        }

        private void btnInnitK_Click(object sender, EventArgs e)
        {
            this.txtKey.Text = "";
            this.L = Convert.ToInt32(this.txtL.Text);
            int L_square = L * L;
            this.txtL.Text = L.ToString();
            for (int i = 0; i < L_square; i++)
            {
                txtKey.Text += ran.Next(1, 99) + " ";
            }
            txtKey.Text = txtKey.Text.Trim();
        }

        private void btnMaHoa_Click(object sender, EventArgs e)
        {

            FillToText(this.openFileDialog1, this.txtPlainText);


        }

        private void btnGiaiMa_Click(object sender, EventArgs e)
        {
            FillToText(this.openFileDialog1, this.txtCipher);
        }

        private void btnSaveKey_Click(object sender, EventArgs e)
        {
            FileStream fileStream = File.Create("E:/key.txt");
            StreamWriter streamWriter = new StreamWriter(fileStream);
            if (fileStream.CanWrite)
            {
                streamWriter.Write(this.txtKey.Text);
            }

            if (streamWriter != null)
            {
                streamWriter.Close();
            }

            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
        public double[,] getMatrixFromText(String text)
        {
            int i = 0;
            int j = 0;

            string[] arrayString = text.Split(' ');
            double[,] matrix = new double[L, L];

            foreach (string str in arrayString)
            {
                if (Double.TryParse(str, out matrix[i, j]))
                {

                    if (j + 1 < L)
                    {
                        j = j + 1;
                    }
                    else
                    {
                        j = 0;
                        i = i + 1;
                    }

                    if (i + 1 > L)
                    {
                        break;
                    }
                }
            }
            return matrix;
        }
        public void MatrixRevert(TextBox textBox)
        {

            double[,] matrix = new double[L, L];

            matrix = getMatrixFromText(this.txtKey.Text);

            int i = 0;
            int j = 0;


            int det = 0;
            double[,] detMatrix = getInverseMatrix(matrix, L);

            for (i = 0; i < L; i++)
            {
                det = det + (int)(matrix[0, i] * detMatrix[0, i]);
            }

            det = det % 97;

            if (det < 0)
            {
                det = det + 97;
            }

            int idet = findIdet(det, 97);

            double[,] ikMatrix = new double[L, L];
            double temp;
            for (i = 0; i < L; i++)
            {
                for (j = 0; j < L; j++)
                {
                    temp = (idet * detMatrix[i, j]) % 97;
                    if (temp < 0)
                    {
                        temp = temp + 97;
                    }
                    ikMatrix[j, i] = temp;


                }

            }
            for (i = 0; i < L; i++)
            {
                for (j = 0; j < L; j++)
                {
                    Console.Write(ikMatrix[i, j].ToString() + " ");
                    txtInvertKey.Text += ikMatrix[i, j].ToString() + " ";
                }
                Console.WriteLine();
            }
            txtInvertKey.Text = txtInvertKey.Text.Trim();

            /*
            if (L == 1)
            {
                delMatrix = matrix[0, 0];
            }

            if (L == 2)
            {
                delMatrix = matrix[0, 0] * matrix[0, 1] - matrix[1, 0] * matrix[0, 1];

                matrixAS[0, 0] = matrix[1, 1];
                matrixAS[0, 1] = -matrix[0, 1];
                matrixAS[1, 0] = -matrix[1, 0];
                matrixAS[1, 1] = matrix[0, 0];
            }

            if (L == 3)
            {
               

            }

            inverseMatrix = matrix;

            this.txtInvertKey.Text = this.txtInvertKey.Text.Trim();
            //Convert.ToDouble("-186/-115870");
            //MessageBox.Show(this,delMatrix.ToString()); */

        }
        public double DetMatrixLevel3(double[,] matrix)
        {
            return matrix[0, 0] * matrix[1, 1] * matrix[2, 2] +
                    matrix[0, 1] * matrix[1, 2] * matrix[2, 0] +
                    matrix[0, 2] * matrix[1, 0] * matrix[2, 1] -
                    matrix[0, 2] * matrix[1, 1] * matrix[2, 0] -
                    matrix[0, 1] * matrix[1, 0] * matrix[2, 2] -
                    matrix[0, 0] * matrix[1, 2] * matrix[2, 1];
        }

        public double DetMatrixLevel2(double[,] matrix)
        {
            return matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtInvertKey.Text = "";
            MatrixRevert(this.txtKey);

        }

        private double[,] getInverseMatrix(double[,] matrix, int L)
        {
            /*
            double delMatrix = DelMatrixLevel3(matrix)%97;
            if (delMatrix < 0)
            {
                delMatrix = delMatrix + 97;
            }
            int i = 0;
            int j = 0;
            double[,] matrixAS = new double[L, L];
            double[,] temp = new double[L - 1, L - 1];
            i = 0;
            j = 0;
            int startX = 0, startY = 0;
            int lim = L - 1;
            int x = 0;
            int y = 0;
            while (i < L)
            {
                startX = 0;
                startY = 0;
                x = 0;
                y = 0;

                while (x < L)
                {
                    if (x == i)
                    {
                        x++;

                        continue;
                    }
                    if (y == j)
                    {
                        y++;
                        if (y >= L)
                        {
                            y = 0;
                            x++;
                        }
                        continue;
                    }

                    temp[startX, startY++] = matrix[x, y++];

                    if (y >= L)
                    {
                        y = 0;
                        x++;
                    }
                    if (startY >= lim)
                    {
                        startY = 0;
                        startX++;
                    }
                }


                matrixAS[j, i] = Math.Pow(-1, j) * DelMatrixLevel2(temp);
                j++;

                if (j >= L)
                {
                    j = 0;
                    i++;
                }
            }

            for (i = 0; i < L; i++)
            {
                for (j = 0; j < L; j++)
                {
                    matrix[i, j] = matrixAS[i, j] / delMatrix;
                    this.txtInvertKey.Text += matrixAS[i, j] + "/" + delMatrix + " ";
                }
            }
            */
            double[,] reverseMatrix = new double[L, L];
            int lim = L - 1;

            if (L == 1)
            {
                reverseMatrix[0, 0] = matrix[0, 0];

            }
            else if (L == 2)
            {

                reverseMatrix[0, 0] = matrix[1, 1];
                reverseMatrix[0, 1] = matrix[1, 0];
                reverseMatrix[1, 0] = matrix[0, 1];
                reverseMatrix[1, 1] = matrix[0, 0];
            }
            else if (L >= 3)
            {


                int startX = 0;
                int startY = 0;
                double[,] temp = new double[lim, lim];

                for (int i = 0; i < L; i++)
                {
                    for (int j = 0; j < L; j++)
                    {

                        startX = 0;
                        startY = 0;

                        for (int k = 0; k < L; k++)
                        {
                            if (k == i)
                            {
                                continue;
                            }

                            for (int q = 0; q < L; q++)
                            {
                                if (q == j)
                                {
                                    continue;
                                }

                                temp[startX, startY++] = matrix[k, q];

                                if (startY == lim)
                                {
                                    startY = 0;
                                    startX = startX + 1;
                                }
                            }

                        }
                        if (L == 4)
                        {
                            reverseMatrix[i, j] = Math.Pow(-1, j + i) * DetMatrixLevel3(temp);
                        }
                        else if (L > 4)
                        {

                            reverseMatrix = getInverseMatrix(temp, L - 1);
                        }
                        else
                        {

                            reverseMatrix[i, j] = Math.Pow(-1, j + 1) * DetMatrixLevel2(temp);
                        }
                    }
                }
            }

            return reverseMatrix;
        }

        private int findIdet(int det, int total)
        {
            int a1 = 1;
            int a2 = 0;
            int a3 = total;
            int b1 = 0;
            int b2 = 1;
            int b3 = det;
            int q, t1, t2 = 0, t3 = 0;

            while (t3 != 1)
            {
                q = a3 / b3;
                t1 = a1 - q * b1;
                t2 = a2 - q * b2;
                t3 = a3 - q * b3;

                a1 = b1;
                a2 = b2;
                a3 = b3;
                b1 = t1;
                b2 = t2;
                b3 = t3;
            }

            return t2;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            long time = (DateTime.Now.Ticks);
            this.txtCipher.Text = "";
            this.setTextFromTextBox(this.txtPlainText,this.txtKey,this.txtCipher,true);
           
            this.textBox6.Text = String.Format("{0:0.00}", (DateTime.Now.Ticks - time)/10000);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            long time = (DateTime.Now.Ticks);
            this.txtPlainText.Text = "";
            this.setTextFromTextBox(this.txtCipher, this.txtInvertKey,this.txtPlainText,false);
            this.textBox7.Text = String.Format("{0:0.00}", (DateTime.Now.Ticks - time) / 10000);
        }
       
        public void setTextFromTextBox(TextBox fromText,TextBox key, TextBox toText,bool encrypt)
        {
            
            double[,] matrix = new double[L, L];

            matrix = getMatrixFromText(key.Text);
            string text = fromText.Text;
            int length = text.Length;
            int[] vector = new int[L];
            
            if (length % L != 0 && encrypt) {
                int num = length % L;
                for (int i = 0; i < num; i++) {
                    text += " ";
                }
                length = text.Length;
            }

            string subText = "";

            for(int i = 0; i < length; i++)
            {
                subText += text[i];
                if (subText.Length == 4)
                {
                   
                    crypted(vector, matrix, subText, toText);
                    subText = "";
                }
                
            }
           





        }
        public void crypted(int [] vector,double[,] matrix,string text, TextBox toText)
        {
            int temp;
            for (int i = 0; i < L; i++)
            {
                temp = 0;

                foreach (string s in this.str)
                {
                    if (Convert.ToString(text[i]).Equals(s))
                    {
                        vector[i] = temp;
                        break;
                    }

                    temp++;
                }

            }

            int[] kq = new int[L];

            for (int i = 0; i < L; i++)
            {
                temp = 0;
                for (int j = 0; j < L; j++)
                {
                    temp += (int) matrix[i, j] * vector[j];
                }

                kq[i] = temp % 97;
            }

            foreach (int number in kq)
            {
                toText.Text += str[number];
            }
        }
    }
}

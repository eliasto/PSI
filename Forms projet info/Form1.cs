using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms_projet_info
{
    public partial class Form1 : Form
    {
        private MyImage image;
        private Color colorWheel = Color.Black;
        private MyImage logo = null;
        private MyImage fractale;
        
        private MyImage cache;
        private MyImage source;
        private MyImage result;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.image.NB();

            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\elias\\Desktop\\projet info\\projet info\\bin\\Debug\\image";
            openFileDialog1.Filter = "Image Files(*.BMP)|*.BMP";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = false;
                string selectedFileName = openFileDialog1.FileName;

                this.image = new MyImage(selectedFileName);
                this.image.From_Image_To_File("./original.bmp");
                Bitmap b = new Bitmap(selectedFileName);

                pictureBox1.Image = b;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox6.Image = b;
                pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.image.Nuance();
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.image.SymmetrieX();
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.image.SymmetrieY();
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            this.image.SymmetrieXY();
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.image.Tourner(90);
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.image.Tourner(180);
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            this.image.Tourner(270);
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.image = new MyImage("./original.bmp");
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        //enregistrement
        private void button11_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                label3.Visible = true;
                this.image.From_Image_To_File(saveFileDialog1.FileName);
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {
            button11.Enabled = true;
            button11.Visible = true;
            label3.Visible = false;

        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            this.image.Miroir();
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.image.Rotation((double)numericUpDown1.Value);
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                label6.Visible = true;
                this.image.From_Image_To_File(saveFileDialog1.FileName);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            button15.Visible = true;
            if(this.logo == null)
            {
                MyImage qrcode = MyImage.QRCode(textBox1.Text, Convert.ToInt32(numericUpDown2.Value), this.colorWheel);
                image = qrcode;
                MemoryStream ms = new MemoryStream(qrcode.From_Image_To_Array());
                Image bitmap = Image.FromStream(ms);
                pictureBox3.Image = bitmap;
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            } else
            {
                MyImage qrcode = MyImage.QRCode(textBox1.Text, Convert.ToInt32(numericUpDown2.Value), this.colorWheel);
                image = qrcode;
                qrcode = MyImage.ImageDansQrCode(qrcode, this.logo);
                MemoryStream ms = new MemoryStream(qrcode.From_Image_To_Array());
                Image bitmap = Image.FromStream(ms);
                pictureBox3.Image = bitmap;
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            }

            

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.colorWheel = colorDialog1.Color;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\elias\\Desktop\\projet info\\projet info\\bin\\Debug\\image";
            openFileDialog1.Filter = "Image Files(*.BMP)|*.BMP";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                this.image = new MyImage(selectedFileName);
                this.image.From_Image_To_File("./original.bmp");
                Bitmap b = new Bitmap(selectedFileName);

                pictureBox4.Image = b;
                pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                this.logo = this.image;
                
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.logo = null;
            pictureBox4.Image = null;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void button31_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\elias\\Desktop\\projet info\\projet info\\bin\\Debug\\image";
            openFileDialog1.Filter = "Image Files(*.BMP)|*.BMP";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = false;
                string selectedFileName = openFileDialog1.FileName;

                this.image = new MyImage(selectedFileName);
                this.image.From_Image_To_File("./original.bmp");
                Bitmap b = new Bitmap(selectedFileName);

                pictureBox1.Image = b;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox6.Image = b;
                pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            button30.Enabled = true;
            button30.Visible = true;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                double[,] convolution = { { Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox4.Text), Convert.ToDouble(textBox3.Text) }, { Convert.ToDouble(textBox7.Text), Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text) }, { Convert.ToDouble(textBox10.Text), Convert.ToDouble(textBox8.Text), Convert.ToDouble(textBox9.Text) } };
                this.image.Convolution(convolution);

            }
            catch
            {
                string message = "La matrice de convolution n'est pas valide !";
                string caption = "Erreur";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
            }
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox5.Image = bitmap;
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            textBox2.Text = "-1";
            textBox4.Text = "-1";
            textBox3.Text = "-1";
            textBox7.Text = "-1";
            textBox5.Text = "8";
            textBox6.Text = "-1";
            textBox10.Text = "-1";
            textBox8.Text = "-1";
            textBox9.Text = "-1";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBox2.Text = "0,0625";
            textBox4.Text = "0,125";
            textBox3.Text = "0,0625";
            textBox7.Text = "0,125";
            textBox5.Text = "0,25";
            textBox6.Text = "0,125";
            textBox10.Text = "0,0625";
            textBox8.Text = "0,125";
            textBox9.Text = "0,0625";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            this.image = new MyImage("./original.bmp");
            MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox5.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                label7.Visible = true;
                this.image.From_Image_To_File(saveFileDialog1.FileName);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            MyImage histo = this.image.Histogramme(2);
            MemoryStream ms = new MemoryStream(histo.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button24_Click(object sender, EventArgs e)
        {

            MyImage histo = this.image.Histogramme(1);
            MemoryStream ms = new MemoryStream(histo.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button25_Click(object sender, EventArgs e)
        {

            MyImage histo = this.image.Histogramme(3);
            MemoryStream ms = new MemoryStream(histo.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox2.Image = bitmap; pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            button27.Enabled = false;
            MyImage fractale = MyImage.Fractale((int)numericUpDown3.Value, (int)numericUpDown4.Value);
            MemoryStream ms = new MemoryStream(fractale.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);

            this.fractale = fractale;

            button27.Enabled = true;
            pictureBox7.Image = bitmap; pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            string message = "Votre image a bien été générée !";
            string caption = "Succès";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button27_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                this.fractale.From_Image_To_File(saveFileDialog1.FileName);
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button32_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\elias\\Desktop\\projet info\\projet info\\bin\\Debug\\image";
            openFileDialog1.Filter = "Image Files(*.BMP)|*.BMP";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                this.source = new MyImage(selectedFileName);
                Bitmap b = new Bitmap(selectedFileName);

                pictureBox10.Image = b;
                pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox8.Image = b;
                pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
                button34.Enabled = false;
                button29.Enabled = true;
                button28.Enabled = true;
                this.cache = null;
                pictureBox9.Image = null;
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\elias\\Desktop\\projet info\\projet info\\bin\\Debug\\image";
            openFileDialog1.Filter = "Image Files(*.BMP)|*.BMP";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                this.cache = new MyImage(selectedFileName);
                Bitmap b = new Bitmap(selectedFileName);

                pictureBox9.Image = b;
                pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click_1(object sender, EventArgs e)
        {
            this.source.Caché(this.source.Image, this.cache.Image);
            this.result = this.source;

            MemoryStream ms = new MemoryStream(this.result.From_Image_To_Array());
            Image bitmap = Image.FromStream(ms);


            pictureBox10.Image = bitmap; pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
            button34.Enabled = true;
            string message = "Votre image a bien été générée !";
            string caption = "Succès";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
            

        }

        private void button29_Click(object sender, EventArgs e)
        {

        }

        private void button34_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                this.result.From_Image_To_File(saveFileDialog1.FileName);
                string message = "Le fichier a été sauvegardé !";
                string caption = "Succès";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click_1(object sender, EventArgs e)
        {

            try
            {
                this.source.Cherché();
                this.result = this.source;

                MemoryStream ms = new MemoryStream(this.result.From_Image_To_Array());
                Image bitmap = Image.FromStream(ms);


                pictureBox10.Image = bitmap; pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
                button34.Enabled = true;
                string message = "L'image a été décodé !";
                string caption = "Succès";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
            }
            catch
            {
                string message1 = "Cette image ne peut pas être décodé !";
                string caption1 = "Erreur";
                MessageBoxButtons buttons1 = MessageBoxButtons.OK;
                DialogResult result1;

                // Displays the MessageBox.
                result1 = MessageBox.Show(message1, caption1, buttons1);
            }
            
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void button36_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\elias\\Desktop\\projet info\\projet info\\bin\\Debug\\image";
            openFileDialog1.Filter = "Image Files(*.BMP)|*.BMP";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                this.image = new MyImage(selectedFileName);
                this.image.From_Image_To_File("./original.bmp");
                Bitmap b = new Bitmap(selectedFileName);
                button35.Visible = true;

                button37.Visible = true;
                button38.Visible = true;


                pictureBox12.Image = b;
                pictureBox12.SizeMode = PictureBoxSizeMode.Zoom;
                
            }
        }

        private void button35_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                this.image.From_Image_To_File(saveFileDialog1.FileName);
                string message = "L'image a été sauvegardé !";
                string caption = "Succès";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
            }
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void button38_Click(object sender, EventArgs e)
        {
            try
            {
                this.image.Rétrécissement(Convert.ToInt32(textBox14.Text));

                MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
                Image bitmap = Image.FromStream(ms);
                button35.Enabled = true;

                pictureBox11.Image = bitmap; 
                pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
                string message = "L'image a été rétréci !";
                string caption = "Succès";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
            }
            catch
            {
                string message1 = "Cette image ne peut pas être rétréci !";
                string caption1 = "Erreur";
                MessageBoxButtons buttons1 = MessageBoxButtons.OK;
                DialogResult result1;

                // Displays the MessageBox.
                result1 = MessageBox.Show(message1, caption1, buttons1);
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            try
            {
                this.image.Agrandir(Convert.ToInt32(textBox14.Text));

                MemoryStream ms = new MemoryStream(this.image.From_Image_To_Array());
                Image bitmap = Image.FromStream(ms);
                button35.Enabled = true;

                pictureBox11.Image = bitmap; pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
                string message = "L'image a été agrandi !";
                string caption = "Succès";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
            }
            catch
            {
                string message1 = "Cette image ne peut pas être agrandi !";
                string caption1 = "Erreur";
                MessageBoxButtons buttons1 = MessageBoxButtons.OK;
                DialogResult result1;

                // Displays the MessageBox.
                result1 = MessageBox.Show(message1, caption1, buttons1);
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

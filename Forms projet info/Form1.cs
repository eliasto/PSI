using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms_projet_info
{
    public partial class Form1 : Form
    {
        private MyImage image;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.image.NB();
            this.image.From_Image_To_File("./nb.bmp");
            pictureBox2.Image = new Bitmap("./nb.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\elias\\Desktop\\projet info\\projet info\\bin\\Debug\\image";
            openFileDialog1.Filter = "Image Files(*.BMP)|*.BMP";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                groupBox1.Enabled = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = false;
                string selectedFileName = openFileDialog1.FileName;

                this.image = new MyImage(selectedFileName);
                this.image.From_Image_To_File("./original.bmp");
                Bitmap b = new Bitmap(selectedFileName);

                pictureBox1.Image = b;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.image.Nuance();
            this.image.From_Image_To_File("./nuance.bmp");
            pictureBox2.Image = new Bitmap("./nuance.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.image.SymmetrieX();
            this.image.From_Image_To_File("./symX.bmp");
            pictureBox2.Image = new Bitmap("./symX.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.image.SymmetrieY();
            this.image.From_Image_To_File("./symY.bmp");
            pictureBox2.Image = new Bitmap("./symY.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            this.image.SymmetrieXY();
            this.image.From_Image_To_File("./symXY.bmp");
            pictureBox2.Image = new Bitmap("./symXY.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.image.Tourner90();
            this.image.From_Image_To_File("./t90.bmp");
            pictureBox2.Image = new Bitmap("./t90.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.image.Tourner180();
            this.image.From_Image_To_File("./t180.bmp");
            pictureBox2.Image = new Bitmap("./t180.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            this.image.Tourner270();
            this.image.From_Image_To_File("./t270.bmp");
            pictureBox2.Image = new Bitmap("./t270.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            this.image = new MyImage("./original.bmp");
            pictureBox2.Image = new Bitmap("./original.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        //enregistrement
        private void button11_Click(object sender, EventArgs e)
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            button11.Enabled = true;
            button11.Visible = true;
            label3.Visible = false;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.image.Miroir();
            this.image.From_Image_To_File("./miroir.bmp");
            pictureBox2.Image = new Bitmap("./miroir.bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

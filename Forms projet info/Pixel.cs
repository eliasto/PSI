using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms_projet_info
{
    class Pixel
    {
        private int R;
        private int G;
        private int B;
        private int A;

        public Pixel(int R, int G, int B, int A = 0)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public int Moyenne()
        {
            int moyenne = (this.R + this.G + this.B) / 3;
            return moyenne;
        }

        public static Pixel Moyenne_1(Pixel[,] mat)
        {
            Pixel moyenne;
            int rouge = 0;
            int vert = 0;
            int bleu = 0;
            int alpha = 0;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    rouge = rouge + mat[i, j].r;
                    vert = vert + mat[i, j].g;
                    bleu = bleu + mat[i, j].b;
                    alpha = alpha + mat[i, j].a;
                }
            }
            rouge = rouge / (mat.GetLength(0) * mat.GetLength(1));
            vert = vert / (mat.GetLength(0) * mat.GetLength(1));
            bleu = bleu / (mat.GetLength(0) * mat.GetLength(1));
            alpha = alpha / (mat.GetLength(0) * mat.GetLength(1));
            moyenne = new Pixel(rouge, vert, bleu, alpha);
            return moyenne;
        }

        public static Pixel operator +(Pixel a, Pixel b)
        {
            int rMoyenne = b.R + a.R;
            int gMoyenne = a.G + b.G;
            int bMoyenne = a.B + b.B;
            int aMoyenne = a.A + b.A;

            return new Pixel(rMoyenne, gMoyenne, bMoyenne, aMoyenne);
        }

        public static Pixel operator /(Pixel a, int b)
        {
            int rMoyenne = a.R / b;
            int gMoyenne = a.G / b;
            int bMoyenne = a.B / b;
            int aMoyenne = a.A / b;

            return new Pixel(rMoyenne, gMoyenne, bMoyenne, aMoyenne);
        }


        public int r
        {
            get { return this.R; }
            set { this.R = value; }
        }
        public int g
        {
            get { return this.G; }
            set { this.G = value; }
        }
        public int b
        {
            get { return this.B; }
            set { this.B = value; }
        }
        public int a
        {
            get { return this.A; }
            set { this.A = value; }
        }
    }
}

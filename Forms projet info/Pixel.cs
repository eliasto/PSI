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

        public int r
        {
            get { return this.R; }
        }
        public int g
        {
            get { return this.G; }
        }
        public int b
        {
            get { return this.B; }
        }
        public int a
        {
            get { return this.A; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Forms_projet_info
{
    class MyImage
    {
        private string type;
        private int taille;
        private int offset;
        private int largeur;
        private int hauteur;
        private int nombreDeBitsParCouleur;
        private Pixel[,] image;

        public MyImage(string path)
        {
            try
            {
                byte[] myfile = File.ReadAllBytes(path);

                byte[] heightByte = { myfile[18], myfile[19], myfile[20], myfile[21] };
                int height = Convertir_Endian_To_Int(heightByte);

                byte[] widthByte = { myfile[22], myfile[23], myfile[24], myfile[25] };
                int width = Convertir_Endian_To_Int(widthByte);

                byte[] tailleByte = { myfile[2], myfile[3], myfile[4], myfile[5] };
                int taille = Convertir_Endian_To_Int(tailleByte);

                byte[] offsetByte = { myfile[10], myfile[11], myfile[12], myfile[13] };
                int offset = Convertir_Endian_To_Int(offsetByte);

                byte[] numberOfPixelsByte = { myfile[28], myfile[29] };
                int numberOfPixels = Convertir_Endian_To_Int(numberOfPixelsByte);

                this.hauteur = height;
                this.largeur = width;
                this.offset = offset;
                this.taille = taille;
                this.nombreDeBitsParCouleur = numberOfPixels;
                this.type = (char)myfile[0] + "" + (char)myfile[1];

                Pixel[,] tab = new Pixel[this.largeur, this.hauteur];

                int position = this.offset;
                for (int i = 0; i < tab.GetLength(0); i++)
                {

                    for (int j = 0; j < tab.GetLength(1); j++)
                    {
                        if (this.nombreDeBitsParCouleur == 32)
                        {
                            tab[i, j] = new Pixel(myfile[position], myfile[position + 1], myfile[position + 2], myfile[position + 3]);
                            position = position + 4;
                        }
                        else
                        {
                            tab[i, j] = new Pixel(myfile[position], myfile[position + 1], myfile[position + 2]);
                            position = position + 3;
                        }
                    }
                }

                this.image = tab;

            }
            catch (Exception e)
            {
                Console.WriteLine("Oups, une erreur est survenue.");
                Console.WriteLine(e);
            }
        }

        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int compteur = 0;
            int value = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                value += Convert.ToInt32(tab[i] * (Math.Pow(256, compteur)));
                compteur++;
            }
            return value;
        }

        public byte[] Convertir_Int_To_Endian(int val, int taille)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            if (bytes.Length != taille)
            {
                byte[] tab = new byte[taille];
                for (int i = 0; i < taille; i++)
                {
                    tab[i] = bytes[i];
                }
                return tab;
            }

            return bytes;
        }

        public void From_Image_To_File(string file)
        {
            byte[] bytes = new byte[this.taille];
            byte[] type = { (byte)this.type[0], (byte)this.type[1] };
            byte[] taille = Convertir_Int_To_Endian(this.taille, 4);
            byte[] offset = Convertir_Int_To_Endian(this.offset, 4);
            byte[] largeur = Convertir_Int_To_Endian(this.largeur, 4);
            byte[] hauteur = Convertir_Int_To_Endian(this.hauteur, 4);
            byte[] nombreDeBitsParcouleur = Convertir_Int_To_Endian(this.nombreDeBitsParCouleur, 2);

            for (int i = 18; i < 22; i++)
            {
                bytes[i] = hauteur[i - 18];
            }

            for (int i = 22; i < 26; i++)
            {
                bytes[i] = largeur[i - 22];
            }

            for (int i = 2; i < 6; i++)
            {
                bytes[i] = taille[i - 2];
            }

            for (int i = 10; i < 14; i++)
            {
                bytes[i] = offset[i - 10];
            }

            for (int i = 28; i < 30; i++)
            {
                bytes[i] = nombreDeBitsParcouleur[i - 28];
            }

            for (int i = 0; i < 2; i++)
            {
                bytes[i] = type[i];
            }

            bytes[14] = 40;
            bytes[26] = 1;

            int position = this.offset;

            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    if (this.nombreDeBitsParCouleur == 32)
                    {
                        bytes[position] = (byte)this.image[i, j].r;
                        bytes[position + 1] = (byte)this.image[i, j].g;
                        bytes[position + 2] = (byte)this.image[i, j].b;
                        bytes[position + 3] = (byte)this.image[i, j].a;
                        position = position + 4;
                    }
                    else
                    {
                        bytes[position] = (byte)this.image[i, j].r;
                        bytes[position + 1] = (byte)this.image[i, j].g;
                        bytes[position + 2] = (byte)this.image[i, j].b;

                        position = position + 3;
                    }
                }
            }

            File.WriteAllBytes(file, bytes);
        }

        public void NB()
        {
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    if (this.image[i, j].Moyenne() < 128) //Si le pixel est plus sombre que lumineux alors il devient noir.
                    {
                        this.image[i, j] = new Pixel(0, 0, 0);
                        ;
                    }
                    else
                    {
                        this.image[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
        }

        public void Nuance()
        {
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    /*
                     * Indications d'après: https://fr.wikipedia.org/wiki/Niveau_de_gris
                     * "Si l'écran ou le fichier prend un format d'image en couleurs, 
                     * les trois valeurs (rouge, vert, bleu) sont égales."
                     * On prend donc la moyenne du R,G et B et on l'applique.
                     */

                    int moyenne = this.image[i, j].Moyenne();
                    this.image[i, j] = new Pixel(moyenne, moyenne, moyenne);
                }
            }
        }

        public void Miroir()
        {
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    Pixel p2 = this.image[i, j];
                    this.image[i, j] = this.image[i, this.image.GetLength(1) - j - 1];
                }
            }
        }

        public void SymmetrieY()
        {
            Pixel[,] symmetrie = new Pixel[this.image.GetLength(0), this.image.GetLength(1)];
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    symmetrie[i, j] = this.image[i, this.image.GetLength(1) - j - 1];
                }
            }
            this.image = symmetrie;
        }

        public void SymmetrieX()
        {
            Pixel[,] symmetrie = new Pixel[this.image.GetLength(0), this.image.GetLength(1)];
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    symmetrie[i, j] = this.image[this.image.GetLength(0) - i - 1, j];
                }
            }
            this.image = symmetrie;
        }

        public void SymmetrieXY()
        {
            Pixel[,] symmetrie = new Pixel[this.image.GetLength(0), this.image.GetLength(1)];
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    symmetrie[i, j] = this.image[this.image.GetLength(0) - i - 1, this.image.GetLength(1) - j - 1];
                }
            }
            this.image = symmetrie;
        }

        /// <summary>
        /// Code du partiel. Je l'ai juste recopié j'ai rien testé (comme ça c'est fait). Faudrait juste vérifier
        /// et évidemment faire tous les tests (donc regénérer la hauteur/largeur, le offset, la taille du fichier, etc...)
        /// - Il faudrait donc créer une nouvelle instance de classe qui prend en paramètre la nouvelle image avec
        /// ses nouvelles dimensions. Le type reste le même, pareil pour la profondeur. L'offset devrait rester le même normalement.
        /// Il faudra vérifier ça et puis voilà ! :)
        /// </summary>
        /// <param name="valeur">Facteur d'agrandissement</param>
        public void Agrandir(int valeur)
        {
            Pixel[,] tab = new Pixel[image.GetLength(0) * valeur, image.GetLength(1) * valeur];
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    Pixel[,] mat = new Pixel[valeur, valeur];
                    for (int o = 0; o < mat.GetLength(0); o++)
                    {
                        for (int n = 0; n < mat.GetLength(1); n++)
                        {
                            mat[o, n] = image[i, j];
                        }
                    }
                    for (int o = 0; o < mat.GetLength(0); o++)
                    {
                        for (int n = 0; n < mat.GetLength(1); n++)
                        {
                            tab[o + i * valeur, n + j * valeur] = mat[o, n];
                        }
                    }
                }
            }
        }

        public void Tourner90()
        {
            Pixel[,] tab = new Pixel[image.GetLength(1), image.GetLength(0)];
            int largeur = this.largeur;
            int hauteur = this.hauteur;

            this.largeur = hauteur;
            this.hauteur = largeur;

            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    tab[i, j] = this.image[image.GetLength(0) - 1 - i, this.image.GetLength(1) - 1 - j];
                }
            }

            for (int i = 0; i < this.image.GetLength(1); i++)
            {
                for (int j = 0; j < image.GetLength(0); j++)
                {
                    this.image[i, j] = tab[j, i];
                }
            }

            SymmetrieY();
        }

        public void Tourner180()
        {
            Pixel[,] tab = new Pixel[image.GetLength(0), image.GetLength(1)];

            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    tab[i, j] = this.image[tab.GetLength(0) - i - 1, j];
                }
            }
            this.image = tab;
            SymmetrieY();

        }

        public void Tourner270()
        {
            Pixel[,] tab = new Pixel[image.GetLength(1), image.GetLength(0)];

            int largeur = this.largeur;
            int hauteur = this.hauteur;

            this.largeur = hauteur;
            this.hauteur = largeur;

            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    tab[j, i] = this.image[i, this.image.GetLength(1) - 1 - j];
                }
            }

            this.image = tab;
            SymmetrieXY();
        }

        public int Hauteur
        {
            get { return this.hauteur; }
        }

        public int Largeur
        {
            get { return this.largeur; }
        }

        public Pixel[,] Image
        {
            get { return this.image; }
        }
    }

}

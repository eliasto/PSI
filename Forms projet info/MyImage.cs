using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Diagnostics;

namespace Forms_projet_info
{
    public class MyImage
    {
        private string type; //Type de l'image
        private int taille; //Taille en octet de l'image
        private int offset; //Offset de l'image = 54
        private int largeur; //Largeur de l'image
        private int hauteur; //Hauteur de l'image
        private int nombreDeBitsParCouleur; //Nombre de bits par couleur pour l'image
        private Pixel[,] image; //Image

        /// <summary>
        /// Classe MyImage qui récupère les informations de l'image
        /// </summary>
        /// <param name="path">Chemin de l'image</param>
        public MyImage(string path)
        {
            try
            {
                byte[] myfile = File.ReadAllBytes(path); //Lit les bytes du fichier

                byte[] widthByte = { myfile[18], myfile[19], myfile[20], myfile[21] }; //Largeur de l'image
                int width = Convertir_Endian_To_Int(widthByte);

                byte[] heightByte = { myfile[22], myfile[23], myfile[24], myfile[25] }; //Hauteur de l'image
                int height = Convertir_Endian_To_Int(heightByte);


                byte[] tailleByte = { myfile[2], myfile[3], myfile[4], myfile[5] }; //Taille du fichier
                int taille = Convertir_Endian_To_Int(tailleByte);

                byte[] offsetByte = { myfile[10], myfile[11], myfile[12], myfile[13] }; //Position du offset
                int offset = Convertir_Endian_To_Int(offsetByte);

                byte[] numberOfPixelsByte = { myfile[28], myfile[29] }; //Nombre de bits par couleurs
                int numberOfPixels = Convertir_Endian_To_Int(numberOfPixelsByte);

                //On assigne les valeurs aux propriétés
                this.hauteur = height;
                this.largeur = width;
                this.offset = offset;
                this.taille = taille;
                this.nombreDeBitsParCouleur = numberOfPixels;
                this.type = (char)myfile[0] + "" + (char)myfile[1];

                Pixel[,] tab = new Pixel[this.hauteur, this.largeur]; //Création d'une matrice de pixel vide
                int nombrePourMultiple4 = (4 - (this.largeur * 3) % 4) % 4; //Cadeau par le prof
                int position = this.offset; 
                for (int i = 0; i < tab.GetLength(0); i++)
                {
                    for (int j = 0; j < tab.GetLength(1); j++)
                    {
                        if(this.nombreDeBitsParCouleur == 32) //Image avec alpha
                        {
                            tab[i, j] = new Pixel(myfile[position], myfile[position + 1], myfile[position + 2], myfile[position + 3]); //Lecture d'un pixel, puis on positionne dans la matrice
                            position = position + 4;
                        }
                        else //Image sans alpha
                        {
                            tab[i, j] = new Pixel(myfile[position], myfile[position + 1], myfile[position + 2]);
                            position = position + 3;
                        }
                    }
                    position = position + nombrePourMultiple4;

                }

                this.image = tab;

            }
            catch (Exception e)
            {
                Console.WriteLine("Oups, une erreur est survenue.");
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Génère une image à partir de rien. L'image sera vierge.
        /// </summary>
        /// <param name="largeut">Largeur de l'image en pixel</param>
        /// <param name="hauteur">Hauteur de l'image en pixel</param>
        public MyImage(int largeut, int hauteur)
        {
            try //On génère une image vierge
            {
                //Même chose que pour le premier constructeur, rien de spécial ici
                this.largeur = largeut;
                this.hauteur = hauteur;
                this.type = "BM";
                this.nombreDeBitsParCouleur = 24;
                this.offset = 54;
                int nombrePourMultiple4 = (4 - (this.largeur * 3) % 4) % 4; //Cadeau par le prof
                this.taille = this.offset + (largeut * 3 + nombrePourMultiple4) * hauteur;
                Pixel[,] tab = new Pixel[this.hauteur, this.largeur];
                for (int i = 0; i < this.hauteur; i++)
                {
                    for (int j = 0; j < this.largeur; j++)
                    {
                        tab[i, j] = new Pixel(255, 255, 255);
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


        /// <summary>
        /// Conversion d'un little endian en int
        /// </summary>
        /// <param name="tab">Tableau de byte à convertir</param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int compteur = 0;
            int value = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                value += Convert.ToInt32(tab[i] * (Math.Pow(256, compteur))); //Permet de faire la conversion d'un byte[] en int
                compteur++;
            }
            return value;
        }

        /// <summary>
        /// Conversion d'un entier en un tableau de little endian
        /// </summary>
        /// <param name="val">Valeur à convertir</param>
        /// <param name="taille">Taille du tableau</param>
        /// <returns></returns>
        public byte[] Convertir_Int_To_Endian(int val, int taille)
        {
            byte[] bytes;

            switch (taille)
            {
                case 1:
                    bytes = new byte[1];
                    bytes[0] = (byte)val;
                    return bytes;
                case 2:
                    bytes = new byte[2];
                    bytes[0] = (byte)val;
                    bytes[1] = (byte)(((uint)val >> 8) & 0xFF);
                    return bytes;
                case 3:
                    bytes = new byte[3];
                    bytes[0] = (byte)val;
                    bytes[1] = (byte)(((uint)val >> 8) & 0xFF);
                    bytes[2] = (byte)(((uint)val >> 16) & 0xFF);
                    return bytes;
                case 4:
                    bytes = new byte[4];
                    bytes[0] = (byte)val;
                    bytes[1] = (byte)(((uint)val >> 8) & 0xFF);
                    bytes[2] = (byte)(((uint)val >> 16) & 0xFF);
                    bytes[3] = (byte)(((uint)val >> 24) & 0xFF);
                    return bytes;
                default:
                    bytes = new byte[4];
                    bytes[0] = (byte)val;
                    bytes[1] = (byte)(((uint)val >> 8) & 0xFF);
                    bytes[2] = (byte)(((uint)val >> 16) & 0xFF);
                    bytes[3] = (byte)(((uint)val >> 24) & 0xFF);
                    return bytes;
            }

        }

        /// <summary>
        /// Enregistre l'image dans un fichier
        /// </summary>
        /// <param name="file">Chemin du fichier</param>
        public void From_Image_To_File(string file)
        {
            //On récupère les valeurs (int en tableau de byte)
            byte[] bytes = new byte[this.taille]; //Chaîne de byte qui contient le fichier
            byte[] type = { (byte)this.type[0], (byte)this.type[1] };
            byte[] taille = Convertir_Int_To_Endian(this.taille, 4);
            byte[] offset = Convertir_Int_To_Endian(this.offset, 4);
            byte[] largeur = Convertir_Int_To_Endian(this.largeur, 4);
            byte[] hauteur = Convertir_Int_To_Endian(this.hauteur, 4);
            byte[] nombreDeBitsParcouleur = Convertir_Int_To_Endian(this.nombreDeBitsParCouleur, 2);

            //On assigne les valeurs
            for (int i = 18; i < 22; i++)
            {
                bytes[i] = largeur[i - 18];
            }

            for (int i = 22; i < 26; i++)
            {
                bytes[i] = hauteur[i - 22];
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

            bytes[14] = 40; //éléments importants pour la compression, etc...
            bytes[26] = 1;

            int position = this.offset;
            int nombrePourMultiple4 = (4 - (this.largeur * 3) % 4) % 4; //Cadeau par le prof

            for (int i = 0; i < this.image.GetLength(0); i++) //On assigne les valeurs de l'image
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {

                    if (this.nombreDeBitsParCouleur == 32) //Image avec alpha
                    {
                        bytes[position] = (byte)this.image[i, j].r;
                        bytes[position + 1] = (byte)this.image[i, j].g;
                        bytes[position + 2] = (byte)this.image[i, j].b;
                        bytes[position + 3] = (byte)this.image[i, j].a;


                        position = position + 4;
                    }
                    else //Image sans alpha
                    {
                        bytes[position] = (byte)this.image[i, j].r;
                        bytes[position + 1] = (byte)this.image[i, j].g;
                        bytes[position + 2] = (byte)this.image[i, j].b;

                        position = position + 3;
                    }
                }
                position = nombrePourMultiple4 + position;

            }

            File.WriteAllBytes(file, bytes); //On écrit le fichier
        }

        /// <summary>
        /// Permet de retourner la suite de byte d'une image
        /// </summary>
        /// <returns>Retourne un tableau de byte d'une image Bitmap</returns>
        public byte[] From_Image_To_Array() //Même chose qu'eau dessus sauf qu'on retourne le tableau de byte
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
                bytes[i] = largeur[i - 18];
            }

            for (int i = 22; i < 26; i++)
            {
                bytes[i] = hauteur[i - 22];
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
            int nombrePourMultiple4 = (4 - (this.largeur * 3) % 4) % 4; //Cadeau par le prof

            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {

                    if(this.nombreDeBitsParCouleur == 32)
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
                position = nombrePourMultiple4 + position;

            }

            return bytes;
        }


        /// <summary>
        /// Génère un histogramme d'une image en fonction des composantes couleurs RGB
        /// </summary>
        public MyImage Histogramme(int couleur)
        {
            int max = 0;

            //Créer la matrice de l'histogramme et le remplis de la couleur que l'on veut avec se densité allant de 0 à 255
            int[,] matrice = new int[this.hauteur, this.largeur];
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    if (couleur == 1)
                    {
                        matrice[i, j] = image[i, j].r;
                    }
                    if (couleur == 2)
                    {
                        matrice[i, j] = image[i, j].g;
                    }
                    if (couleur == 3)
                    {
                        matrice[i, j] = image[i, j].b;
                    }
                }
            }
            int compteur = 0; //Pour connaître le nombre d'itération 

            //Cherche le mew d'itération (entre 0 et 255)
            for (int k = 0; k < 256; k++)
            {
                compteur = 0;
                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        if (matrice[i, j] == k)
                        {
                            compteur++;
                        }
                    }
                    if (max <= compteur)
                    {
                        max = compteur;
                    }

                }
            }
            compteur = 0;
            Pixel[,] histo = new Pixel[max, 256]; //Crée l'image en couleur de l'histogramme

            for (int i = 0; i < max; i++) // Initialisation de l'image en noir
            {
                for (int j = 0; j < 256; j++)
                {
                    histo[i, j] = new Pixel(0, 0, 0);
                }
            }
            for (int k = 0; k < 256; k++)
            {
                compteur = 0;
                for (int i = 0; i < hauteur; i++) //Nombre d'itération de chaque densité de la couleur
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        if (matrice[i, j] == k)
                        {
                            compteur++;
                        }
                    }
                }
                for (int i = 0; i < compteur; i++)  //Remplis l'image de l'histogramme par le nombre d'itération de la densité de la couleur
                {
                    if (couleur == 1)
                    {
                        histo[i, k] = new Pixel(255, 0, 0); //En rouge
                    }
                    if (couleur == 2)
                    {
                        histo[i, k] = new Pixel(0, 255, 0); //En vert
                    }
                    if (couleur == 3)
                    {
                        histo[i, k] = new Pixel(0, 0, 255); //En bleu
                    }
                }
            }


            MyImage histogramme = new MyImage(256, max);
            histogramme.image = histo;


            return histogramme;
        }
        /// <summary>
        /// Transforme l'image en soit des pixels noir ou blanc
        /// </summary>
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


        /// <summary>
        /// Une magnifique fonction pour générer un QrCode de version 1 ou version 2.
        /// L'algorithme pourrait générer en théorie les 40 versions, mais il faudrait :
        /// - Les coordonnées pour placer les alignments patterns (voir comment les calculer)
        /// - Rajouter les informations supplémentaires sur les 2 rectangles quand on est > à la version x
        /// - Améliorer le système de calcul des caractères alphanumériques
        /// https://stackoverflow.com/questions/13238704/calculating-the-position-of-qr-code-alignment-patterns
        /// 
        /// Il faudrait aussi pouvoir rajouter plusieurs correcteurs car pour l'instant on ne supporte que le correcteur L
        /// avec un mask de niveau 0.
        /// </summary>
        /// <param name="texte">Texte à écrire</param>
        /// <param name="coeff">Coefficient pour la taille du QRCode</param>
        public static MyImage QRCode(string texte, int coeff, Color colorWheel)
        {
            int version = 1; //Version initiale du QRCode
            if (texte.Length > 24) version = 2; //On passe à la version 2
            MyImage qrcode = new MyImage((17 + version * 4) * coeff, (17 + version * 4) * coeff); //Générer un canva vierge pour contenir le QRCode

            int[,] tab = new int[17 + version * 4, 17 + version * 4]; //Mega matrice qui contient plein de chiffre
            /*
             * - Chiffre 1 ou 2 pour mettre pixels noir et blanc qui CODENT le message
             * Autres chiffres permettant de faire du débogage et ces chiffres seront les pixels qui servent
             * à la détection du QRCode, donner le type de maskage utilisé, etc...
             */

            for (int i = 0; i < 9; i++) //Permet de "sécuriser" une zone, pour pas que l'algorithme de placement y touche
            {
                tab[8, i] = 9;
                tab[i, 8] = 9;
            }

            //Carré de détection en haut à gauche
            for (int i = 0; i < 7; i++)
            {
                tab[i, 0] = 1;
                tab[0, i] = 1;
                tab[6, i] = 1;
                tab[i, 6] = 1;

                tab[i, 7] = 2;
                tab[7, i] = 2;
            }
            tab[7, 7] = 2;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tab[i + 2, j + 2] = 1;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                tab[i + 1, 1] = 2;
                tab[i + 1, 5] = 2;
                tab[1, i + 1] = 2;
                tab[5, i + 1] = 2;

                tab[1, tab.GetLength(0) - 6 + i] = 2;
                tab[5, tab.GetLength(0) - 6 + i] = 2;
                tab[i + 1, tab.GetLength(0) - 6] = 2;
                tab[i + 1, tab.GetLength(0) - 2] = 2;

                tab[tab.GetLength(0) - 6, i + 1] = 2;
                tab[tab.GetLength(0) - 2, i + 1] = 2;
                tab[i + tab.GetLength(0) - 6, 1] = 2;
                tab[i + tab.GetLength(0) - 6, 5] = 2;

            }

            //En haut à droite
            for (int i = 0; i < 7; i++)
            {
                tab[6, tab.GetLength(0) - 7 + i] = 1;
                tab[0, tab.GetLength(0) - 7 + i] = 1;
                tab[i, tab.GetLength(0) - 7] = 1;
                tab[i, tab.GetLength(0) - 1] = 1;

                tab[i, tab.GetLength(0) - 8] = 2;
                tab[7, tab.GetLength(0) - 7 + i] = 2;
            }
            tab[7, tab.GetLength(0) - 8] = 2;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tab[2 + i, tab.GetLength(0) - 5 + j] = 1;
                }
            }

            //En bas à gauche
            for (int i = 0; i < 7; i++)
            {
                tab[tab.GetLength(0) - 1 - 7, i] = 2;
                tab[i + tab.GetLength(0) - 1 - 6, 7] = 2;

                tab[tab.GetLength(0) - 1 - 6, i] = 1;
                tab[tab.GetLength(0) - 1, i] = 1;
                tab[i + tab.GetLength(0) - 1 - 6, 0] = 1;
                tab[i + tab.GetLength(0) - 1 - 6, 6] = 1;
            }
            tab[tab.GetLength(0) - 1 - 7, 7] = 2;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tab[tab.GetLength(0) - 5 + i, 2 + j] = 1;
                }
            }


            int compteurDelimitation = 0;
            //Permet de poser les alignements patterns qui alternent entre noir et blanc
            
            while (tab.GetLength(0) - 1 - 8 - compteurDelimitation > 7)
            {
                if (compteurDelimitation % 2 == 0)
                {
                    tab[tab.GetLength(0) - 1 - 8 - compteurDelimitation, 6] = 1;
                    tab[6, tab.GetLength(0) - 1 - 8 - compteurDelimitation] = 1;

                }
                else
                {
                    tab[tab.GetLength(0) - 1 - 8 - compteurDelimitation, 6] = 2;
                    tab[6, tab.GetLength(0) - 1 - 8 - compteurDelimitation] = 2;
                }
                compteurDelimitation++;
            }

            int[,] alignCoords = { { 6, 6 }, { 6, 18 }, { 18, 6 }, { 18, 18 } }; //For version 2 only. Pattern d'alignement


            for (int i = 0; i < alignCoords.GetLength(0); i++) //Permet de poser l'autre carré "alignement pattern" pour les versions 2 est +
            {
                //Vérifie si on peut poser l'alignement pattern et pas que ça empiète sur des trucs du QRCode
                if (AlignmentPatternGenerator(alignCoords[i, 0], alignCoords[i, 1], tab) && version > 1)
                {
                    tab[alignCoords[i, 0], alignCoords[i, 1]] = 1;
                    for (int k = 0; k < 3; k++)
                    {
                        tab[alignCoords[i, 0] - 1 + k, alignCoords[i, 1] - 1] = 2;
                        tab[alignCoords[i, 0] - 1 + k, alignCoords[i, 1] + 1] = 2;
                        tab[alignCoords[i, 0] - 1, alignCoords[i, 1] - 1 + k] = 2;
                        tab[alignCoords[i, 0] + 1, alignCoords[i, 1] - 1 + k] = 2;
                    }
                    for (int k = 0; k < 5; k++)
                    {
                        tab[alignCoords[i, 0] - 2 + k, alignCoords[i, 1] - 2] = 1;
                        tab[alignCoords[i, 0] - 2 + k, alignCoords[i, 1] + 2] = 1;
                        tab[alignCoords[i, 0] - 2, alignCoords[i, 1] - 2 + k] = 1;
                        tab[alignCoords[i, 0] + 2, alignCoords[i, 1] - 2 + k] = 1;
                    }
                }
            }

            //01000: Correction L mask 0
            int index = 0;
            int secondIndex = 7;
            int[] correction = { 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0 }; //Correction pour L-0


            //Pose les modules qui permettent de dire quel type de correction on utilise
            for (int i = 0; i < 9; i++)
            {
                if (tab[8, i] == 9)
                {
                    if (correction[index] == 1) tab[8, i] = 1;
                    else tab[8, i] = 2;
                    index++;
                }

                if (tab[8 - i, 8] == 9)
                {
                    if (correction[secondIndex] == 1) tab[8 - i, 8] = 1;
                    else tab[8 - i, 8] = 2;
                    secondIndex++;
                }

            }

            //^même chose qu'au dessus
            for (int i = 0; i < 7; i++)
            {
                if (correction[i] == 1) tab[tab.GetLength(0) - 1 - i, 8] = 1;
                else tab[tab.GetLength(0) - 1 - i, 8] = 2;
            }

            for (int i = 0; i < 8; i++)
            {
                if (correction[i + 7] == 1) tab[8, tab.GetLength(0) - 1 - 7 + i] = 1;
                else tab[8, tab.GetLength(0) - 1 - 7 + i] = 2;
            }





            //Pose un carré noir qui est obligé d'être posé
            tab[(4 * version) + 9, 8] = 1; //Carré noir

            //Et là petite boucle qui permet de poser nos modules des différents éléments d'au-dessus
            //ça les ECRIT sur l'image
            for (int i = 0; i < tab.GetLength(0); i++) //1 noir, 2 ou 0 blanc, 9 vert
            {
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    if (tab[i, j] == 1) setModule(i, j, 17 + version * 4);
                    //if (tab[i, j] == 2) setModule(i, j, 17+version * 4,8);
                    //if (tab[i, j] == 9) setModule(i, j, 17+version * 4, 9);
                    //if (tab[i, j] == 7) setModule(i, j, 17+version * 4, 7);

                }
            }



            string longueurBitsTexte = Convert.ToString(texte.Length, 2); //Longueur du texte en bits


            if (longueurBitsTexte.Length < 9) //C'est comme la fonction .PadLeft sauf que je ne la connaissais pas encore lol
            {
                int longueur = 9 - longueurBitsTexte.Length;
                for (int i = 0; i < longueur; i++)
                {
                    longueurBitsTexte = "0" + longueurBitsTexte;
                }
            }

            List<string> texteEnList = new List<string>(); //Texte mais découpé par morceaux de deux lettres par deux lettrees
            List<string> listeDeBinaire = new List<string>(); //Suite des lettres codés en binaire

            //On découpe le message par deux lettres par deux
            for (int i = 0; i < texte.Length - 1; i++) 
            {
                texteEnList.Add(texte[i] + "" + texte[(i + 1)]);
                i++;
            }
            if (texte.Length % 2 != 0)
            {
                texteEnList.Add("" + texte[texte.Length - 1]);
            }

            //Récupère le code alphanumérique de la lettre puis applique le calcul pour obtenir le code en binaire des lettres
            for (int i = 0; i < texteEnList.Count; i++)
            {
                int somme = 0;
                string c = texteEnList[i];
                string lesBitsFinalement;
                for (int a = 0; a < c.Length; a++)
                {
                    int value;
                    if (c.Length > 1)
                    {
                        if (a == 0)
                        {
                            if (c[a] == ' ')
                            {
                                value = 45 * 36;
                            }
                            else if (int.TryParse("" + c[a], out int o)) value = 45 * Convert.ToInt32("" + c[a]); //Permet de convertir une lettre en code alphanumérique
                            else if (c[a] == '$') value = 45 * 37;
                            else if (c[a] == '%') value = 45 * 38;
                            else if (c[a] == '*') value = 45 * 39;
                            else if (c[a] == '+') value = 45 * 40;
                            else if (c[a] == '-') value = 45 * 41;
                            else if (c[a] == '.') value = 45 * 42;
                            else if (c[a] == '/') value = 45 * 43;
                            else if (c[a] == ':') value = 45 * 44;

                            else
                            {
                                value = 45 * (9 + AlphabetPosition(c[a]));
                            }

                        }
                        else //Même chose qu'en haut
                        {
                            if (c[a] == ' ')
                            {
                                value = 36;
                            }
                            else if (int.TryParse("" + c[a], out int o)) value = Convert.ToInt32("" + c[a]);
                            else if (c[a] == '$') value = 37;
                            else if (c[a] == '%') value = 38;
                            else if (c[a] == '*') value = 39;
                            else if (c[a] == '+') value = 40;
                            else if (c[a] == '-') value = 41;
                            else if (c[a] == '.') value = 42;
                            else if (c[a] == '/') value = 43;
                            else if (c[a] == ':') value = 44;
                            else
                            {
                                value = 9 + AlphabetPosition(c[a]);
                            }
                        }
                    }

                    else
                    {
                        if (c[a] == ' ')
                        {
                            value = 36;
                        }
                        else if (int.TryParse("" + c[a], out int o)) value = Convert.ToInt32("" + c[a]);
                        else if (c[a] == '$') value = 37;
                        else if (c[a] == '%') value = 38;
                        else if (c[a] == '*') value = 39;
                        else if (c[a] == '+') value = 40;
                        else if (c[a] == '-') value = 41;
                        else if (c[a] == '.') value = 42;
                        else if (c[a] == '/') value = 43;
                        else if (c[a] == ':') value = 44;
                        else
                        {
                            value = 9 + AlphabetPosition(c[a]);
                        }
                    }
                    somme += value;
                }
                lesBitsFinalement = Convert.ToString(somme, 2); //Convertit en binaire le résultat
                if (c.Length > 1) lesBitsFinalement = lesBitsFinalement.PadLeft(11, '0'); //On applique un padLeft si c'est une combinaison de deux lettres
                else lesBitsFinalement = lesBitsFinalement.PadLeft(6, '0'); //Sinon on applique ça si c'est seulement une lettre

                listeDeBinaire.Add(lesBitsFinalement);
            }

            string bitPresqueTermine = "";
            bitPresqueTermine = "0010"; //On code le mode indicator
            bitPresqueTermine = bitPresqueTermine + longueurBitsTexte; //on mets la longueur du message en bits
            foreach (string mot in listeDeBinaire)
            {
                bitPresqueTermine = bitPresqueTermine + mot; //On met le message en bits
            }

            int longueurFinal = version == 1 ? 152 : 272; //152 pour v1 272 pour v2

            //ça fait la même chose que ce soit le if ou le else mais j'ai peur d'enlever et que ça casse tout donc on garde ça comme ça
            if (bitPresqueTermine.Length < longueurFinal - 4) //On rajoute les 0 si on peut
            {
                bitPresqueTermine = bitPresqueTermine + "0000";
            }
            else if (bitPresqueTermine.Length < longueurFinal) //Sinon on complète avec des 0
            {
                bitPresqueTermine.PadLeft(longueurFinal, '0');
            }

            while (bitPresqueTermine.Length % 8 != 0) //Si la longueur total de notre message est pas modulo 8 on rajoute un 0 jusqu'à ce que le soit
            {
                bitPresqueTermine = bitPresqueTermine + "0";
            }


            //On complète notre message par ces codes #MerciThonky.com
            int nombreDePavesDoctets = (longueurFinal - bitPresqueTermine.Length) / 8;
            string pave1 = "11101100";
            string pave2 = "00010001";

            for (int i = 0; i < nombreDePavesDoctets; i++)
            {
                if (i % 2 == 0) //Permet d'alténer à chaque fois entre chaque pave1 ou pave2
                {
                    bitPresqueTermine = bitPresqueTermine + pave1;
                }
                else
                {
                    bitPresqueTermine = bitPresqueTermine + pave2;
                }
            }

            byte[] tabDeByte = new byte[bitPresqueTermine.Length / 8]; //Permet de convertir notre chaîne de byte en un tableau
            for (int i = 0; i < tabDeByte.Length; ++i)
            {
                tabDeByte[i] = Convert.ToByte(bitPresqueTermine.Substring(8 * i, 8), 2); //On assigne
            }
            byte[] result = ReedSolomon.ReedSolomonAlgorithm.Encode(tabDeByte, version == 1 ? 7 : 10, ReedSolomon.ErrorCorrectionCodeType.QRCode); //On utilise l'algo de correction de ReedSolomon

            foreach (byte bit in result)
            {
                bitPresqueTermine = bitPresqueTermine + Convert.ToString(bit, 2).PadLeft(8, '0'); //Et pour terminé on rajoute les 0 pour que ça fasse une chaîne de 8 octets sur la correction de ReedSolomon
            }

            //Ma fierté, ce petit algo
            bool monte = true; //Est-ce que le serpent monte ?
            bool estADroite = true; //Serpent à droite ou à gauche ?

            int x = tab.GetLength(0) - 1; //Position en x
            int y = tab.GetLength(0) - 1; //Position en y

            for (int k = 0; k < bitPresqueTermine.Length; k++) //On parcours nos bits qu'on doit écrire
            {
                if (y == 6) y--; //Enft à cette position on pourra jamais poser, donc on "skip" ce passage pour éviter que le serpent soit
                                 //cassé (y'a le timing pattern vertical ici).
                //Eh oui si on réfléchit bien, vu que la côté d'un carré est impair, et que le serpent pose deux par deux, et bien on
                //doit obligatoirement à un moment sauté un passage : et ce passage il est là, c'est ici où on a un des "timing pattern"
                if (x < 0) //Si le serpent est en bas
                {
                    monte = !monte; //Il remonte
                    x = 0; //On le re-positionne
                    y = y - 2; //Il se décale de deux cases
                }
                if (x > tab.GetLength(0) - 1) //S'il est en haut
                {
                    monte = !monte; //Il redescend
                    x = tab.GetLength(0) - 1; //On le re-positionne
                    y = y - 2; //Il se décale de deux cases
                }

                if (tab[x, y] == 0) //Si la case est libre
                {
                    if (bitPresqueTermine[k] == '1') //Si le bit actuel est 1
                    {
                        tab[x, y] = 10; //Bah on dit "ok", et on dit qu'on a posé un pixel noir ici
                        setModule(x, y, 17 + version * 4); //On ECRIT le pixel noir sur l'image
                    }
                    else tab[x, y] = 20; //Sinon bah on pose un pixel blanc, mais on écrit rien vu que l'image est blanche de base
                }
                else k--; //Si case pas dispo, on recule de 1 dans notre boucle for
                
                //Ici on fait déplacer notre serpent
                if (monte) //Si ça monte
                {
                    if (estADroite) //Si c'est à droite
                    {
                        y--; //hop à gauche
                        estADroite = false; //Bah il est plus à droite
                    }
                    else //Sinon
                    {
                        y++; //Hop à droite
                        x--; //Il monte
                        estADroite = true; //Il est à droite (logique)
                    }
                }
                else //Même chose sauf que là il descend
                {
                    if (estADroite)
                    {
                        y--;
                        estADroite = false;
                    }
                    else
                    {
                        y++;
                        x++;
                        estADroite = true;
                    }
                }
            }

            //On applique le Mask Pattern de niveau 0
            //If the formula below is true for a given row/column coordinate, switch the bit at that coordinate
            //(row + column) mod 2 == 0

            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(0); j++)
                {
                    if (tab[i, j] == 10 || tab[i, j] == 20)
                    {
                        if ((i + j) % 2 == 0)
                        {
                            if (tab[i, j] == 20)
                            {
                                tab[i, j] = 20;
                                setModule(i, j, 17 + version * 4);
                            }
                            else
                            {
                                tab[i, j] = 10;
                                setModule(i, j, 17 + version * 4, 15);
                            }
                        }
                    }

                }
            }

            return qrcode; //On retourne le QRCode
            
            //Permet de place un module sur le QRCode
            void setModule(int coordX, int coordY, int taille, int color = 0)
            {
                int module = qrcode.largeur / taille;
                for (int i = 0; i < module; i++)
                {
                    for (int j = 0; j < module; j++)
                    {
                        ///Les différentes couleurs sont juste pour le debug
                        if (color == 0) qrcode.image[qrcode.hauteur - 1 - (i + coordX * module), j + coordY * module] = new Pixel(colorWheel.B, colorWheel.G, colorWheel.R);
                        if (color == 9) qrcode.image[qrcode.hauteur - 1 - (i + coordX * module), j + coordY * module] = new Pixel(0, 0, 255);
                        if (color == 8) qrcode.image[qrcode.hauteur - 1 - (i + coordX * module), j + coordY * module] = new Pixel(0, 255, 0);
                        if (color == 7) qrcode.image[qrcode.hauteur - 1 - (i + coordX * module), j + coordY * module] = new Pixel(255, 0, 0);
                        if (color == 15) qrcode.image[qrcode.hauteur - 1 - (i + coordX * module), j + coordY * module] = new Pixel(255, 255, 255);

                    }
                }
            }

            //Position de la lettre en code alphanumérique
            int AlphabetPosition(char c)
            {
                return char.ToUpper(c) - 64;//index == 1
            }

            //Fonction qui dit si l'espace est libre pour l'alignement pattern
            bool AlignmentPatternGenerator(int coordX, int coordY, int[,] tableau)
            {
                bool isAvailable = true;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (tableau[coordX - 2 + i, coordY - 2 + i] != 0)
                        {
                            isAvailable = false;
                            i = 6;
                            j = 6;
                        }

                    }
                }
                return isAvailable;

            }


        }

        /// <summary>
        /// Petite fonction pour donner les infos sur le QRCode et pourquoi pas lire ce qu'il contient si je réussis
        /// </summary>
        /// <returns></returns>
        public List<string> LecteurQrCode()
        {
            List<string> infos = new List<string>();
            if(this.Hauteur == this.Largeur)
            {
                if(this.Hauteur%21 == 0)
                {
                    infos.Add("QRCode version 1");

                    this.Rétrécissement(this.Hauteur / 21); //On réduit pour obtenir un carré de 21*21
                    int[,] tab = new int[21,21];

                    for(int i = 0; i < tab.GetLength(0); i++)
                    {
                        for(int j = 0; j < tab.GetLength(1); j++)
                        {
                            tab[i, j] = (this.image[i, j].Moyenne() > 122) ? 7 :8; //Vu que le QRCode peut avoir plusieurs couleurs, ici on mets un 10 si c'est blanc et 20 sinon 
                        }
                    }


                    for (int i = 0; i < 9; i++) //Permet de "sécuriser" une zone, pour pas que l'algorithme de placement y touche
                    {
                        tab[8, i] = 9;
                        tab[i, 8] = 9;
                    }

                    //Carré de détection en haut à gauche
                    for (int i = 0; i < 7; i++)
                    {
                        tab[i, 0] = 1;
                        tab[0, i] = 1;
                        tab[6, i] = 1;
                        tab[i, 6] = 1;

                        tab[i, 7] = 2;
                        tab[7, i] = 2;
                    }
                    tab[7, 7] = 2;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            tab[i + 2, j + 2] = 1;
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        tab[i + 1, 1] = 2;
                        tab[i + 1, 5] = 2;
                        tab[1, i + 1] = 2;
                        tab[5, i + 1] = 2;

                        tab[1, tab.GetLength(0) - 6 + i] = 2;
                        tab[5, tab.GetLength(0) - 6 + i] = 2;
                        tab[i + 1, tab.GetLength(0) - 6] = 2;
                        tab[i + 1, tab.GetLength(0) - 2] = 2;

                        tab[tab.GetLength(0) - 6, i + 1] = 2;
                        tab[tab.GetLength(0) - 2, i + 1] = 2;
                        tab[i + tab.GetLength(0) - 6, 1] = 2;
                        tab[i + tab.GetLength(0) - 6, 5] = 2;

                    }

                    //En haut à droite
                    for (int i = 0; i < 7; i++)
                    {
                        tab[6, tab.GetLength(0) - 7 + i] = 1;
                        tab[0, tab.GetLength(0) - 7 + i] = 1;
                        tab[i, tab.GetLength(0) - 7] = 1;
                        tab[i, tab.GetLength(0) - 1] = 1;

                        tab[i, tab.GetLength(0) - 8] = 2;
                        tab[7, tab.GetLength(0) - 7 + i] = 2;
                    }
                    tab[7, tab.GetLength(0) - 8] = 2;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            tab[2 + i, tab.GetLength(0) - 5 + j] = 1;
                        }
                    }

                    //En bas à gauche
                    for (int i = 0; i < 7; i++)
                    {
                        tab[tab.GetLength(0) - 1 - 7, i] = 2;
                        tab[i + tab.GetLength(0) - 1 - 6, 7] = 2;

                        tab[tab.GetLength(0) - 1 - 6, i] = 1;
                        tab[tab.GetLength(0) - 1, i] = 1;
                        tab[i + tab.GetLength(0) - 1 - 6, 0] = 1;
                        tab[i + tab.GetLength(0) - 1 - 6, 6] = 1;
                    }
                    tab[tab.GetLength(0) - 1 - 7, 7] = 2;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            tab[tab.GetLength(0) - 5 + i, 2 + j] = 1;
                        }
                    }


                    int compteurDelimitation = 0;
                    //Permet de poser les alignements patterns qui alternent entre noir et blanc

                    while (tab.GetLength(0) - 1 - 8 - compteurDelimitation > 7)
                    {
                        if (compteurDelimitation % 2 == 0)
                        {
                            tab[tab.GetLength(0) - 1 - 8 - compteurDelimitation, 6] = 1;
                            tab[6, tab.GetLength(0) - 1 - 8 - compteurDelimitation] = 1;

                        }
                        else
                        {
                            tab[tab.GetLength(0) - 1 - 8 - compteurDelimitation, 6] = 2;
                            tab[6, tab.GetLength(0) - 1 - 8 - compteurDelimitation] = 2;
                        }
                        compteurDelimitation++;
                    }

                    //01000: Correction L mask 0
                    int index = 0;
                    int secondIndex = 7;
                    int[] correction = { 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0 }; //Correction pour L-0


                    //Pose les modules qui permettent de dire quel type de correction on utilise
                    for (int i = 0; i < 9; i++)
                    {
                        if (tab[8, i] == 9)
                        {
                            if (correction[index] == 1) tab[8, i] = 1;
                            else tab[8, i] = 2;
                            index++;
                        }

                        if (tab[8 - i, 8] == 9)
                        {
                            if (correction[secondIndex] == 1) tab[8 - i, 8] = 1;
                            else tab[8 - i, 8] = 2;
                            secondIndex++;
                        }

                    }

                    //^même chose qu'au dessus
                    for (int i = 0; i < 7; i++)
                    {
                        if (correction[i] == 1) tab[tab.GetLength(0) - 1 - i, 8] = 1;
                        else tab[tab.GetLength(0) - 1 - i, 8] = 2;
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        if (correction[i + 7] == 1) tab[8, tab.GetLength(0) - 1 - 7 + i] = 1;
                        else tab[8, tab.GetLength(0) - 1 - 7 + i] = 2;
                    }





                    //Pose un carré noir qui est obligé d'être posé
                    tab[(4 * 1) + 9, 8] = 1; //Carré noir

                    string cadre = "";
                    for(int i = 0; i < tab.GetLength(0); i++)
                    {
                        for (int j = 0; j < tab.GetLength(1); j++)
                        {
                            cadre += tab[i,j] == 7?"0":tab[i,j] == 8?"1":"*";
                        }
                        cadre += "\n";
                    }
                    infos.Add(cadre);

                    //On applique le Mask Pattern de niveau 0
                    //If the formula below is true for a given row/column coordinate, switch the bit at that coordinate
                    //(row + column) mod 2 == 0

                    //for (int i = 0; i < tab.GetLength(0); i++)
                    //{
                    //    for (int j = 0; j < tab.GetLength(0); j++)
                    //    {
                    //            if ((i + j) % 2 == 0)
                    //            {
                    //                if (tab[i, j] == 20) tab[i, j] = 10;
                    //                if (tab[i,j] == 10) tab[i, j] = 20;
                    //            }
                    //        }                        
                    //}

                    //Ma fierté, ce petit algo
                    bool monte = true; //Est-ce que le serpent monte ?
                    bool estADroite = true; //Serpent à droite ou à gauche ?
                    string result = "";

                    int x = tab.GetLength(0) - 1; //Position en x
                    int y = tab.GetLength(0) - 1; //Position en y

                    for (int k = 0; k < 208; k++) //On parcours nos bits qu'on doit écrire
                    {
                        if (y == 6) y--; //Enft à cette position on pourra jamais poser, donc on "skip" ce passage pour éviter que le serpent soit cassé (y'a le timing pattern vertical ici).
                                         //Eh oui si on réfléchit bien, vu que la côté d'un carré est impair, et que le serpent pose deux par deux, et bien on
                                         //doit obligatoirement à un moment sauté un passage : et ce passage il est là, c'est ici où on a un des "timing pattern"
                        if (x < 0) //Si le serpent est en bas
                        {
                            monte = !monte; //Il remonte
                            x = 0; //On le re-positionne
                            y = y - 2; //Il se décale de deux cases
                        }
                        if (x > tab.GetLength(0) - 1) //S'il est en haut
                        {
                            monte = !monte; //Il redescend
                            x = tab.GetLength(0) - 1; //On le re-positionne
                            y = y - 2; //Il se décale de deux cases
                        }

                        if (tab[x, y] == 7 || tab[x,y] == 8) //Si la case est libre
                        {
                            result += (tab[x, y] == 7) ? "0" : "1";
                        }
                        else k--; //Si case pas dispo, on recule de 1 dans notre boucle for

                        //Ici on fait déplacer notre serpent
                        if (monte) //Si ça monte
                        {
                            if (estADroite) //Si c'est à droite
                            {
                                y--; //hop à gauche
                                estADroite = false; //Bah il est plus à droite
                            }
                            else //Sinon
                            {
                                y++; //Hop à droite
                                x--; //Il monte
                                estADroite = true; //Il est à droite (logique)
                            }
                        }
                        else //Même chose sauf que là il descend
                        {
                            if (estADroite)
                            {
                                y--;
                                estADroite = false;
                            }
                            else
                            {
                                y++;
                                x++;
                                estADroite = true;
                            }
                        }
                    }

                    infos.Add("Message : " + result);


                }
                else if(this.Hauteur%25 == 0)
                {
                    infos.Add("QRCode version 2");
                }
                else
                {
                    infos.Add("Cette version de QRCode est supérieur à la version 2. Veuillez fournir un QRCode en version 1 ou 2.");
                }
            }
            else
            {
                infos.Add("Erreur : Ce n'est pas un QRCode.");
            }

            return infos;
        }

        /// <summary>
        /// Permet de générer un QRCode avec une image au centre de celui-ci
        /// </summary>
        /// <param name="QRCode">QRCode généré</param>
        /// <param name="logo">Image à mettre au centre</param>
        /// <returns></returns>
        public static MyImage ImageDansQrCode(MyImage QRCode, MyImage logo)
        {
            int hauteur = QRCode.Hauteur;
            int hauteurFinal = hauteur / 6;
            int largeurFinal = QRCode.Largeur / 6;

            if(logo.Hauteur / hauteurFinal > 1 || logo.Largeur / largeurFinal > 1)
            {
                if (largeurFinal > hauteurFinal) logo.Rétrécissement(logo.Hauteur / hauteurFinal);
                else logo.Rétrécissement(logo.Largeur / largeurFinal);                
            } else
            {
                if (largeurFinal > hauteurFinal) logo.Agrandir(hauteurFinal/logo.Hauteur);
                else logo.Agrandir(largeurFinal/logo.Largeur);
            }
            
           


            int hauteurLogo = logo.hauteur/2;
            int largeurLogo = logo.largeur / 2;

            for (int i = 0; i < logo.hauteur; i++)
            {
                for(int j = 0; j < logo.largeur; j++)
                {
                    QRCode.image[i+QRCode.hauteur/2- hauteurLogo, j+ QRCode.hauteur / 2 - largeurLogo] = logo.image[i, j];
                }
            }

            return QRCode;
        }
        
        /// <summary>
        /// Génère l'image dans des couleurs de nuances de gris (R=G=B)
        /// </summary>
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

        /// <summary>
        /// L'image devient en mirori
        /// </summary>
        public void Miroir()
        {
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    this.image[i, j] = this.image[i, this.image.GetLength(1) - j - 1];
                }
            }
        }

        /// <summary>
        /// Symmétrie sur l'axe des Y
        /// </summary>
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

        /// <summary>
        /// Symmétrie sur l'axe des X
        /// </summary>
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

        /// <summary>
        /// Symmétrie sur l'axe des X et Y
        /// </summary>
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
            this.largeur = this.largeur * valeur;
            this.hauteur = this.hauteur * valeur;

            //Faire vérifier la taille au prof
            if (this.nombreDeBitsParCouleur == 32)
            {
                this.taille = this.largeur * this.hauteur * 4 + this.offset;
            }
            else
            {
                this.taille = this.largeur * this.hauteur * 3 + this.offset;
            }

            Pixel[,] tab = new Pixel[image.GetLength(0) * valeur, image.GetLength(1) * valeur]; //nouvelle matrice de pixel avec la taille agrandit
            //la boucle permet d'agrandir l'image
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                //permet de cloner les pixels autour pour l'agrandir
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

            this.image = tab;
        }

        /// <summary>
        /// Fait tourner l'image de n'importe quel degré
        /// </summary>
        /// <param name="a">Degré de l'image </param>
        public void Tourner(int a)
        {
            for (int k = 0; k < a; k = k + 90)
            {
                Pixel[,] tab = new Pixel[this.image.GetLength(1), this.image.GetLength(0)];
                int largeur = this.largeur;
                int hauteur = this.hauteur;

                this.largeur = hauteur;
                this.hauteur = largeur;

                for (int i = 0; i < this.image.GetLength(1); i++)
                {
                    for (int j = 0; j < this.image.GetLength(0); j++)
                    {
                        tab[i, j] = this.image[j, image.GetLength(1) - 1 - i];
                    }
                }
                this.image = tab;
            }

        }

        /// <summary>
        /// Permet de rétrécir l'image
        /// </summary>
        /// <param name="valeur"></param>
        public void Rétrécissement(int valeur)
        {
            this.largeur = this.largeur / valeur;  //rétrécir la largeur
            this.hauteur = this.hauteur / valeur;  //rétrécir la largeur

            if (this.nombreDeBitsParCouleur == 32)  //image codé sur 32 bits
            {
                this.taille = (this.largeur * this.hauteur * 4) + this.offset;
            }
            else
            {
                this.taille = (this.largeur * this.hauteur * 3) + this.offset;
            }

            Pixel[,] petit = new Pixel[this.hauteur, this.largeur]; //matrice rétrécie

            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    int rouge = 0;
                    int vert = 0;
                    int bleu = 0;
                    int alpha = 0;
                    Pixel a;
                    //la boucle va permettre de faire la somme de la densité des couleurs des pixels côte à côte
                    for (int k = 0; k < valeur; k++)
                    {
                        for (int l = 0; l < valeur; l++)
                        {
                            rouge = rouge + this.image[i * valeur + k, j * valeur + l].r;
                            vert = vert + this.image[i * valeur + k, j * valeur + l].g;
                            bleu = bleu + this.image[i * valeur + k, j * valeur + l].b;
                            alpha = alpha + this.image[i * valeur + k, j * valeur + l].a;
                        }
                    }
                    //divise chaque couleur de chaque pixel par la valeur à la puissance 2
                    rouge = Convert.ToInt32(rouge / (Math.Pow(valeur, 2)));
                    vert = Convert.ToInt32(vert / (Math.Pow(valeur, 2)));
                    bleu = Convert.ToInt32(bleu / (Math.Pow(valeur, 2)));
                    alpha = Convert.ToInt32(alpha / (Math.Pow(valeur, 2)));
                    a = new Pixel(rouge, vert, bleu, alpha);
                    petit[i, j] = a;

                }
            }
            this.image = petit;
        }

        /// <summary>
        /// Fonction permettant de faire une convolution sur n'importe quelle image
        /// </summary>
        /// <param name="Noyau">Matrice de convolution appliqué à l'image</param>
        public void Convolution(double[,] Noyau)
        {
            int largeur = image.GetLength(1);
            int hauteur = image.GetLength(0);
            Pixel[,] img = new Pixel[hauteur, largeur];
            for (int x = 0; x < hauteur; x++)
            {
                for (int y = 0; y < largeur; y++)
                {
                    img[x, y] = Somme(Noyau, x, y);
                }
            }
            this.image = img;
        }

        /// <summary>
        /// Permet de faire la somme pour la fonction convolution
        /// </summary>
        /// <param name="noyau">Matrice de convolution</param>
        /// <param name="ligne">Ligne où s'effectue le calcul</param>
        /// <param name="colonne">Colonne où s'effectue le calcul</param>
        /// <returns></returns>
        public Pixel Somme(double[,] noyau, int ligne, int colonne)
        {

            double sommeRed = 0;
            double sommeGreen = 0;
            double sommeBlue = 0;
            double sommeAlpha = 0;
            int ligneNoyau = noyau.GetLength(0);
            int colonneNoyau = noyau.GetLength(1);

            for (int i = 0; i < ligneNoyau; i++)
            {
                for (int j = 0; j < colonneNoyau; j++)
                {

                    int x = i + (ligne - ligneNoyau / 2);
                    if (x < 0)
                    {
                        x += image.GetLength(0);
                    }
                    if (x >= image.GetLength(0))
                    {
                        x = x - image.GetLength(0);
                    }

                    int y = j + (colonne - colonneNoyau / 2);

                    if (y < 0)
                    {
                        y += image.GetLength(1);
                    }
                    if (y >= image.GetLength(1))
                    {
                        y = y - image.GetLength(1);
                    }



                    sommeRed += noyau[i, j] * image[x, y].r;
                    sommeGreen += noyau[i, j] * image[x, y].g;
                    sommeBlue += noyau[i, j] * image[x, y].b;
                }
            }
            sommeRed = (sommeRed > 255) ? 255 : (sommeRed < 0) ? 0 : sommeRed;
            sommeGreen = (sommeGreen > 255) ? 255 : (sommeGreen < 0) ? 0 : sommeGreen;
            sommeBlue = (sommeBlue > 255) ? 255 : (sommeBlue < 0) ? 0 : sommeBlue;

            Pixel couleur = new Pixel((int)Math.Ceiling(sommeRed), (int)Math.Ceiling(sommeGreen), (int)Math.Ceiling(sommeBlue));
            return couleur;
        }

        /// <summary>
        /// Génère une fractale 
        /// </summary>
        /// <param name="hauteur">hauteur de l'image en pixel</param>
        /// <param name="largeur">largeur de l'image en pixel</param>
        /// <returns></returns>
        public static MyImage Fractale(int hauteur, int largeur)
        {
            int limite = 500;
            MyImage myImage = new MyImage(hauteur, largeur);
            for (int i = 0; i < myImage.hauteur; i++)
            {
                for (int j = 0; j < myImage.largeur; j++)
                {
                    int compteur = 0;
                    Complex z = new Complex(0, 0);
                    Complex c = new Complex((double)(i - (myImage.hauteur / 2)) / (double)(myImage.hauteur / 4), (double)(j - (myImage.largeur / 2)) / (double)(myImage.largeur / 4));

                    while (compteur < limite)
                    {
                        z = z * z + c;
                        compteur++;
                        if (z.Magnitude > 2) break;
                    }

                    int[] color = { 255, 255, 255 };

                    int red = (compteur % 255 - (255 - color[0])) < 0 ? 0 : (compteur % 255 - (255 - color[0])) > 255 ? 255 : (compteur % 255 - (255 - color[0]));
                    int green = (compteur % 255 - (255 - color[1])) < 0 ? 0 : (compteur % 255 - (255 - color[1])) > 255 ? 255 : (compteur % 255 - (255 - color[1]));
                    int blue = (compteur % 255 - (255 - color[2])) < 0 ? 0 : (compteur % 255 - (255 - color[2])) > 255 ? 255 : (compteur % 255 - (255 - color[2]));

                    if (compteur == limite) myImage.image[i, j] = new Pixel(0, 0, 0);
                    else myImage.image[i, j] = new Pixel(red, green, blue);
                }
            }
            return myImage;

        }

        /// <summary>
        /// Rotation d'une image de n'importe quel degré
        /// </summary>
        /// <param name="angle">Angle de rotation en degré</param>
        public void Rotation(double angle)
        {

            Pixel[,] result = new Pixel[this.image.GetLength(0), this.image.GetLength(1)];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = new Pixel(255, 255, 255);
                }
            }

            double rad = Math.PI * angle / 180;
            double cosinus = Math.Cos(rad);
            double sinus = Math.Sin(rad);

            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    //Les quatre lignes les plus compliqués de ma vie
                    int centreI = this.image.GetLength(0) / 2;
                    int centreJ = this.image.GetLength(1) / 2;

                    int newPosI = Convert.ToInt32(cosinus * (i - centreI) + sinus * (j - centreJ) + centreI);
                    int newPosJ = Convert.ToInt32(-sinus * (i - centreI) + cosinus * (j - centreJ) + centreJ);

                    if (newPosI >= 0 && newPosJ >= 0 && newPosI < this.image.GetLength(0) && newPosJ < this.image.GetLength(1))
                    {
                        result[i, j] = image[newPosI, newPosJ];
                    }
                }
            }

            this.image = result;
        }

        /*public void MatriceDeConvultion(int[,] kernel)
        {
            
                Pixel[,] temp = new Pixel[this.hauteur, this.largeur];

                for (int i = 0; i < this.hauteur; i++)
                {
                    for (int j = 0; j < this.largeur; j++)
                    {
                        if (i > (kernel.GetLength(0) / 2) && i < this.hauteur - (kernel.GetLength(0) / 2) && j > (kernel.GetLength(1) / 2) && j < this.largeur - (kernel.GetLength(1) / 2))
                        {
                            int pixelRouge = 0;
                            int pixelVert = 0;
                            int pixelBleu = 0;

                            for (int k = 0; k < kernel.GetLength(0); k++)
                            {
                                for (int l = 0;l < kernel.GetLength(1); l++)
                                {
                                    pixelRouge += this.image[i - 1 + l, j - 1 + k].r * kernel[l, k];
                                    pixelVert += this.image[i - 1 + l, j - 1 + k].g * kernel[l, k];
                                    pixelBleu += this.image[i - 1 + l, j - 1 + k].b * kernel[l, k];
                                }
                            }
                            if (pixelBleu < 0)
                                pixelBleu = 0;
                            if (pixelRouge < 0)
                                pixelRouge = 0;
                            if (pixelVert < 0)
                                pixelVert = 0;
                            if (pixelBleu > 255)
                                pixelBleu = 255;
                            if (pixelVert > 255)
                                pixelVert = 255;
                            if (pixelRouge > 255)
                                pixelRouge = 255;

                            temp[i, j] = new Pixel(pixelRouge, pixelVert, pixelBleu);
                        }
                        else
                        {
                            temp[i, j] = this.image[i, j];
                        }
                    }
                }
            this.image = temp;
            

        }

        */
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

        /// <summary>
        /// Permet de cacher une image dans une autre
        /// </summary>
        /// <param name="Image1">Image qui va contenir l'autre image</param>
        /// <param name="Image2">Image qui sera caché</param>
        public void Caché(Pixel[,] Image1, Pixel[,] Image2)
        {
            if (Image1.GetLength(0) < Image2.GetLength(0) || Image1.GetLength(1) < Image2.GetLength(1))
            {
                Console.WriteLine("Il n'est pas possible de cacher l'image ");
            }
            else
            {

                string longueur = ConvertIntToBinaryStringTaille(Image2.GetLength(0));
                string largeur = ConvertIntToBinaryStringTaille(Image2.GetLength(1));

                string bitR;  //string de bit rouge
                string bitG;  //string de bit vert
                string bitB;  //string de bit bleu
                string bitA;  //string de bit alpha
                string bitR1; //string de bit rouge de la nouvelle image  
                string bitG1; //string de bit vert de la nouvelle image
                string bitB1;  //string de bit bleu de la nouvelle image
                string bitA1;  //string de bit alpha de la nouvelle image
                int R1;  //densité du rouge  
                int G1;  //densité du vert  
                int B1;  //densité du bleu
                int A1;  //densité du alpha
                for (int i = 0; i < Image2.GetLength(0); i++)
                {
                    bitR = "";
                    bitG = "";
                    bitB = "";
                    bitA = "";

                    bitR1 = "";
                    bitG1 = "";
                    bitB1 = "";
                    bitA1 = "";
                    R1 = 0;
                    G1 = 0;
                    B1 = 0;
                    A1 = 0;

                    Pixel couleur;
                    for (int j = 0; j < Image2.GetLength(1); j++)
                    {
                        bitR = ConvertIntToBinaryString(Image1[i, j].r);  //Convertit les entiers rouge de l'image 1 en bits    
                        bitG = ConvertIntToBinaryString(Image1[i, j].g);  //Convertit les entiers vert de l'image 1 en bits                   
                        bitB = ConvertIntToBinaryString(Image1[i, j].b); //Convertit les entiers bleu de l'image 1 en bits
                        bitA = ConvertIntToBinaryString(Image1[i, j].a); //Convertit les entiers alpha de l'image 1 en bits

                        bitR1 = bitR.Substring(0, 4);  //Ecrit les bits de poids fort de l'image 1 sur les bits de poids fort 
                        bitG1 = bitG.Substring(0, 4);
                        bitB1 = bitB.Substring(0, 4);
                        bitA1 = bitA.Substring(0, 4);

                        bitR = ConvertIntToBinaryString(Image2[i, j].r);  //Convertit les entiers de l'image 2 en bits
                        bitG = ConvertIntToBinaryString(Image2[i, j].g);
                        bitB = ConvertIntToBinaryString(Image2[i, j].b);
                        bitA = ConvertIntToBinaryString(Image2[i, j].a);

                        bitR1 = bitR1 + bitR.Substring(0, 4); //Ecrit les bits de poids fort de l'image 2 sur les bits de poids faible
                        bitG1 = bitG1 + bitG.Substring(0, 4);
                        bitB1 = bitB1 + bitB.Substring(0, 4);
                        bitA1 = bitA1 + bitA.Substring(0, 4);


                        R1 = ConvertBinaryStringToInt(bitR1); //Convertit les bits en int 
                        G1 = ConvertBinaryStringToInt(bitG1);
                        B1 = ConvertBinaryStringToInt(bitB1);
                        A1 = ConvertBinaryStringToInt(bitA1);

                        couleur = new Pixel(R1, G1, B1, A1);
                        Image1[i, j] = couleur;
                    }
                }

                this.image[this.image.GetLength(0) - 1, this.image.GetLength(1) - 1] = GarderTaille(largeur, this.image[this.image.GetLength(0) - 1, this.image.GetLength(1) - 1]); //Place les données de la largeur dans les bits de poids faibles
                this.image[0, this.image.GetLength(1) - 1] = GarderTaille(longueur, this.image[0, this.image.GetLength(1) - 1]);  //Place les données de la longueur dans les bits de poids faibles
            }

        }
        /// <summary>
        /// Convertit les int (la taille) en bits
        /// </summary>
        /// <param name="value"> c'est la taille</param>
        /// <returns></returns>
        public static string ConvertIntToBinaryStringTaille(int value)
        {
            string binaryString = Convert.ToString(value, 2);  //Convertit en binaire
            int a = 16 - binaryString.Length;
            string result = "";
            for (var index = 0; index < a; index++)  //Permet de rajouter les 0 manquants
            {
                result = result + '0';
            }
            result = result + binaryString;
            return result;

        }
        /// <summary>
        /// Convertit les int (densité de la couleur) en bits
        /// </summary>
        /// <param name="value">c'est la densité</param>
        /// <returns></returns>
        public static string ConvertIntToBinaryString(int value)
        {
            string binaryString = Convert.ToString(value, 2); //Convertit en binaire

            int a = 8 - (binaryString.Length);

            string result = "";
            for (var index = 0; index < a; index++)
            {
                result = result + '0';  //Permet de rajouter les 0 manquants
            }
            result = result + binaryString;
            return result;


        }
        /// <summary>
        /// Convertit les btis en int
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static int ConvertBinaryStringToInt(string bit)
        {
            int a;
            int intensité = 0;
            for (int k = 0; k < bit.Length; k++)
            {
                a = 0;
                if (bit[k] == '1')
                {
                    a = 1;
                }
                intensité = intensité + a * Convert.ToInt32(Math.Pow(2, (bit.Length - 1 - k)));

            }
            return intensité;
        }
        /// <summary>
        /// Permet de mettre les dimensions dans un Pixel sur les bits de poids faible
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="couleur"></param>
        /// <returns></returns>
        public static Pixel GarderTaille(string bits, Pixel couleur)
        {
            string nouveaubits;

            nouveaubits = Convert.ToString(couleur.r, 2).Substring(0, 4) + bits.Substring(12, 4);
            couleur.r = ConvertBinaryStringToInt(nouveaubits);  //pour le rouge les puissances les plus petites
            nouveaubits = Convert.ToString(couleur.g, 2).Substring(0, 4) + bits.Substring(8, 4);
            couleur.g = ConvertBinaryStringToInt(nouveaubits);
            nouveaubits = Convert.ToString(couleur.b, 2).Substring(0, 4) + bits.Substring(4, 4);
            couleur.b = ConvertBinaryStringToInt(nouveaubits);
            nouveaubits = Convert.ToString(couleur.a, 2).PadLeft(4, '0').Substring(0, 4) + bits.Substring(0, 4);
            couleur.a = ConvertBinaryStringToInt(nouveaubits);  //pour le alpha les puissances les plus grandes

            return couleur;
        }


        /// <summary>
        /// Permet de retrouver l'image caché
        /// </summary>
        public void Cherché()
        {
            //Permet de retrouver la largeur de l'image 
            string a = ConvertIntToBinaryString(this.image[0, this.image.GetLength(1) - 1].a).Substring(4, 4);
            a = a + ConvertIntToBinaryString(this.image[0, this.image.GetLength(1) - 1].b).Substring(4, 4);
            a = a + ConvertIntToBinaryString(this.image[0, this.image.GetLength(1) - 1].g).Substring(4, 4);
            a = a + ConvertIntToBinaryString(this.image[0, this.image.GetLength(1) - 1].r).Substring(4, 4);
            //Permet de retrouver la longueur de l'image 
            string b = ConvertIntToBinaryString(this.image[this.image.GetLength(0) - 1, this.image.GetLength(1) - 1].a).Substring(4, 4);
            b = b + ConvertIntToBinaryString(this.image[this.image.GetLength(0) - 1, this.image.GetLength(1) - 1].b).Substring(4, 4);
            b = b + ConvertIntToBinaryString(this.image[this.image.GetLength(0) - 1, this.image.GetLength(1) - 1].g).Substring(4, 4);
            b = b + ConvertIntToBinaryString(this.image[this.image.GetLength(0) - 1, this.image.GetLength(1) - 1].r).Substring(4, 4);

            int largeur = ConvertBinaryStringToInt(a); //largeur de l'image caché
            int longueur = ConvertBinaryStringToInt(b);  //longueur de l'image caché


            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < longueur; j++)
                {
                    this.image[i, j] = Retrouver(this.image[i, j]);
                }
            }
        }
        /// <summary>
        /// Permet de prendre les bits de poids faible (les bits de poids fort de base de l'image caché) et de les placer sur les bits de poids fort
        /// </summary>
        /// <param name="couleur"></param>
        /// <returns></returns>
        public static Pixel Retrouver(Pixel couleur)
        {
            string rouge = ConvertIntToBinaryString(couleur.r);
            string vert = ConvertIntToBinaryString(couleur.g);
            string bleu = ConvertIntToBinaryString(couleur.b);

            rouge = rouge.Substring(4, 4) + "0000";
            vert = vert.Substring(4, 4) + "0000";
            bleu = bleu.Substring(4, 4) + "0000";

            Pixel nouveau = new Pixel(ConvertBinaryStringToInt(rouge), ConvertBinaryStringToInt(vert), ConvertBinaryStringToInt(bleu));
            return nouveau;

        }
    }


}

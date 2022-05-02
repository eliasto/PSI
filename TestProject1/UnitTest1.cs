using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Forms_projet_info
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEndianToInt()
        {

            MyImage test = new MyImage("./image/original.bmp");
            byte[] tab = { 7, 2, 3 };
            int a = test.Convertir_Endian_To_Int(tab);
            Assert.AreEqual(a, 197127);

        }
        [TestMethod]
        public void TestMoyennePixel()
        {
            Pixel couleur = new Pixel(200, 150, 175);
            int a = couleur.Moyenne();
            Assert.AreEqual(a, 175);
        }
        [TestMethod]
        public void TestHistogramme()
        {
            MyImage test = new MyImage("./image/original.bmp");
            MyImage hist = new MyImage("./image/hist.bmp");
            test = test.Histogramme(2);
            for (int i = 0; i < test.Image.GetLength(0); i++)
            {
                for (int j = 0; j < test.Image.GetLength(1); j++)
                {
                    Assert.AreEqual(test.Image[i, j].r, hist.Image[i, j].r);
                    Assert.AreEqual(test.Image[i, j].g, hist.Image[i, j].g);
                    Assert.AreEqual(test.Image[i, j].b, hist.Image[i, j].b);
                    Assert.AreEqual(test.Image[i, j].a, hist.Image[i, j].a);
                }
            }
        }

        [TestMethod]
        public void TestNB()
        {
            MyImage test = new MyImage("./image/original.bmp");
            test.NB();
            MyImage nb = new MyImage("./image/nb.bmp");
            for (int i = 0; i < test.Image.GetLength(0); i++)
            {
                for (int j = 0; j < test.Image.GetLength(1); j++)
                {
                    Assert.AreEqual(test.Image[i, j].r, nb.Image[i,j].r);
                    Assert.AreEqual(test.Image[i, j].g, nb.Image[i, j].g);
                    Assert.AreEqual(test.Image[i, j].b, nb.Image[i, j].b);
                    Assert.AreEqual(test.Image[i, j].a, nb.Image[i, j].a);

                }
            }
        }

        [TestMethod]
        public void Rotation30()
        {
            MyImage test = new MyImage("./image/original.bmp");
            test.Rotation(30);
            MyImage rotation30 = new MyImage("./image/rotation30.bmp");
            for (int i = 0; i < test.Image.GetLength(0); i++)
            {
                for (int j = 0; j < test.Image.GetLength(1); j++)
                {
                    Assert.AreEqual(test.Image[i, j].r, rotation30.Image[i, j].r);
                    Assert.AreEqual(test.Image[i, j].g, rotation30.Image[i, j].g);
                    Assert.AreEqual(test.Image[i, j].b, rotation30.Image[i, j].b);
                    Assert.AreEqual(test.Image[i, j].a, rotation30.Image[i, j].a);

                }
            }
        }
    }
}


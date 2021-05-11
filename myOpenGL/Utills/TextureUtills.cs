using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace myOpenGL
{
    public class TextureUtills
    {
        public static uint[] Textures = new uint[9];
        public static void GenerateTextures()
        {
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glGenTextures(9, Textures);
            string[] imagesName ={ "wall1.PNG","wall2.PNG",
                                    "car.PNG","BANNER.PNG","Metal.JPG","ArcadeFloor.PNG","car.PNG", "bear_texture.PNG","BANNER.PNG"};
            for (int i = 0; i < imagesName.Length; i++)
            {
                SetTexture(imagesName[i], Textures[i]);
            }
        }
        public static void SetTexture(string imagesName, uint texture)
        {
            Bitmap image = new Bitmap(imagesName);
            image.RotateFlip(RotateFlipType.RotateNoneFlipY); //Y axis in Windows is directed downwards, while in OpenGL-upwards
            System.Drawing.Imaging.BitmapData bitmapdata;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.glBindTexture(GL.GL_TEXTURE_2D, texture);
            //2D for XYZ
            GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                                          0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

            image.UnlockBits(bitmapdata);
            image.Dispose();
        }
    }
}

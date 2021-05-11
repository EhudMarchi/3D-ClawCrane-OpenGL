using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenGL;

namespace myOpenGL
{
    class SkyBox
    {
        //public uint[] Textures = new uint[6];
        //public void GenerateTextures()
        //{
        //    GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
        //    GL.glGenTextures(6, Textures);
        //    string[] imagesName ={ "wall.PNG","back.bmp",
        //                            "left.bmp","right.bmp","top.bmp","ArcadeFloor.PNG"};
        //    for (int i = 0; i < 6; i++)
        //    {
        //        TextureUtills.SetTexture(imagesName[i], Textures[i]);
        //    }
        //}



        public void DrawSkyBox()
        {
            GL.glPushMatrix();
            GL.glColor3d(1, 1, 1);
            GL.glRotatef(90, 0, 1, 0);

            int width = 50;
            int height = 50;
            int length = 50;


            int x = -25;
            int y = -3;
            int z = -25;


            //x = x - width / 2;
            //y = y - height / 2;
            //z = z - length / 2;

            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[1]);


            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3d(-1, 1, 1);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3d(x + width, y, z);
            GL.glNormal3d(-1, -1, 1);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3d(x + width, y + height, z);
            GL.glNormal3d(1, -1, 1);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3d(x, y + height, z);
            GL.glNormal3d(1, 1, 1);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3d(x, y, z);
            GL.glEnd();

            GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[1]);
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3d(1, 1, -1);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3d(x, y, z + length);
            GL.glNormal3d(1, -1, -1);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3d(x, y + height, z + length);
            GL.glNormal3d(-1, -1, -1);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3d(x + width, y + height, z + length);
            GL.glNormal3d(-1, 1, -1);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3d(x + width, y, z + length);
            GL.glEnd();

            GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[5]);//top
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3d(-1, -1, 1);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3d(x + width, y + height, z);
            GL.glNormal3d(-1, -1, -1);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3d(x + width, y + height, z + length);
            GL.glNormal3d(1, -1, -1);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3d(x, y + height, z + length);
            GL.glNormal3d(1, -1, 1);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3d(x, y + height, z);
            GL.glEnd();

            GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[0]);
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3d(1, -1, 1);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3d(x, y + height, z);
            GL.glNormal3d(1, -1, -1);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3d(x, y + height, z + length);
            GL.glNormal3d(1, 1, -1);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3d(x, y, z + length);
            GL.glNormal3d(1, 1, 1);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3d(x, y, z);
            GL.glEnd();

            GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[0]);
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3d(-1, 1, 1);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3d(x + width, y, z);
            GL.glNormal3d(-1, 1, -1);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3d(x + width, y, z + length);
            GL.glNormal3d(-1, -1, -1);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3d(x + width, y + height, z + length);
            GL.glNormal3d(-1, -1, 1);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3d(x + width, y + height, z);
            GL.glEnd();

            GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[5]);//floor
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3d(1, 1, 1);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3d(x + width, y, z);
            GL.glNormal3d(1, 1, -1);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3d(x + width, y, z + length);
            GL.glNormal3d(-1, 1, -1);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3d(x, y, z + length);
            GL.glNormal3d(-1, 1, 1);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3d(x, y, z);
            GL.glEnd();

            GL.glPopMatrix();
            GL.glDisable(GL.GL_TEXTURE_2D);
        }
    }
}

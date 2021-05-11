
using myOpenGL;
using myOpenGL.Objects;
using System;

namespace OpenGL
{
    public class TeddyBear : Toy
    {
        public TeddyBear(uint list, uint shadowList, int textureIndex, string objectFileName) : base(list, shadowList, textureIndex, objectFileName)
        {
        }

        public override void DrawToy(double[] translateCoords, double[] rotateCoords, bool isShadow)
        {
            float[] material = new float[3];
            material[0] = 0.0215f;
            material[1] = 0.1745f;
            material[2] = 0.0215f;
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, material);
            material[0] = 0.07568f;
            material[1] = 0.61424f;
            material[2] = 0.07568f;
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, material);
            material[0] = 0.633f;
            material[1] = 0.727811f;
            material[2] = 0.633f;
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, material);
            GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, 5f);
            GL.glTranslated(translateCoords[0], translateCoords[1], translateCoords[2]);
            if (!isShadow)
            {
                GL.glColor3d(1, 1, 1);
            }
            else
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            GL.glRotated(rotateCoords[0], rotateCoords[1], rotateCoords[2], rotateCoords[3]);
            GL.glScaled(10, 10, 10);
            if (isShadow)
            {
                GL.glCallList(SHADOW_LIST);
            }
            else
            {
                GL.glCallList(LIST);
            }
            GL.glScaled(0.1, 0.1, 0.1);
            GL.glRotated(-rotateCoords[0], rotateCoords[1], rotateCoords[2], rotateCoords[3]);
            GL.glTranslated(-translateCoords[0], -translateCoords[1], -translateCoords[2]);
            GL.glDisable(GL.GL_TEXTURE_2D);
        }
    }
}

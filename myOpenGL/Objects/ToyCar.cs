
using myOpenGL;
using myOpenGL.Objects;
using System;

namespace OpenGL
{
    public class ToyCar : Toy
    {
        public ToyCar(uint list, uint shadowList, int textureIndex, string objectFileName) : base(list, shadowList, textureIndex, objectFileName)
        {
        }

        public override void DrawToy(double[] translateCoords, double[] rotateCoords, bool isShadow)
        {
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

            if (isShadow)
            {
                GL.glCallList(SHADOW_LIST);
            }
            else
            {
                GL.glCallList(LIST);
            }
            GL.glRotated(-rotateCoords[0], rotateCoords[1], rotateCoords[2], rotateCoords[3]);
            GL.glTranslated(-translateCoords[0], -translateCoords[1], -translateCoords[2]);
            GL.glDisable(GL.GL_TEXTURE_2D);
        }
    }
}

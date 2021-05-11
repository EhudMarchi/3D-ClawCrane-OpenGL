using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL.Objects
{
   
    public class SideMachine
    {
        public uint LIST { get; }
        public uint SHADOW_LIST { get; }
        public SideMachine(uint list, uint shadowList)
        {
            LIST = list;
            SHADOW_LIST = shadowList;
            ObjectReader arcadeReader = new ObjectReader();
            arcadeReader.loadFile("arcade.obj");
            arcadeReader.createListFace(LIST, -1, false);
            arcadeReader.createListFace(SHADOW_LIST, -1, true);

        }
        public void DrawSideMachines(double[] translateCoords, bool isShadow)
        {
            if (!isShadow)
                GL.glColor3d(1, 1, 1);

            GL.glRotatef(-90, 0.0f, 1.0f, 0.0f);
            GL.glTranslated(translateCoords[0], translateCoords[1], translateCoords[2]);
            GL.glScaled(5, 5, 5);
            if (isShadow)
            {
                GL.glCallList(SHADOW_LIST);
            }
            else
            {
                GL.glCallList(LIST);
            }
            GL.glScaled(0.2, 0.2, 0.2);
            GL.glTranslated(-translateCoords[0], -translateCoords[1], -translateCoords[2]);
            GL.glRotatef(90, 0.0f, 1.0f, 0.0f);
        }
    }
}

using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL.Objects
{
   
    public class Claw
    {
        private GLUquadric m_Quadric;
        public uint LIST { get; }
        public uint SHADOW_LIST { get; }
        public float CableLength { get; set; }
        public Claw(GLUquadric i_Quadric, uint list, uint shadowList)
        {
            CableLength = 1.5f;
            m_Quadric = i_Quadric;
            LIST = list;
            SHADOW_LIST = shadowList;
            ObjectReader arcadeReader = new ObjectReader();
            arcadeReader.loadFile("claw.obj");
            arcadeReader.createListFace(LIST, -1, false);
            arcadeReader.createListFace(SHADOW_LIST, -1, true);
        }
        public void DrawClaw(double[] translateCoords, bool isShadow)
        {

            if (!isShadow)
                GL.glColor3f(0.7f, 0.7f, 0.0f);
            GL.glTranslated(translateCoords[0], translateCoords[1], translateCoords[2]);
            GL.glBegin(GL.GL_QUADS);
            //1

            GL.glVertex3d(-0.3f, -0.35f, -0.3f);
            GL.glVertex3d(-0.3f, 0f, -0.3f);
            GL.glVertex3d(0.3f, 0f, -0.3f);
            GL.glVertex3d(0.3f, -0.35f, -0.3f);

            //2
            GL.glVertex3f(-0.3f, -0.35f, -0.3f);
            GL.glVertex3f(-0.3f, -0.35f, 0.3f);
            GL.glVertex3f(-0.3f, 0f, 0.3f);
            GL.glVertex3f(-0.3f, 0f, -0.3f);
            //3

            GL.glVertex3f(-0.3f, -0.35f, -0.3f);
            GL.glVertex3f(0.3f, -0.35f, -0.3f);
            GL.glVertex3f(0.3f, -0.35f, 0.3f);
            GL.glVertex3f(-0.3f, -0.35f, 0.3f);


            //4 
            GL.glVertex3f(0.3f, -0.35f, -0.3f);
            GL.glVertex3f(0.3f, -0.35f, 0.3f);
            GL.glVertex3f(0.3f, 0f, 0.3f);
            GL.glVertex3f(0.3f, 0f, -0.3f);


            //5
            GL.glVertex3f(0.3f, 0f, 0.3f);
            GL.glVertex3f(0.3f, 0f, -0.3f);
            GL.glVertex3f(-0.3f, 0f, -0.3f);
            GL.glVertex3f(-0.3f, 0f, 0.3f);
            //6
            GL.glVertex3f(0.3f, 0f, 0.3f);
            GL.glVertex3f(-0.3f, 0f, 0.3f);
            GL.glVertex3f(-0.3f, -0.35f, 0.3f);
            GL.glVertex3f(0.3f, -0.35f, 0.3f);

            GL.glEnd();

            

            GL.glTranslated(0, -CableLength, 0);
            GL.glRotated(-90, 1, 0, 0);
            if (!isShadow)
                GL.glColor3d(0.06, 0.06, 0.06);
            GLU.gluCylinder(m_Quadric, 0.02, 0.02, CableLength, 20, 20);
            GL.glRotated(90, 1, 0, 0);
            GL.glScaled(5, 5, 5);
            GL.glCallList(LIST);
            GL.glScaled(0.2, 0.2, 0.2);
            GL.glTranslated(-translateCoords[0], -translateCoords[1]+ CableLength, -translateCoords[2]);

        }

    }
}

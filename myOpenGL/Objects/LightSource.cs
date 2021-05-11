using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    public class LightSource
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
       
        public LightSource(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public void DrawLightSource(float [] rgb)
        {
            //Draw Light Source
            GL.glDisable(GL.GL_LIGHTING);
            GL.glTranslatef(X, Y, Z);
            //Yellow Light source
            GL.glColor3f(rgb[0], rgb[1], rgb[2]);
            GLUT.glutSolidSphere(0.9, 10, 10);
            GL.glTranslatef(-X, -Y, -Z);
        }
        public float[] LightLocation()
        {
            return new float[4] { X, Y, Z, 1 };
        }
    }
}

using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL.Objects
{
    public class ClawMachine
    {
        private GLUquadric m_Quadric;
        public float HandleUpDownAngle { get; set; }
        public float HandleLeftRightAngle { get; set; }
        public double[] ClawPosition; 
        TeddyBear teddyBear;
        List<teddyBearLocation> teddyBearsLocations;
        public Claw Claw { get; }
        private struct teddyBearLocation
        {
            public double[] translation;
            public double[] rotation;
            public teddyBearLocation(double[] translation, double[] rotation)
            {
                this.translation = translation;
                this.rotation = rotation;
            }
        }
        public ClawMachine(GLUquadric i_Quadric,uint clawMachineList, uint shadowList, TeddyBear teddyBear, Claw claw)
        {
            ClawPosition = new double[3] { 0, 6.1, 0 };
            teddyBearsLocations = new List<teddyBearLocation>();
            addTeddyBearsLocations();
            this.m_Quadric = i_Quadric;
            this.teddyBear = teddyBear;
            this.Claw = claw;
            this.HandleUpDownAngle = -45;
            this.HandleLeftRightAngle = 0;
            CreateClawMachine(clawMachineList, false);
            CreateClawMachine(shadowList, true);
        }
        private void addTeddyBearsLocations()
        {
            teddyBearsLocations.Add(new teddyBearLocation(new double[3] { 0, 0, 0 }, new double[4] { 0, 0, 0, 0 }));
            teddyBearsLocations.Add(new teddyBearLocation(new double[3] { 1.1, 0, 1.1 }, new double[4] { 35, 0, 1, 0 }));
            teddyBearsLocations.Add(new teddyBearLocation(new double[3] { -1.2, 0, -0.8 }, new double[4] { -100, 0, 1, 0 }));
            teddyBearsLocations.Add(new teddyBearLocation(new double[3] { -1, 0, 1.3 }, new double[4] { -15, 0, 1, 0 }));
            teddyBearsLocations.Add(new teddyBearLocation(new double[3] { 1.3, 0, -1.3 }, new double[4] { -160, 0, 1, 0 }));
        }
        public void CreateClawMachine(uint MACHINE_LIST_KEY,bool isShadow)
        {
            GL.glPushMatrix();
            GL.glNewList(MACHINE_LIST_KEY, GL.GL_COMPILE);

            DrawBaseCube(isShadow);
            for (int i = 0; i < teddyBearsLocations.Count; i++)
            {
                //need to change location
                teddyBear.DrawToy(teddyBearsLocations[i].translation, teddyBearsLocations[i].rotation, isShadow);
            }
            Claw.DrawClaw(ClawPosition, isShadow);
            if(isShadow)
            {
                DrawHandle(isShadow);
                DrawGlass(isShadow);
            }
            else
            {
                DrawGlass(isShadow);
                DrawHandle(isShadow);
            }
            GL.glEnable(GL.GL_BLEND);
            GL.glEndList();
            GL.glPopMatrix();
        }
        private void DrawGlass(bool isShadow)
        {
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glEnable(GL.GL_BLEND);
            GL.glEnable(GL.GL_CULL_FACE);       // ON - cull polygons on their winding(twist) in window coordinates
            GL.glCullFace(GL.GL_FRONT);
            DrawGlassCube(isShadow);
            GL.glCullFace(GL.GL_BACK);        // do not show Back Surface of the cube
            DrawGlassCube(isShadow);
            GL.glDisable(GL.GL_CULL_FACE);      // OFF - cull polygons on their winding(twist) 
            GL.glDisable(GL.GL_BLEND);
        }
        private void DrawGlassCube(bool isShadow)
        {
            //float[] material = new float[3];
            //material[0] = 0.0215f;
            //material[1] = 0.1745f;
            //material[2] = 0.0215f;
            //GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, material);
            //material[0] = 0.07568f;
            //material[1] = 0.61424f;
            //material[2] = 0.07568f;
            //GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, material);
            //material[0] = 0.633f;
            //material[1] = 0.727811f;
            //material[2] = 0.633f;
            //GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, material);
            //GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, 0.5f);
            // cube

            GL.glBegin(GL.GL_QUADS);

            //1
            if (!isShadow)
            {
                GL.glColor4f(1.0f, 1.0f, 1.0f, 0.38f);
            }

            GL.glVertex3f(-2f, 0f, -2f);
            GL.glVertex3f(-2f, 6f, -2f);
            GL.glVertex3f(2f, 6f, -2f);
            GL.glVertex3f(2f, 0f, -2f);

            //2
            GL.glVertex3f(-2f, 0f, -2f);
            GL.glVertex3f(-2f, 0f, 2f);
            GL.glVertex3f(-2f, 6f, 2f);
            GL.glVertex3f(-2f, 6f, -2f);
            //3
            GL.glVertex3f(-2f, 0f, -2f);
            GL.glVertex3f(2f, 0f, -2f);
            GL.glVertex3f(2f, 0f, 2f);
            GL.glVertex3f(-2f, 0f, 2f);


            //4 
            GL.glVertex3f(2f, 0f, -2f);
            GL.glVertex3f(2f, 0f, 2f);
            GL.glVertex3f(2f, 6f, 2f);
            GL.glVertex3f(2f, 6f, -2f);


            //5
            GL.glVertex3f(2f, 6f, 2f);
            GL.glVertex3f(2f, 6f, -2f);
            GL.glVertex3f(-2f, 6f, -2f);
            GL.glVertex3f(-2f, 6f, 2f);
            //6
            GL.glVertex3f(2f, 6f, 2f);
            GL.glVertex3f(-2f, 6f, 2f);
            GL.glVertex3f(-2f, 0f, 2f);
            GL.glVertex3f(2f, 0f, 2f);

            GL.glEnd();
        }
        private void DrawHandle(bool isShadow)
        {
            GL.glRotatef(10, 1, 0, 0);
            GL.glTranslated(0, 0, 2.3f);
            if (!isShadow)
            {
                GL.glColor3f(0, 1, 0.2f);
            }
            GL.glTranslated(-0.7, -0.1, 0.1);
            GLU.gluSphere(m_Quadric, 0.38, 20, 20);
            GL.glTranslated(1.7, 0, 0);
            if (!isShadow)
            {
                GL.glColor3f(0, 0.6f, 1);
            }
            GLU.gluSphere(m_Quadric, 0.38, 20, 20);
            GL.glTranslated(-1.7, 0, 0);
            GL.glRotatef(HandleUpDownAngle, 1, 0, 0);
            GL.glRotatef(HandleLeftRightAngle, 0, 1, 0);
            //------------------------------
            if (!isShadow)
            {
                GL.glColor3d(0, 0, 0);
            }
            //black pole
            GLU.gluCylinder(m_Quadric, 0.08, 0.08, 1, 20, 20);
            GL.glTranslated(0, 0, 1);
            if (!isShadow)
            {
                GL.glColor3f(1, 0, 0);
            }
            //red handle
            GLU.gluSphere(m_Quadric, 0.2, 20, 20);
            GL.glRotatef(-HandleUpDownAngle - 10, 1, 0, 0);

            GL.glTranslated(0.7, 0, -3.16f);
            GL.glRotatef(-HandleLeftRightAngle, 0, 1, 0);

        }
        private void DrawBaseCube(bool isShadow)
        {
            // cube
            GL.glBegin(GL.GL_QUADS);
            //1
            if (!isShadow)
            {
                GL.glColor3f(1.0f, 1.0f, 0.0f);
            }

            GL.glVertex3f(-2f, -2.95f, -2f);
            GL.glVertex3f(-2f, 0f, -2f);
            GL.glVertex3f(2f, 0f, -2f);
            GL.glVertex3f(2f, -2.95f, -2f);

            //2
            GL.glVertex3f(-2f, -2.95f, -2f);
            GL.glVertex3f(-2f, -2.95f, 2f);
            GL.glVertex3f(-2f, 0f, 2f);
            GL.glVertex3f(-2f, 0f, -2f);
            //3

            GL.glVertex3f(-2f, -2.95f, -2f);
            GL.glVertex3f(2f, -2.95f, -2f);
            GL.glVertex3f(2f, -2.95f, 2f);
            GL.glVertex3f(-2f, -2.95f, 2f);


            //4 
            GL.glVertex3f(2f, -2.95f, -2f);
            GL.glVertex3f(2f, -2.95f, 2f);
            GL.glVertex3f(2f, 0f, 2f);
            GL.glVertex3f(2f, 0f, -2f);


            //5
            GL.glVertex3f(2f, 0f, 2f);
            GL.glVertex3f(2f, 0f, -2f);
            GL.glVertex3f(-2f, 0f, -2f);
            GL.glVertex3f(-2f, 0f, 2f);
            //6
            GL.glVertex3f(2f, 0f, 2f);
            GL.glVertex3f(-2f, 0f, 2f);
            GL.glVertex3f(-2f, -2.95f, 2f);
            GL.glVertex3f(2f, -2.95f, 2f);

            //--------Panel--------
            //2
            GL.glVertex3f(-2f, -2.95f, 2f);
            GL.glVertex3f(-2f, -2.95f, 3f);
            GL.glVertex3f(-2f, -1f, 3f);
            GL.glVertex3f(-2f, 0f, 2f);
            //3

            GL.glVertex3f(-2f, -2.95f, 2f);
            GL.glVertex3f(2f, -2.95f, 2f);
            GL.glVertex3f(2f, -2.95f, 3f);
            GL.glVertex3f(-2f, -2.95f, 3f);

            //4 
            GL.glVertex3f(2f, -2.95f, 2f);
            GL.glVertex3f(2f, -2.95f, 3f);
            GL.glVertex3f(2f, -1f, 3f);
            GL.glVertex3f(2f, 0f, 2f);

            //5
            GL.glVertex3f(2f, -1f, 3f);
            GL.glVertex3f(2f, 0f, 2f);
            GL.glVertex3f(-2f, 0f, 2f);
            GL.glVertex3f(-2f, -1f, 3f);
            //6
            GL.glVertex3f(2f, -1f, 3f);
            GL.glVertex3f(-2f, -1f, 3f);
            GL.glVertex3f(-2f, -3f, 3f);
            GL.glVertex3f(2f, -2.95f, 3f);
            GL.glEnd();
            // ----------Frame--------
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[4]);
            GL.glBegin(GL.GL_QUADS);
            if (!isShadow)
            {
                GL.glColor3f(1f, 1f, 1f);
            }
            //1
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(2.01f, 0f, 2.01f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(1.8f, 0f, 2.01f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(1.8f, 6f, 2.01f);
            GL.glTexCoord2f(0f, 1f);
            GL.glVertex3f(2.01f, 6f, 2.01f);

            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(2.01f, 0f, 2.01f);
            GL.glTexCoord2f(1f, 0.0f);
            GL.glVertex3f(2.01f, 0f, 1.8f);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(2.01f, 6f, 1.8f);
            GL.glTexCoord2f(0.0f, 1f);
            GL.glVertex3f(2.01f, 6f, 2.01f);
            //2
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-2.01f, 0f, 2.01f);
            GL.glTexCoord2f(1f, 0.0f);
            GL.glVertex3f(-1.8f, 0f, 2.01f);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(-1.8f, 6f, 2.01f);
            GL.glTexCoord2f(0.0f, 1f);
            GL.glVertex3f(-2.01f, 6f, 2.01f);

            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-2.01f, 0f, 2.01f);
            GL.glTexCoord2f(1f, 0.0f);
            GL.glVertex3f(-2.01f, 0f, 1.8f);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(-2.01f, 6f, 1.8f);
            GL.glTexCoord2f(0.0f, 1f);
            GL.glVertex3f(-2.01f, 6f, 2.01f);
            //3
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-2.01f, 0f, -2.01f);
            GL.glTexCoord2f(1f, 0.0f);
            GL.glVertex3f(-1.8f, 0f, -2.01f);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(-1.8f, 6f, -2.01f);
            GL.glTexCoord2f(0.0f, 1f);
            GL.glVertex3f(-2.01f, 6f, -2.01f);

            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-2.01f, 0f, -2.01f);
            GL.glTexCoord2f(1f, 0.0f);
            GL.glVertex3f(-2.01f, 0f, -1.8f);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(-2.01f, 6f, -1.8f);
            GL.glTexCoord2f(0.0f, 1f);
            GL.glVertex3f(-2.01f, 6f, -2.01f);
            //4
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(2.01f, 0f, -2.01f);
            GL.glTexCoord2f(1f, 0.0f);
            GL.glVertex3f(1.8f, 0f, -2.01f);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(1.8f, 6f, -2.01f);
            GL.glTexCoord2f(0.0f, 1f);
            GL.glVertex3f(2.01f, 6f, -2.01f);

            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(2.01f, 0f, -2.01f);
            GL.glTexCoord2f(1f, 0.0f);
            GL.glVertex3f(2.01f, 0f, -1.8f);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(2.01f, 6f, -1.8f);
            GL.glTexCoord2f(0.0f, 1f);
            GL.glVertex3f(2.01f, 6f, -2.01f);

            GL.glDisable(GL.GL_TEXTURE_2D);
            // -------------Roof---------
            if (!isShadow)
            {
                GL.glColor3f(0.2f, 0.2f, 0.2f);
            }
            GL.glVertex3f(2.1f, 6f, -2.1f);
            GL.glVertex3f(2.1f, 6f, 2.1f);
            GL.glVertex3f(2.1f, 7.5f, 2.4f);
            GL.glVertex3f(2.1f, 7.5f, -2.1f);

            GL.glVertex3f(-2.1f, 6f, -2.1f);
            GL.glVertex3f(-2.1f, 6f, 2.1f);
            GL.glVertex3f(-2.1f, 7.5f, 2.4f);
            GL.glVertex3f(-2.1f, 7.5f, -2.1f);

            GL.glVertex3f(-2.1f, 6f, 2.1f);
            GL.glVertex3f(2.1f, 6f, 2.1f);
            GL.glVertex3f(2.1f, 7.5f, 2.4f);
            GL.glVertex3f(-2.1f, 7.5f, 2.4f);

            GL.glVertex3f(-2.1f, 6f, -2.1f);
            GL.glVertex3f(2.1f, 6f, -2.1f);
            GL.glVertex3f(2.1f, 7.5f, -2.1f);
            GL.glVertex3f(-2.1f, 7.5f, -2.1f);

            GL.glVertex3f(2.1f, 6f, 2.1f);
            GL.glVertex3f(2.1f, 6f, -2.1f);
            GL.glVertex3f(-2.1f, 6f, -2.1f);
            GL.glVertex3f(-2.1f, 6f, 2.1f);

            GL.glVertex3f(-2.1f, 7.5f, -2.1f);
            GL.glVertex3f(2.1f, 7.5f, -2.1f);
            GL.glVertex3f(2.1f, 7.5f, 2.4f);
            GL.glVertex3f(-2.1f, 7.5f, 2.4f);
            GL.glEnd();
            //--------Banner-------
            if (!isShadow)
            {
                GL.glColor3f(1f, 1f, 1.0f);
            }
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[3]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-2.0f, 6.1f, 2.15f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(2.0f, 6.1f, 2.15f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(2.0f, 7.4f, 2.41f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-2.0f, 7.4f, 2.41f);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glEnd();
        }

        public void RaiseTeddyBear(int index, double yTranslation)
        {
            teddyBearsLocations[index].translation[1] += yTranslation;
        }
        public void RemoveTeddyBear(int index)
        {
            teddyBearsLocations.RemoveAt(index);
        }
        public void AddTeddyBear()
        {
            bool possiblePosition = true;
            Random random = new Random();
            double x;
            double z;
            int triesAmount = 0;
            while (possiblePosition && triesAmount < 20)
            {
                triesAmount++;
                x = random.Next(-14, 14) / 10.0;
                z = random.Next(-14, 14) / 10.0;
                for (int i = 0; i < teddyBearsLocations.Count; i++)
                {
                    if (Math.Abs(teddyBearsLocations[i].translation[0] - x) + Math.Abs(teddyBearsLocations[i].translation[2] - z) <= 1)
                    {
                        possiblePosition = false;
                    }
                }
                if (possiblePosition)
                {
                    teddyBearsLocations.Add(new teddyBearLocation(new double[3] { x, 0, z }, new double[4] { random.Next(360), 0, 1, 0 }));
                    possiblePosition = false;
                }
                else
                {
                    possiblePosition = true;
                }
            }
            
        }
        public int IsCaught(double clawX, double clawZ)
        {
            int caughtIndex = -1;
            for (int i = 0; i < teddyBearsLocations.Count; i++)
            {
                if(Math.Abs(teddyBearsLocations[i].translation[0] - clawX) + Math.Abs(teddyBearsLocations[i].translation[2] - clawZ) <= 0.3)
                {
                    caughtIndex = i;
                    break;
                }
            }
            return caughtIndex;
        }
    }
}

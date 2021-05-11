using System;
using System.Collections.Generic;
using System.Windows.Forms;
using myOpenGL;
using myOpenGL.Objects;
namespace OpenGL
{
    class cOGL
    {
        //Settings
        private uint m_uint_HWND = 0;
        private uint m_DeviceContext = 0;
        private uint m_RenderingContext = 0;
        private Control m_DisplayPanel;
        public ClawMachine ClawMachine { get; }
        public LightSource MainLightSource { get; }
        public LightSource StaticRedLightSource { get; }
        public LightSource StaticBlueLightSource { get; }

        private ShadowUtills mainShadowManager;
        private ShadowUtills redShadowManager;
        private ShadowUtills blueShadowManager;
        private SideMachine m_SideMachine;
        SkyBox skyBox = new SkyBox();
        float[,] ground = new float[3, 3];
        public float[] bannerLightPos = new float[4];
        public string[] bannerLightDirections = { "UP", "DOWN", "RIGHT", "LEFT" };
        public string bannerLightDirection;
        float[] cubeXform = new float[16];
        GLUquadric obj;
        public float zoom = 1;
        uint CLAW_MACHINE_LIST, SHADOW_LIST , TEDDY_BEAR_LIST, TEDDY_BEAR_SHADOW_LIST, SIDE_MACHINE_LIST, SIDE_MACHINE_SHADOW_LIST , CLAW_LIST, CLAW_SHADOW_LIST, CAR_LIST, CAR_SHADOW_LIST;
        TeddyBear teddyBear;
        ToyCar toyCar;
        Claw claw;
        public float[] ScrollValue = new float[14];
        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;
        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;
        double[] AccumulatedRotationsTraslations = new double[16];
        public cOGL(Control pb)
        {
            m_DisplayPanel = pb;
            InitializeGL();
            obj = GLU.gluNewQuadric(); //!!!
            
            PrepareLists();
            teddyBear = new TeddyBear(TEDDY_BEAR_LIST,TEDDY_BEAR_SHADOW_LIST,7, "toy.obj");
            toyCar = new ToyCar(CAR_LIST, CAR_SHADOW_LIST,2, "car.obj");
            claw = new Claw(obj, CLAW_LIST, CLAW_SHADOW_LIST);
            ClawMachine = new ClawMachine(obj,CLAW_MACHINE_LIST,SHADOW_LIST, teddyBear, claw);
            MainLightSource = new LightSource(0, 0, 0);
            StaticRedLightSource = new LightSource(10, 8, 10);
            StaticBlueLightSource = new LightSource(-10, 8, -10);
            m_SideMachine = new SideMachine(SIDE_MACHINE_LIST, SIDE_MACHINE_SHADOW_LIST);
            mainShadowManager = new ShadowUtills(MainLightSource);
            redShadowManager = new ShadowUtills(StaticRedLightSource);
            blueShadowManager = new ShadowUtills(StaticBlueLightSource);
            ground[0, 0] = 1;                   
            ground[0, 1] = -3f;
            ground[0, 2] = 0f;

            ground[1, 0] = 0;
            ground[1, 1] = -3f;
            ground[1, 2] = 1f;

            ground[2, 0] = 1;
            ground[2, 1] = -3f;
            ground[2, 2] = 1f;

            //----------banner light position-----------
            bannerLightPos[0] = 0;
            bannerLightPos[1] = 5.9f;
            bannerLightPos[2] = 2.7f;
            bannerLightPos[3] = 1f;
            bannerLightDirection = "RIGHT";

            
        }

        ~cOGL()
        {
            GLU.gluDeleteQuadric(obj); //!!!
            WGL.wglDeleteContext(m_RenderingContext);
        }

       


        void DrawOldAxes()
        {
	        //INITIAL axes
            GL.glEnable ( GL.GL_LINE_STIPPLE);
            GL.glLineStipple (1, 0xFF00);  //  dotted   
	        GL.glBegin( GL.GL_LINES);	
	            //x  RED
	            GL.glColor3f(1.0f,0.0f,0.0f);						
		        GL.glVertex3f( -3.0f, 0.0f, 0.0f);	
		        GL.glVertex3f( 3.0f, 0.0f, 0.0f);	
	            //y  GREEN 
	            GL.glColor3f(0.0f,1.0f,0.0f);						
		        GL.glVertex3f( 0.0f, -3.0f, 0.0f);	
		        GL.glVertex3f( 0.0f, 3.0f, 0.0f);	
	            //z  BLUE
	            GL.glColor3f(0.0f,0.0f,1.0f);						
		        GL.glVertex3f( 0.0f, 0.0f, -3.0f);	
		        GL.glVertex3f( 0.0f, 0.0f, 3.0f);	
            GL.glEnd();
            GL.glDisable ( GL.GL_LINE_STIPPLE);
        }
        void DrawAxes()
        {
            GL.glBegin( GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-3.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -3.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -3.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
            GL.glColor3f(1.0f, 1.0f, 1.0f);
            
        }

        
        void DrawFigures()
        {
            skyBox.DrawSkyBox();
            MainLightSource.DrawLightSource(new float[] { 1, 1, 0 });
            StaticRedLightSource.DrawLightSource(new float[] { 1, 0, 0 });
            StaticBlueLightSource.DrawLightSource(new float[] { 0, 0, 1 });
            //Banner Light source
            GL.glTranslatef(bannerLightPos[0], bannerLightPos[1], bannerLightPos[2]);
            Random r = new Random();
            GL.glColor3d(r.NextDouble(), r.NextDouble(), r.NextDouble());
            GLUT.glutSolidSphere(0.1, 8, 8);
            GL.glTranslatef(-bannerLightPos[0], -bannerLightPos[1], -bannerLightPos[2]);
            GL.glEnd();
            GL.glDisable(GL.GL_DEPTH_TEST);
            GL.glDisable(GL.GL_LIGHTING);
            // Start Drawing Floor Shadow
            GL.glPushMatrix();
            GL.glEnable(GL.GL_BLEND);
            GL.glColor4d(0, 0, 0, 0.25);
            mainShadowManager.MakeShadowMatrix(mainShadowManager.Ground, cubeXform);
            GL.glMultMatrixf(cubeXform);
            m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, 6 }, true);
            m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, -6 }, true);
            GL.glCallList(SHADOW_LIST);

            //GL.glColor4d(0.05, 0, 0, 0.25);
            //redShadowManager.MakeShadowMatrix(redShadowManager.Ground, cubeXform);
            //GL.glMultMatrixf(cubeXform);
            //m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, 6 }, true);
            //m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, -6 }, true);
            //GL.glCallList(SHADOW_LIST);

            //GL.glColor4d(0, 0, 0.05, 0.25);
            //blueShadowManager.MakeShadowMatrix(blueShadowManager.Ground, cubeXform);
            //GL.glMultMatrixf(cubeXform);
            //m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, 6 }, true);
            //m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, -6 }, true);
            //GL.glCallList(SHADOW_LIST);

            GL.glDisable(GL.GL_BLEND);
            GL.glPopMatrix();
            //for (int i = 0; i < 4; i++)
            //{
            //    GL.glPushMatrix();
            //    shadowManager.MakeShadowMatrix(shadowManager.Walls[i], cubeXform);
            //    GL.glMultMatrixf(cubeXform);
            //    m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, 6 }, true);
            //    m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, -6 }, true);
            //    GL.glCallList(SHADOW_LIST);
            //    GL.glPopMatrix();
            //}
            //End Drawing Floor Shadow
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_LIGHT1);
            GL.glEnable(GL.GL_LIGHT2);
            GL.glEnable(GL.GL_LIGHT3);
            GL.glEnable(GL.GL_LIGHTING);
            
            GL.glPushMatrix();
            m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, 6 }, false);
            m_SideMachine.DrawSideMachines(new double[3] { 0, -0.4, -6 }, false);

            //toyCar.DrawToy(new double[3] { 1.4, 0.3, 0 }, new double[4] { 40, 0, 1, 0 }, false);

            GL.glCallList(CLAW_MACHINE_LIST);
            GL.glPopMatrix();
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glDisable(GL.GL_LIGHTING);
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        
        public void Draw()
        {
            //Light position for shadows
            MainLightSource.X = ScrollValue[10];
            MainLightSource.Y = ScrollValue[11];
            MainLightSource.Z = ScrollValue[12];
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, MainLightSource.LightLocation());
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, new float[]{0f, 0f, 0f,1f });
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, new float[] { 1f, 1f, 1f });
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, new float[] { 0.628281f, 0.555802f, 0.366065f });

            GL.glLightfv(GL.GL_LIGHT2, GL.GL_POSITION, StaticRedLightSource.LightLocation());
            GL.glLightfv(GL.GL_LIGHT2, GL.GL_AMBIENT, new float[] { 0f, 0f, 0f, 1f });
            GL.glLightfv(GL.GL_LIGHT2, GL.GL_DIFFUSE, new float[] { 1f, 0f, 0f });
            GL.glLightfv(GL.GL_LIGHT2, GL.GL_SPECULAR, new float[] { 0.628281f, 0.555802f, 0.366065f });

            GL.glLightfv(GL.GL_LIGHT3, GL.GL_POSITION, StaticBlueLightSource.LightLocation());
            GL.glLightfv(GL.GL_LIGHT3, GL.GL_AMBIENT, new float[] { 0f, 0f, 0f ,1f});
            GL.glLightfv(GL.GL_LIGHT3, GL.GL_DIFFUSE, new float[] { 0f, 0f, 1f });
            GL.glLightfv(GL.GL_LIGHT3, GL.GL_SPECULAR, new float[] { 0.628281f, 0.555802f, 0.366065f });

            //GL.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, bannerLightPos);
            if (m_DeviceContext == 0 || m_RenderingContext == 0)
                return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            GL.glLoadIdentity();

            // not trivial
            double[] ModelVievMatrixBeforeSpecificTransforms = new double[16];
            double[] CurrentRotationTraslation = new double[16];

            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2],
                       ScrollValue[3], ScrollValue[4], ScrollValue[5],
                       ScrollValue[6], ScrollValue[7], ScrollValue[8]);
            GL.glTranslatef(0.0f, 0.0f, -3.0f);

            DrawOldAxes();

            //save current ModelView Matrix values
            //in ModelVievMatrixBeforeSpecificTransforms array
            //ModelView Matrix ========>>>>>> ModelVievMatrixBeforeSpecificTransforms
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);
            //ModelView Matrix was saved, so
            GL.glLoadIdentity(); // make it identity matrix

            //make transformation in accordance to KeyCode
            float delta;
            if (intOptionC != 0)
            {
                delta = 3.0f * Math.Abs(intOptionC) / intOptionC; // signed 5

                switch (Math.Abs(intOptionC))
                {
                    case 1:
                        GL.glRotatef(delta, 1, 0, 0);
                        break;
                    case 2:
                        GL.glRotatef(delta, 0, 1, 0);
                        break;
                    case 3:
                        GL.glRotatef(delta, 0, 0, 1);
                        break;
                    case 4:
                        GL.glTranslatef(delta / 20, 0, 0);
                        break;
                    case 5:
                        GL.glTranslatef(0, delta / 20, 0);
                        break;
                    case 6:
                        GL.glTranslatef(0, 0, delta / 20);
                        break;
                }
            }
            //as result - the ModelView Matrix now is pure representation
            //of KeyCode transform and only it !!!

            //save current ModelView Matrix values
            //in CurrentRotationTraslation array
            //ModelView Matrix =======>>>>>>> CurrentRotationTraslation
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, CurrentRotationTraslation);

            //The GL.glLoadMatrix function replaces the current matrix with
            //the one specified in its argument.
            //The current matrix is the
            //projection matrix, modelview matrix, or texture matrix,
            //determined by the current matrix mode (now is ModelView mode)
            GL.glLoadMatrixd(AccumulatedRotationsTraslations); //Global Matrix

            //The GL.glMultMatrix function multiplies the current matrix by
            //the one specified in its argument.
            //That is, if M is the current matrix and T is the matrix passed to
            //GL.glMultMatrix, then M is replaced with M • T
            GL.glMultMatrixd(CurrentRotationTraslation);

            //save the matrix product in AccumulatedRotationsTraslations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            //replace ModelViev Matrix with stored ModelVievMatrixBeforeSpecificTransforms
            GL.glLoadMatrixd(ModelVievMatrixBeforeSpecificTransforms);
            //multiply it by KeyCode defined AccumulatedRotationsTraslations matrix
            GL.glMultMatrixd(AccumulatedRotationsTraslations);
            GL.glPushMatrix(); // save the current matrix
            GL.glScalef(zoom, zoom, zoom); // scale the matrix
            DrawAxes();
            DrawFigures();
            GL.glPopMatrix(); // load the unscaled matrix
            GL.glFlush();

            WGL.wglSwapBuffers(m_DeviceContext);

        }

       

        protected virtual void InitializeGL()
		{
			m_uint_HWND = (uint)m_DisplayPanel.Handle.ToInt32();
			m_DeviceContext   = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
			// result in a failure to subsequently create the RC.
			WGL.wglSwapBuffers(m_DeviceContext);

			WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
			WGL.ZeroPixelDescriptor(ref pfd);
			pfd.nVersion        = 1; 
			pfd.dwFlags         = (WGL.PFD_DRAW_TO_WINDOW |  WGL.PFD_SUPPORT_OPENGL |  WGL.PFD_DOUBLEBUFFER); 
			pfd.iPixelType      = (byte)(WGL.PFD_TYPE_RGBA);
			pfd.cColorBits      = 32;
			pfd.cDepthBits      = 32;
			pfd.iLayerType      = (byte)(WGL.PFD_MAIN_PLANE);
            pfd.cStencilBits = 32;
            int pixelFormatIndex = 0;
			pixelFormatIndex = WGL.ChoosePixelFormat(m_DeviceContext, ref pfd);
			if(pixelFormatIndex == 0)
			{
				MessageBox.Show("Unable to retrieve pixel format");
				return;
			}

			if(WGL.SetPixelFormat(m_DeviceContext,pixelFormatIndex,ref pfd) == 0)
			{
				MessageBox.Show("Unable to set pixel format");
				return;
			}
			//Create rendering context
			m_RenderingContext = WGL.wglCreateContext(m_DeviceContext);
			if(m_RenderingContext == 0)
			{
				MessageBox.Show("Unable to get rendering context");
				return;
			}
			if(WGL.wglMakeCurrent(m_DeviceContext,m_RenderingContext) == 0)
			{
				MessageBox.Show("Unable to make rendering context current");
				return;
			}


            initRenderingGL();
        }

        public void OnResize()
        {
            GL.glViewport(0, 0, m_DisplayPanel.Width, m_DisplayPanel.Height);
            Draw();
        }

        protected virtual void initRenderingGL()
		{
			if(m_DeviceContext == 0 || m_RenderingContext == 0)
				return;
			if(m_DisplayPanel.Width == 0 || m_DisplayPanel.Height == 0)
				return;

            GL.glShadeModel(GL.GL_SMOOTH);
            GL.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
            GL.glClearDepth(1.0f);


            GL.glClearColor(0.5f, 0.5f, 0.5f, 0.0f);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);
            GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_Hint, GL.GL_NICEST);

            GL.glViewport(0, 0, m_DisplayPanel.Width, m_DisplayPanel.Height);
			GL.glMatrixMode ( GL.GL_PROJECTION );
			GL.glLoadIdentity();
            
            //nice 3D
			GLU.gluPerspective( 70.0,  1.0, 0.4,  500.0);

            GL.glMatrixMode ( GL.GL_MODELVIEW );
			GL.glLoadIdentity();
            TextureUtills.GenerateTextures();
            //save the current MODELVIEW Matrix (now it is Identity)
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);
		}

        void PrepareLists()
         {
             CLAW_MACHINE_LIST = GL.glGenLists(10);
             SHADOW_LIST = CLAW_MACHINE_LIST + 1;
             TEDDY_BEAR_LIST = CLAW_MACHINE_LIST + 2;
             TEDDY_BEAR_SHADOW_LIST = CLAW_MACHINE_LIST + 3;
             SIDE_MACHINE_LIST = CLAW_MACHINE_LIST + 4;
             SIDE_MACHINE_SHADOW_LIST = CLAW_MACHINE_LIST + 5;
             CLAW_LIST = CLAW_MACHINE_LIST + 6;
             CLAW_SHADOW_LIST = CLAW_MACHINE_LIST + 7;
             CAR_LIST = CLAW_MACHINE_LIST + 8;
             CAR_SHADOW_LIST = CLAW_MACHINE_LIST + 9;
        }
         public void NotifyHandleMoved()
         {
           ClawMachine.CreateClawMachine(CLAW_MACHINE_LIST, false);
         }
    }
   

}



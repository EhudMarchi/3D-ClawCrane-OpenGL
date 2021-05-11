using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGL;

namespace myOpenGL
{
    public partial class AracdeForm : Form
    {
        cOGL cGL;
        System.Media.SoundPlayer arcadeMusic = new System.Media.SoundPlayer();
        bool isDragging = false;
        int waitingTime = 600;//1 second
        int caughtIndex = -1;
        float xDifference= 0.3f, zDifference= 0f;
        bool lightDirection = true;
        private enum eCommand
        {
            FORWARD,
            BACKWARD,
            RIGHT,
            LEFT,
            DOWN,
            UP,
            WAIT,
            IDLE
        }
        private eCommand currentCommand = eCommand.IDLE;
        Point pivot;

        public AracdeForm()
        {

            InitializeComponent();
            arcadeMusic.SoundLocation = "ArcadeSound.wav";
            arcadeMusic.LoadAsync();
            cGL = new cOGL(displayPanel);
            cGL.ScrollValue[10] = 5;
            cGL.ScrollValue[11] = 12;
            cGL.ScrollValue[12] = -5;
            //apply the bars values as cGL.ScrollValue[..] properties 
            //!!!
            hScrollBarScroll(hScrollBar1, null);
            hScrollBarScroll(hScrollBar2, null);
            hScrollBarScroll(hScrollBar3, null);
            hScrollBarScroll(hScrollBar4, null);
            hScrollBarScroll(hScrollBar5, null);
            hScrollBarScroll(hScrollBar6, null);
            hScrollBarScroll(hScrollBar7, null);
            hScrollBarScroll(hScrollBar8, null);
            hScrollBarScroll(hScrollBar9, null);
            displayPanel.MouseWheel += Panel1_MouseWheel;
            displayPanel.MouseDown += Panel1_MouseDown;
            displayPanel.MouseUp += Panel1_MouseUp;
            displayPanel.MouseMove += Panel1_MouseMove;
            
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isDragging)
            {

                if (pivot.X < e.X)
                {
                    cGL.yAngle += 0.02f;
                    cGL.intOptionC = 2;
                }
                else if (pivot.X > e.X)
                {
                    cGL.yAngle -= 0.02f;
                    cGL.intOptionC = -2;
                }
                if (pivot.Y  < e.Y)
                {
                    cGL.xAngle += 0.02f;
                    cGL.intOptionC = 1;
                }
                else if (pivot.Y > e.Y)
                {
                    cGL.xAngle -= 0.02f;
                    cGL.intOptionC = -1;
                }
                pivot = e.Location;
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            cGL.intOptionC = 0;
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            pivot = e.Location;
        }

        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                cGL.zoom += 0.04f;
            }
            else
            {
                cGL.zoom -= 0.04f;
            }
        }

        //public static Panel PanelInstance
        //{
        //    get { return panel1; }
        //    set { panel1 = value; }
        //}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            cGL.Draw();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void hScrollBarScroll(object sender, ScrollEventArgs e)
        {
            cGL.intOptionC = 0;
            HScrollBar hb = (HScrollBar)sender;
            int n = int.Parse(hb.Name.Substring(hb.Name.Length - 1));
            cGL.ScrollValue[n - 1] = (hb.Value - 100) / 10.0f;
            if (e != null)
                cGL.Draw();
        }

        public float[] oldPos = new float[7];

        private void numericUpDownValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nUD = (NumericUpDown)sender;
            int i = int.Parse(nUD.Name.Substring(nUD.Name.Length - 1));
            int pos = (int)nUD.Value; 
            switch(i)
            {
                case 1:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xShift += 0.25f;
                        cGL.intOptionC = 4;
                    }
                    else
                    {
                        cGL.xShift -= 0.25f;
                        cGL.intOptionC = -4;
                    }
                    break;
                case 2:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yShift += 0.25f;
                        cGL.intOptionC = 5;
                    }
                    else
                    {
                        cGL.yShift -= 0.25f;
                        cGL.intOptionC = -5;
                    }
                    break;
                case 3:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zShift += 0.25f;
                        cGL.intOptionC = 6;
                    }
                    else
                    {
                        cGL.zShift -= 0.25f;
                        cGL.intOptionC = -6;
                    }
                    break;
                case 4:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xAngle += 5;
                        cGL.intOptionC = 1;
                    }
                    else
                    {
                        cGL.xAngle -= 5;
                        cGL.intOptionC = -1;
                    }
                    break;
                case 5:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yAngle += 5;
                        cGL.intOptionC = 2;
                    }
                    else
                    {
                        cGL.yAngle -= 5;
                        cGL.intOptionC = -2;
                    }
                    break;
                case 6: 
	                if (pos>oldPos[i-1]) 
	                {
		                cGL.zAngle+=5;
		                cGL.intOptionC=3;
	                }
	                else
	                {
                        cGL.zAngle -= 5;
                        cGL.intOptionC = -3;
                    }
                    break;
            }
            cGL.Draw();
            oldPos[i - 1] = pos;
            cGL.intOptionC = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            moveClaw();
            bannerLightMove();
            spinLight();
            cGL.Draw();
        }
        private void spinLight()
        {
            
            if (xDifference <= 0.002)
            {
                lightDirection = !lightDirection;
                xDifference += 0.3f;
                zDifference = 0;
            }

            xDifference -= 0.002f;
            zDifference += 0.002f;
            if (lightDirection)
            {
                cGL.StaticBlueLightSource.X += xDifference;
                cGL.StaticBlueLightSource.Z += zDifference;
                cGL.StaticRedLightSource.X -= xDifference;
                cGL.StaticRedLightSource.Z -= zDifference;
            }
            else
            {
                cGL.StaticBlueLightSource.X -= xDifference;
                cGL.StaticBlueLightSource.Z -= zDifference;
                cGL.StaticRedLightSource.X += xDifference;
                cGL.StaticRedLightSource.Z += zDifference;
            }
        }
        private void moveClaw()
        {
            switch (currentCommand)
            {
                case eCommand.FORWARD:
                    if (cGL.ClawMachine.ClawPosition[2] > -1.5) {
                        cGL.ClawMachine.ClawPosition[2] -= 0.025;
                        cGL.NotifyHandleMoved();
                    }
                    break;
                case eCommand.BACKWARD:
                    if (cGL.ClawMachine.ClawPosition[2] < 1.5) { 
                        cGL.ClawMachine.ClawPosition[2] += 0.025;
                        cGL.NotifyHandleMoved();
                    }
                    break;
                case eCommand.RIGHT:
                    if (cGL.ClawMachine.ClawPosition[0] < 1.5) { 
                        cGL.ClawMachine.ClawPosition[0] += 0.025;
                        cGL.NotifyHandleMoved();
                    }
                    break;
                case eCommand.LEFT:
                    if (cGL.ClawMachine.ClawPosition[0] > -1.5) { 
                        cGL.ClawMachine.ClawPosition[0] -= 0.025;
                        cGL.NotifyHandleMoved();
                    }
                    break;
                case eCommand.DOWN:
                    if (cGL.ClawMachine.Claw.CableLength < 4.8f) {
                        cGL.ClawMachine.Claw.CableLength += 0.025f;
                        cGL.NotifyHandleMoved();
                    }
                    else
                    {
                        caughtIndex = cGL.ClawMachine.IsCaught(cGL.ClawMachine.ClawPosition[0], cGL.ClawMachine.ClawPosition[2]);
                        currentCommand = eCommand.WAIT;
                    }
                    break;
                case eCommand.WAIT:
                    if (waitingTime > 0)
                    {
                        waitingTime-=10;
                    }
                    else
                    {
                        waitingTime = 600;
                        currentCommand = eCommand.UP;
                    }
                    break;
                case eCommand.UP:
                    if (cGL.ClawMachine.Claw.CableLength > 1.5f)
                    {
                        if(caughtIndex!=-1)
                        {
                            cGL.ClawMachine.RaiseTeddyBear(caughtIndex, 0.025);
                        }
                        cGL.ClawMachine.Claw.CableLength -= 0.025f;
                        cGL.NotifyHandleMoved();
                    }
                    else
                    {
                        if(caughtIndex != -1)
                        {
                            cGL.ClawMachine.RemoveTeddyBear(caughtIndex);
                            caughtIndex = -1;
                            cGL.NotifyHandleMoved();
                        }
                        currentCommand = eCommand.IDLE;
                    }
                    break;
                default:
                    break;
            }
        }
        private void dropClaw()
        {
        }
        private void bannerLightMove()
        {
            switch (cGL.bannerLightDirection)
            {
                case "RIGHT":
                    if (cGL.bannerLightPos[0] < 2) { cGL.bannerLightPos[0] += 0.1f; }
                    else { cGL.bannerLightDirection = "UP"; }
                    break;
                case "UP":
                    if (cGL.bannerLightPos[1] < 7.1f) { cGL.bannerLightPos[1] += 0.1f; cGL.bannerLightPos[2] += 0.028f; }
                    else { cGL.bannerLightDirection = "LEFT"; }
                    break;
                case "LEFT":
                    if (cGL.bannerLightPos[0] > -2) { cGL.bannerLightPos[0] -= 0.1f; }
                    else { cGL.bannerLightDirection = "DOWN"; }
                    break;
                case "DOWN":
                    if (cGL.bannerLightPos[1] > 5.9f) { cGL.bannerLightPos[1] -= 0.1f; cGL.bannerLightPos[2] -= 0.028f; }
                    else { cGL.bannerLightDirection = "RIGHT"; }
                    break;
                default:
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked; 
        }

        private void buttonForwards_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentCommand != eCommand.DOWN && currentCommand != eCommand.UP && currentCommand != eCommand.WAIT)
            {
                currentCommand = eCommand.FORWARD;
                cGL.ClawMachine.HandleUpDownAngle -= 20;
                cGL.NotifyHandleMoved();
                cGL.Draw();
            }
        }

        private void buttonForwards_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentCommand != eCommand.DOWN && currentCommand != eCommand.UP && currentCommand != eCommand.WAIT)
            {
                currentCommand = eCommand.IDLE;
                if (cGL.ClawMachine.HandleUpDownAngle != -45)
                {
                    cGL.ClawMachine.HandleUpDownAngle += 20;
                }
                cGL.NotifyHandleMoved();
                cGL.Draw();
            }
        }

        private void buttonBackwards_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentCommand != eCommand.DOWN && currentCommand != eCommand.UP && currentCommand != eCommand.WAIT)
            {
                currentCommand = eCommand.BACKWARD;
                cGL.ClawMachine.HandleUpDownAngle += 20;
                cGL.NotifyHandleMoved();
                cGL.Draw();
            }
        }

        private void buttonBackwards_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentCommand != eCommand.DOWN && currentCommand != eCommand.UP && currentCommand != eCommand.WAIT)
            {
                currentCommand = eCommand.IDLE;
                if (cGL.ClawMachine.HandleUpDownAngle != -45)
                {
                    cGL.ClawMachine.HandleUpDownAngle -= 20;
                }
                cGL.NotifyHandleMoved();
                cGL.Draw();
            }
        }

        private void buttonRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentCommand != eCommand.DOWN && currentCommand != eCommand.UP && currentCommand != eCommand.WAIT)
            {
                currentCommand = eCommand.RIGHT;
                cGL.ClawMachine.HandleLeftRightAngle += 20;
                cGL.NotifyHandleMoved();
                cGL.Draw();
            }
        }

        private void buttonRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentCommand != eCommand.DOWN && currentCommand != eCommand.UP && currentCommand != eCommand.WAIT)
            {
                currentCommand = eCommand.IDLE;
                if (cGL.ClawMachine.HandleLeftRightAngle != 0)
                {
                    cGL.ClawMachine.HandleLeftRightAngle -= 20;
                }
                cGL.NotifyHandleMoved();
                cGL.Draw();
            }
        }

        private void buttonLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentCommand != eCommand.DOWN && currentCommand != eCommand.UP && currentCommand != eCommand.WAIT)
            {
                currentCommand = eCommand.LEFT;
                cGL.ClawMachine.HandleLeftRightAngle -= 20;
                cGL.NotifyHandleMoved();
                cGL.Draw();
            }
        }

        private void buttonLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentCommand != eCommand.DOWN && currentCommand != eCommand.UP && currentCommand != eCommand.WAIT)
            {
                currentCommand = eCommand.IDLE;
                if (cGL.ClawMachine.HandleLeftRightAngle != 0)
                {
                    cGL.ClawMachine.HandleLeftRightAngle += 20;
                }
                cGL.NotifyHandleMoved();
                cGL.Draw();
            }
        }

        private void lightScrollBarX_Scroll(object sender, ScrollEventArgs e)
        {
            cGL.ScrollValue[10] = (sender as ScrollBar).Value;
            if (e != null)
                cGL.Draw();
        }

        private void lightScrollBarY_Scroll(object sender, ScrollEventArgs e)
        {
            cGL.ScrollValue[11] = (sender as ScrollBar).Value;
            if (e != null)
                cGL.Draw();
        }

        private void lightScrollBarZ_Scroll(object sender, ScrollEventArgs e)
        {
            cGL.ScrollValue[12] = (sender as ScrollBar).Value;
            if (e != null)
                cGL.Draw();
        }

        private void buttonPlaySound_Click(object sender, EventArgs e)
        {
            arcadeMusic.PlayLooping();
        }

        private void buttonStopSound_Click(object sender, EventArgs e)
        {
            arcadeMusic.Stop();
        }

        private void dropButton_Click(object sender, EventArgs e)
        {
            currentCommand = eCommand.DOWN;
        }

        private void addTeddyBearButton_Click(object sender, EventArgs e)
        {
            cGL.ClawMachine.AddTeddyBear();
            cGL.NotifyHandleMoved();
        }
    }
}
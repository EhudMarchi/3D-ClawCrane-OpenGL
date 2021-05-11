using OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace myOpenGL
{
    class ObjectReader
    {
        public ObjectReader()
        {

        }
        public class POINT3
        {
            public double X;
            public double Y;
            public double Z;
        };
        public class TextureCoordinates
        {
            public double TU;
            public double TV;
        };
        public class NormalVector
        {
            public double NX;
            public double NY;
            public double NZ;
        };
        public class Surface
        {
            public int[] V = new int[3];
            public int[] T = new int[3];
            public int[] N = new int[3];
        };
        public class Model
        {

            public List<POINT3> V = new List<POINT3>();//V：Representative vertex. Format is V X Y Z, the latter represents V X Y Z coordinates of three vertices. Float
            public List<TextureCoordinates> VT = new List<TextureCoordinates>();//represents the texture coordinates. Format VT TU TV. Float
            public List<NormalVector> VN = new List<NormalVector>();//VN: normal vector. Each of the three should specify a vertex of the triangle normal vector. Format VN NX NY NZ. Float
            public List<Surface> F = new List<Surface>();//F：surface. Followed back surface are integer indices of the vertices belong, texture coordinates, normal vector of the face.。
        }

        public Model mesh = new Model();
        public int YU = 1;

        public void loadFile(String fileName)
        {

            StreamReader objReader = new StreamReader(fileName);
            ArrayList al = new ArrayList();
            string texLineTem = "";
            while (objReader.Peek() != -1)
            {
                texLineTem = objReader.ReadLine();
                if (texLineTem.Length < 2) continue;
                if (texLineTem.IndexOf("v") == 0)
                {
                    if (texLineTem.IndexOf("t") == 1)
                    {
                        string[] tempArray = texLineTem.Split(' ');
                        TextureCoordinates vt = new TextureCoordinates();
                        vt.TU = double.Parse(tempArray[1]);
                        vt.TV = double.Parse(tempArray[2]);
                        mesh.VT.Add(vt);
                    }
                    else if (texLineTem.IndexOf("n") == 1)
                    {
                        string[] tempArray = texLineTem.Split(new char[] { '/', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                        NormalVector vn = new NormalVector();
                        vn.NX = double.Parse(tempArray[1]);
                        vn.NY = double.Parse(tempArray[2]);
                        if (tempArray[3] == "\\")
                        {
                            texLineTem = objReader.ReadLine();
                            vn.NZ = double.Parse(texLineTem);
                        }
                        else vn.NZ = double.Parse(tempArray[3]);

                        mesh.VN.Add(vn);
                    }
                    else
                    {
                        string[] tempArray = texLineTem.Split(' ');
                        POINT3 v = new POINT3();
                        v.X = double.Parse(tempArray[1]);
                        v.Y = double.Parse(tempArray[2]);
                        v.Z = double.Parse(tempArray[3]);
                        mesh.V.Add(v);
                    }
                }
                else if (texLineTem.IndexOf("f") == 0)
                {
                    
                    string[] tempArray = texLineTem.Split(new char[] { '/', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    Surface f = new Surface();
                    int i = 0;
                    int k = 1;
                    while (i < 3)
                    {
                        if (mesh.V.Count != 0)
                        {
                            f.V[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        if (mesh.VT.Count != 0)
                        {
                            f.T[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        if (mesh.VN.Count != 0)
                        {
                            f.N[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        i++;
                    }
                    mesh.F.Add(f);

                }
            }


        }

        public void createListFace(uint list , int textureIndex, bool isShadow)
        {
            GL.glNewList(list, GL.GL_COMPILE);
            if (!(mesh.V.Count == 0))
            {
                if (!isShadow)
                {
                    if (textureIndex != -1)
                    {
                        GL.glEnable(GL.GL_TEXTURE_2D);
                        GL.glBindTexture(GL.GL_TEXTURE_2D, TextureUtills.Textures[textureIndex]);
                    }
                }
                for (int i = 0; i < mesh.F.Count; i++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    if (mesh.VT.Count != 0)
                    {
                        GL.glTexCoord2f((float)mesh.VT[mesh.F[i].T[0]].TU, (float)mesh.VT[mesh.F[i].T[0]].TV);
                    }
                    if (mesh.VN.Count != 0)
                    {
                        GL.glNormal3d(mesh.VN[mesh.F[i].N[0]].NX, mesh.VN[mesh.F[i].N[0]].NY, mesh.VN[mesh.F[i].N[0]].NZ);
                    }
                    GL.glVertex3d(mesh.V[mesh.F[i].V[0]].X / YU, mesh.V[mesh.F[i].V[0]].Y / YU, mesh.V[mesh.F[i].V[0]].Z / YU);

                    if (mesh.VT.Count != 0)
                    {
                        GL.glTexCoord2f((float)mesh.VT[mesh.F[i].T[1]].TU, (float)mesh.VT[mesh.F[i].T[1]].TV);
                    }
                    if (mesh.VN.Count != 0)
                    {
                        GL.glNormal3d(mesh.VN[mesh.F[i].N[1]].NX, mesh.VN[mesh.F[i].N[1]].NY, mesh.VN[mesh.F[i].N[1]].NZ);
                    }
                    GL.glVertex3d(mesh.V[mesh.F[i].V[1]].X / YU, mesh.V[mesh.F[i].V[1]].Y / YU, mesh.V[mesh.F[i].V[1]].Z / YU);

                    if (mesh.VT.Count != 0)
                    {
                        GL.glTexCoord2f((float)mesh.VT[mesh.F[i].T[2]].TU, (float)mesh.VT[mesh.F[i].T[2]].TV);
                    }
                    if (mesh.VN.Count != 0)
                    {
                        GL.glNormal3d(mesh.VN[mesh.F[i].N[2]].NX, mesh.VN[mesh.F[i].N[2]].NY, mesh.VN[mesh.F[i].N[2]].NZ);
                    }
                    GL.glVertex3d(mesh.V[mesh.F[i].V[2]].X / YU, mesh.V[mesh.F[i].V[2]].Y / YU, mesh.V[mesh.F[i].V[2]].Z / YU);
                    GL.glEnd();
                }
                GL.glDisable(GL.GL_TEXTURE_2D);
                GL.glEndList();

            }
        }
    }
}

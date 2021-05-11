using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    class ShadowUtills
    {

        public float[,] Ground { get; }
        public List<float[,]> Walls { get; }
        LightSource mainLightSource;
        const int x = 0;
        const int y = 1;
        const int z = 2;
        public ShadowUtills(LightSource mainLightSource )
        {
            this.mainLightSource = mainLightSource;
            Walls = new List<float[,]>();
            Walls.Add(new float[3, 3] { { -25,0, 1}, { -25,1 ,1 }, { -25,1, 0} });
            Walls.Add(new float[3, 3] { { 25,0, 1}, { 25,1 ,1 }, { 25,1, 0} });
            Walls.Add(new float[3, 3] { { 1,0, -25}, { 0,1 ,-25 }, { 1,1, -25} });
            Walls.Add(new float[3, 3] { { 1, 0, 25 }, { 0, 1, 25 }, { 1, 1, 25 } });
            Ground= new float[3, 3] { { 1, -2.95f, 0 }, { 0, -2.95f, 1 }, { 1, -2.95f, 1 } };
        }
        public void MakeShadowMatrix(float[,] points, float[] cubeXform)
        {
            float[] planeCoeff = new float[4];
            float dot;

            // Find the plane equation coefficients
            // Find the first three coefficients the same way we
            // find a normal.
            calcNormal(points, planeCoeff);

            // Find the last coefficient by back substitutions
            planeCoeff[3] = -(
                (planeCoeff[0] * points[2, 0]) + (planeCoeff[1] * points[2, 1]) +
                (planeCoeff[2] * points[2, 2]));


            // Dot product of plane and light position
            dot = planeCoeff[0] * mainLightSource.X +
                    planeCoeff[1] * mainLightSource.Y +
                    planeCoeff[2] * mainLightSource.Z +
                    planeCoeff[3];

            // Now do the projection
            // First column
            cubeXform[0] = dot - mainLightSource.X * planeCoeff[0];
            cubeXform[4] = 0.0f - mainLightSource.X * planeCoeff[1];
            cubeXform[8] = 0.0f - mainLightSource.X * planeCoeff[2];
            cubeXform[12] = 0.0f - mainLightSource.X * planeCoeff[3];

            // Second column
            cubeXform[1] = 0.0f - mainLightSource.Y * planeCoeff[0];
            cubeXform[5] = dot - mainLightSource.Y * planeCoeff[1];
            cubeXform[9] = 0.0f - mainLightSource.Y * planeCoeff[2];
            cubeXform[13] = 0.0f - mainLightSource.Y * planeCoeff[3];

            // Third Column
            cubeXform[2] = 0.0f - mainLightSource.Z * planeCoeff[0];
            cubeXform[6] = 0.0f - mainLightSource.Z * planeCoeff[1];
            cubeXform[10] = dot - mainLightSource.Z * planeCoeff[2];
            cubeXform[14] = 0.0f - mainLightSource.Z * planeCoeff[3];

            // Fourth Column
            cubeXform[3] = 0.0f - 1 * planeCoeff[0];
            cubeXform[7] = 0.0f - 1 * planeCoeff[1];
            cubeXform[11] = 0.0f - 1 * planeCoeff[2];
            cubeXform[15] = dot - 1 * planeCoeff[3];
        }
 
        void calcNormal(float[,] v, float[] outp)
        {
            float[] v1 = new float[3];
            float[] v2 = new float[3];

            // Calculate two vectors from the three points
            v1[x] = v[0, x] - v[1, x];
            v1[y] = v[0, y] - v[1, y];
            v1[z] = v[0, z] - v[1, z];

            v2[x] = v[1, x] - v[2, x];
            v2[y] = v[1, y] - v[2, y];
            v2[z] = v[1, z] - v[2, z];

            // Take the cross product of the two vectors to get
            // the normal vector which will be stored in out
            outp[x] = v1[y] * v2[z] - v1[z] * v2[y];
            outp[y] = v1[z] * v2[x] - v1[x] * v2[z];
            outp[z] = v1[x] * v2[y] - v1[y] * v2[x];

            // Normalize the vector (shorten length to one)
            ReduceToUnit(outp);
        }
        void ReduceToUnit(float[] vector)
        {
            float length;

            // Calculate the length of the vector		
            length = (float)Math.Sqrt((vector[0] * vector[0]) +
                                (vector[1] * vector[1]) +
                                (vector[2] * vector[2]));

            // Keep the program from blowing up by providing an exceptable
            // value for vectors that may calculated too close to zero.
            if (length == 0.0f)
                length = 1.0f;

            // Dividing each element by the length will result in a
            // unit normal vector.
            vector[0] /= length;
            vector[1] /= length;
            vector[2] /= length;
        }
    }
}

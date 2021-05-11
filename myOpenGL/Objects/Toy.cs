using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL.Objects
{
    public abstract class Toy
    {
        public uint LIST { get; protected set; }
        public uint SHADOW_LIST { get; protected set; }
        public int TextureIndex { get; protected set; }
        public string ObjectFileName { get; protected set; }
        public Toy(uint list, uint shadowList, int textureIndex, string objectFileName)
        {
            LIST = list;
            SHADOW_LIST = shadowList;
            TextureIndex = textureIndex;
            ObjectFileName = objectFileName;
            ObjectReader objReader = new ObjectReader();
            objReader.loadFile(objectFileName);
            objReader.createListFace(LIST, TextureIndex, false);
            objReader.createListFace(SHADOW_LIST, TextureIndex, true);
        }
        public virtual void DrawToy(double[] translateCoords, double[] rotateCoords, bool isShadow) { }
    }
}

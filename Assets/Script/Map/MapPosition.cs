using System;
using UnityEngine;

namespace Script.Map
{
    [Serializable]
    public class MapPosition
    {
        public JsonVector3 position;
        public int forwardIndex; 
        public int backIndex;
        public int leftIndex;
        public int rightIndex;

        public MapPosition(JsonVector3 position, int forwardIndex, int backIndex, int leftIndex, int rightIndex)
        {
            this.position = position;
            this.forwardIndex = forwardIndex;
            this.backIndex = backIndex;
            this.leftIndex = leftIndex;
            this.rightIndex = rightIndex;
        }
    }
}
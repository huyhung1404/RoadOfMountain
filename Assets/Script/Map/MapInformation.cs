using System;
using System.Collections.Generic;

namespace Script.Map
{
    [Serializable]
    public class MapInformation
    {
        public JsonVector3 cameraOffset;
        public List<MapPosition> mapPositions;
    }
}
using System;
using UnityEngine;

namespace Script.Map
{
    [Serializable]
    public class JsonVector3
    {
        public float x;
        public float y;
        public float z;

        public JsonVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public JsonVector3(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }
    }

    public static class Vector3Extensions
    {
        public static Vector3 ToVector3(this JsonVector3 jsonVector3)
        {
            return new Vector3(jsonVector3.x, jsonVector3.y, jsonVector3.z);
        }

        public static JsonVector3 FromVector3(this Vector3 vector3)
        {
            return new JsonVector3(vector3);
        }
    }
}
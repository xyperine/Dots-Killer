using UnityEngine;

namespace DotsKiller.Utility
{
    public static class VectorExtensions
    {
        public static Vector2 XZPlaneVector2(this Vector3 a)
        {
            return new Vector2(a.x, a.z);
        }
        
        
        public static Vector3 XZPlane(this Vector3 a)
        {
            return a.WithY(0f);
        }


        public static Vector3 XZPlaneToVector3(this Vector2 a, float y = 0f)
        {
            return new Vector3(a.x, y, a.y);
        }


        public static Vector2 Abs(this Vector2 a)
        {
            a.Set(Mathf.Abs(a.x), Mathf.Abs(a.y));
            return a;
        }


        public static Vector3 Abs(this Vector3 a)
        {
            a.Set(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
            return a;
        }


        public static Vector3 WithY(this Vector3 a, float y)
        {
            a.Set(a.x, y, a.z);
            return a;
        }


        public static float BiggestComponentValue(this Vector2 a)
        {
            return Mathf.Max(a.x, a.y);
        }
        
        
        public static float BiggestComponentValue(this Vector3 a)
        {
            return Mathf.Max(a.x, a.y, a.z);
        }


        /// <summary>
        /// Makes a new component-wise randomized vector
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector3 RandomInsideCuboid(this Vector3 a)
        {
            float x = Random.Range(-a.x, a.x);
            float y = Random.Range(-a.y, a.y);
            float z = Random.Range(-a.z, a.z);
            
            return new Vector3(x, y, z);
        }
    }
}
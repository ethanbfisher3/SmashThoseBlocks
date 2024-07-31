using UnityEngine;

namespace Utils
{
    public static class VectorAddons
    {
        #region New Vectors
        public static Vector3 X1Z(float x, float z) { return new Vector3(x, 1f, z); }
        public static Vector3 X1Z(float value) { return new Vector3(value, 1f, value); }
        public static Vector3 XZ(float x, float z) { return new Vector3(x, 0f, z); }
        public static Vector3 XZ(float value) { return new Vector3(value, 0f, value); }
        public static Vector3 OneYZ(float y, float z) { return new Vector3(1f, y, z); }
        public static Vector3 OneYZ(float value) { return new Vector3(1f, value, value); }
        public static Vector3 YZ(float y, float z) { return new Vector3(0f, y, z); }
        public static Vector3 YZ(float value) { return new Vector3(0f, value, value); }
        public static Vector3 Y(float value) { return new Vector3(0f, value, 0f); }
        public static Vector3 Z(float value) { return new Vector3(0f, 0f, value); }
        public static Vector3 X(float value) { return new Vector3(value, 0f, 0f); }
        public static Vector3 Value(float value) { return new Vector3(value, value, value); }
        #endregion
        /// <summary>
        /// Returns a vector 3 where each component is that component of a divided by that component of b
        /// </summary>
        public static Vector3 Divide(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        /// <summary>
        /// Returns a vector 3 where each component is that component of a multiplied by that component of b
        /// </summary>
        public static Vector3 Multiply(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        /// <summary>
        /// Returns a vector 3 where each component is 1f over that component of a
        /// </summary>
        public static Vector3 Inverse(this Vector3 a)
        {
            return new Vector3(1f / a.x, 1f / a.y, 1f / a.z);
        }
        /// <summary>
        /// Returns the value of the greatest component of a
        /// </summary>
        public static float GreatestComponent(this Vector3 a)
        {
            if (a.x > a.y)
            {
                if (a.x > a.z)
                    return a.x;
                return a.z;
            }
            if (a.y > a.z)
                return a.y;
            return a.z;
        }
        /// <summary>
        /// Returns the value of the smallest component of a
        /// </summary>
        public static float SmallestComponent(this Vector3 a)
        {
            if (a.x < a.y)
            {
                if (a.x < a.z)
                    return a.x;
                return a.z;
            }
            if (a.y < a.z)
                return a.y;
            return a.z;
        }
    }
}
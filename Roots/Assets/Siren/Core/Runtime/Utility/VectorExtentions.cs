using UnityEngine;

namespace Siren
{
    /// <summary>
    /// Extension methods for vectors
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        ///     cross prodcut for a v2
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Cross(this Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }

        /// <summary>
        ///     converts a vector 2 into a vectro 3 with y value being 0
        /// </summary>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static Vector2 ToV2(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }

        /// <summary>
        ///     converts a vector2 into a vector3 with y value being 0
        /// </summary>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 ToV3(this Vector2 v2)
        {
            return new Vector3(v2.x, 0, v2.y);
        }

        /// <summary>
        ///     converts a vector 2 into a vector 3 with y value being specified
        /// </summary>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 ToV3(this Vector2 v2, float y)
        {
            return new Vector3(v2.x, y, v2.y);
        }
    }
}
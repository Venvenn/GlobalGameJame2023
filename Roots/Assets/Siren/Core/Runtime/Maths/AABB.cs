using UnityEngine;

namespace Siren
{
    public static class AABB
    {
        /// <summary>
        /// Basic intersection
        /// </summary>
        public static bool Intersect(Rect a, Rect b)
        {
            var comp1 = a.yMin > b.yMax;
            var comp2 = a.yMax < b.yMin;
            var comp3 = a.xMin < b.xMax;
            var comp4 = a.xMax > b.xMin;

            return comp1 && comp2 && comp3 && comp4;
        }

        /// <summary>
        /// Separating Axis Collision
        /// </summary>
        public static bool SeparatingAxis(Rect a, Rect b, ref Vector3 mtv, ref float penetration)
        {
            var mtvDistance = float.MaxValue;
            var mtvAxis = new Vector3();

            // [X Axis]
            if (!TestAxis(new Vector3(1, 0, 0), a.xMin, a.xMax, b.xMin, b.xMax, ref mtvAxis, ref mtvDistance))
            {
                Debug.Assert(!Intersect(a, b), "Lines intersect, this should not happen");
                return false;
            }

            // [Y Axis]
            if (!TestAxis(new Vector3(0, 0, 1), a.yMin, a.yMax, b.yMin, b.yMax, ref mtvAxis, ref mtvDistance))
            {
                Debug.Assert(!Intersect(a, b), "Lines intersect, this should not happen");
                return false;
            }

            // Calculate Minimum Translation Vector (MTV) [normal * penetration]
            mtv = Vector3.Normalize(mtvAxis);

            // Get the penitraation amount
            penetration = Mathf.Sqrt(mtvDistance) * 2f;

            return true;
        }

        private static bool TestAxis(Vector3 axis, float minA, float maxA, float minB, float maxB, ref Vector3 mtvAxis, ref float mtvDistance)
        {
            var axisLengthSquared = Vector3.Dot(axis, axis);

            // If the axis is degenerate then ignore
            if (axisLengthSquared < 1.0e-8f)
                return true;

            // Calculate the two possible overlap ranges
            // Either we overlap on the left or the right sides
            var d0 = maxB - minA; // 'Left' side
            var d1 = maxA - minB; // 'Right' side

            // Intervals do not overlap, so no intersection
            if (d0 <= 0.0f || d1 <= 0.0f)
                return false;

            // Find out if we overlap on the 'right' or 'left' of the object.
            var overlap = d0 < d1 ? d0 : -d1;

            // The mtd vector for that axis
            var sep = axis * (overlap / axisLengthSquared);

            // The mtd vector length squared
            var sepLengthSquared = Vector3.Dot(sep, sep);

            // If that vector is smaller than our computed Minimum Translation Distance use that vector as our current MTV distance
            if (sepLengthSquared < mtvDistance)
            {
                mtvDistance = sepLengthSquared;
                mtvAxis = sep;
            }

            return true;
        }
    }
}
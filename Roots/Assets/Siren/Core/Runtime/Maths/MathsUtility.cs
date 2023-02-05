using UnityEngine;

namespace Siren
{
    public static class MathsUtility
    {
        /// <summary>
        /// Find intersection between two lines
        /// </summary>
        public static Vector3 LineLineIntersection(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {
            var lineVec3 = linePoint2 - linePoint1;
            var crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            var crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            var planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                var s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                return linePoint1 + lineVec1 * s;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Taken from http://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/
        /// </summary>
        public static bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
        {
            var isIntersecting = false;

            var denominator = (l2_p2.y - l2_p1.y) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.y - l1_p1.y);

            //Make sure the denominator is > 0, if not the lines are parallel
            if (denominator != 0f)
            {
                var u_a = ((l2_p2.x - l2_p1.x) * (l1_p1.y - l2_p1.y) - (l2_p2.y - l2_p1.y) * (l1_p1.x - l2_p1.x)) /
                          denominator;
                var u_b = ((l1_p2.x - l1_p1.x) * (l1_p1.y - l2_p1.y) - (l1_p2.y - l1_p1.y) * (l1_p1.x - l2_p1.x)) /
                          denominator;

                //Are the line segments intersecting if the end points are the same
                if (shouldIncludeEndPoints)
                {
                    //Is intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
                    if (u_a >= 0f && u_a <= 1f && u_b >= 0f && u_b <= 1f) isIntersecting = true;
                }
                else
                {
                    //Is intersecting if u_a and u_b are between 0 and 1
                    if (u_a > 0f && u_a < 1f && u_b > 0f && u_b < 1f) isIntersecting = true;
                }
            }

            return isIntersecting;
        }

        public static Mesh CreateMeshFromPoints(Vector3[] points, float meshWidth)
        {
            var verts = new Vector3[points.Length * 2];
            var uvs = new Vector2[verts.Length];
            var numTris = 2 * (points.Length - 1);
            var tris = new int[numTris * 3];
            var vertIndex = 0;
            var triIndex = 0;

            for (var i = 0; i < points.Length; i++)
            {
                var forward = Vector3.zero;
                if (i < points.Length - 1) forward += points[(i + 1) % points.Length] - points[i];

                if (i > 0) forward += points[i] - points[(i - 1 + points.Length) % points.Length];

                forward.Normalize();
                var left = new Vector3(-forward.z, 0, forward.x);

                verts[vertIndex] = points[i] + 0.5f * meshWidth * left;
                verts[vertIndex + 1] = points[i] - 0.5f * meshWidth * left;

                var completionPercent = i / (float) (points.Length - 1);
                var v = 1 - Mathf.Abs(2 * completionPercent - 1);
                uvs[vertIndex] = new Vector2(0, v);
                uvs[vertIndex + 1] = new Vector2(1, v);

                if (i < points.Length - 1)
                {
                    tris[triIndex] = vertIndex;
                    tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                    tris[triIndex + 2] = vertIndex + 1;

                    tris[triIndex + 3] = vertIndex + 1;
                    tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                    tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
                }

                vertIndex += 2;
                triIndex += 6;
            }

            var mesh = new Mesh();
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;

            return mesh;
        }

        /// <summary>
        /// If returns less than 0 its left, greater than 0 is right, 0 is on line
        /// </summary>
        public static float PointIsLeftOrRightOfLine(Vector2 point, Vector2 lineA, Vector2 lineB)
        {
            return (point.x - lineA.x) * (lineB.y - lineA.y) - (point.y - lineA.y) * (lineB.x - lineA.x);
        }

        public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
        {
            var j = polyPoints.Length - 1;
            var inside = false;
            for (var i = 0; i < polyPoints.Length; j = i++)
            {
                var pi = polyPoints[i];
                var pj = polyPoints[j];
                if ((pi.y <= p.y && p.y < pj.y || pj.y <= p.y && p.y < pi.y) &&
                    p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x)
                    inside = !inside;
            }

            return inside;
        }
    }
}
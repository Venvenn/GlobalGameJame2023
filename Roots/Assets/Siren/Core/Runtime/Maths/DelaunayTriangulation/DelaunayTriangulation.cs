using System.Collections.Generic;
using UnityEngine;

namespace Siren
{
    public static class DelaunayTriangulation
    {
        /// <summary>
        /// Delauney triangualtion with edge flipping
        /// </summary>
        public static List<Triangle> TriangulateByFlippingEdges(List<Vertex> vertices)
        {
            var triangles = IncrementalTriangulation.TriangulatePoints(vertices);

            var halfEdges = TransformFromTriangleToHalfEdge(triangles);

            var safety = 0;

            var flippedEdges = 0;

            while (true)
            {
                safety += 1;

                if (safety > 1000000000)
                {
                    Debug.Log("Stuck in endless loop");

                    break;
                }

                var hasFlippedEdge = false;

                for (var i = 0; i < halfEdges.Count; i++)
                {
                    var thisEdge = halfEdges[i];

                    if (thisEdge.OppositeEdge == null) continue;

                    var a = thisEdge.Vertex;
                    var b = thisEdge.NextEdge.Vertex;
                    var c = thisEdge.PrevEdge.Vertex;
                    var d = thisEdge.OppositeEdge.NextEdge.Vertex;

                    var aPos = a.GetXZPos2D();
                    var bPos = b.GetXZPos2D();
                    var cPos = c.GetXZPos2D();
                    var dPos = d.GetXZPos2D();

                    if (IsPointInsideOutsideOrOnCircle(aPos, bPos, cPos, dPos) < 0f)
                        if (IsQuadrilateralConvex(aPos, bPos, cPos, dPos))
                        {
                            if (IsPointInsideOutsideOrOnCircle(bPos, cPos, dPos, aPos) < 0f) continue;

                            flippedEdges += 1;

                            hasFlippedEdge = true;

                            FlipEdge(thisEdge);
                        }
                }

                if (!hasFlippedEdge) break;
            }

            return triangles;
        }

        /// <summary>
        /// Get half edges from triangles
        /// </summary>
        public static List<HalfEdge> TransformFromTriangleToHalfEdge(List<Triangle> triangles)
        {
            OrientTrianglesClockwise(triangles);

            var halfEdges = new List<HalfEdge>(triangles.Count * 3);

            for (var i = 0; i < triangles.Count; i++)
            {
                var t = triangles[i];

                var he1 = new HalfEdge(t.V1);
                var he2 = new HalfEdge(t.V2);
                var he3 = new HalfEdge(t.V3);

                he1.NextEdge = he2;
                he2.NextEdge = he3;
                he3.NextEdge = he1;

                he1.PrevEdge = he3;
                he2.PrevEdge = he1;
                he3.PrevEdge = he2;

                he1.Vertex.HalfEdge = he2;
                he2.Vertex.HalfEdge = he3;
                he3.Vertex.HalfEdge = he1;

                t.HalfEdge = he1;

                he1.Triangle = t;
                he2.Triangle = t;
                he3.Triangle = t;

                halfEdges.Add(he1);
                halfEdges.Add(he2);
                halfEdges.Add(he3);
            }

            for (var i = 0; i < halfEdges.Count; i++)
            {
                var he = halfEdges[i];

                var goingToVertex = he.Vertex;
                var goingFromVertex = he.PrevEdge.Vertex;

                for (var j = 0; j < halfEdges.Count; j++)
                {
                    if (i == j) continue;

                    var heOpposite = halfEdges[j];

                    if (goingFromVertex.Position == heOpposite.Vertex.Position &&
                        goingToVertex.Position == heOpposite.PrevEdge.Vertex.Position)
                    {
                        he.OppositeEdge = heOpposite;

                        break;
                    }
                }
            }


            return halfEdges;
        }

        /// <summary>
        /// Get edges from triangles
        /// </summary>
        public static List<Edge> TransformFromTriangleToEdge(List<Triangle> triangles)
        {
            OrientTrianglesClockwise(triangles);

            var edges = new List<Edge>(triangles.Count * 3);

            for (var i = 0; i < triangles.Count; i++)
            {
                var t = triangles[i];

                var e1 = new Edge(t.V1, t.V2);
                var e2 = new Edge(t.V2, t.V3);
                var e3 = new Edge(t.V3, t.V1);

                e1.NextEdge = e2;
                e2.NextEdge = e3;
                e3.NextEdge = e1;
                e1.PrevEdge = e3;
                e2.PrevEdge = e1;
                e3.PrevEdge = e2;

                edges.Add(e1);
                edges.Add(e2);
                edges.Add(e3);
            }

            return edges;
        }

        /// <summary>
        /// Orient triangles so they have the correct orientation
        /// </summary>
        public static void OrientTrianglesClockwise(List<Triangle> triangles)
        {
            for (var i = 0; i < triangles.Count; i++)
            {
                var tri = triangles[i];

                var v1 = new Vector2(tri.V1.Position.x, tri.V1.Position.z);
                var v2 = new Vector2(tri.V2.Position.x, tri.V2.Position.z);
                var v3 = new Vector2(tri.V3.Position.x, tri.V3.Position.z);

                if (!IsTriangleOrientedClockwise(v1, v2, v3)) tri.ChangeOrientation();
            }
        }

        /// <summary>
        /// Is a triangle in 2d space oriented clockwise or counter-clockwise
        /// </summary>
        public static bool IsTriangleOrientedClockwise(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var isClockWise = true;

            var determinant = p1.x * p2.y + p3.x * p1.y + p2.x * p3.y - p1.x * p3.y - p3.x * p2.y - p2.x * p1.y;

            if (determinant > 0f) isClockWise = false;

            return isClockWise;
        }

        /// <summary>
        /// Is a point d inside, outside or on the same circle as a, b, c
        /// </summary>
        public static float IsPointInsideOutsideOrOnCircle(Vector2 aVec, Vector2 bVec, Vector2 cVec, Vector2 dVec)
        {
            var a = aVec.x - dVec.x;
            var d = bVec.x - dVec.x;
            var g = cVec.x - dVec.x;

            var b = aVec.y - dVec.y;
            var e = bVec.y - dVec.y;
            var h = cVec.y - dVec.y;

            var c = a * a + b * b;
            var f = d * d + e * e;
            var i = g * g + h * h;

            var determinant = a * e * i + b * f * g + c * d * h - g * e * c - h * f * a - i * d * b;

            return determinant;
        }

        /// <summary>
        /// Is a quadrilateral convex? Assume no 3 points are colinear and the shape doesnt look like an hourglass
        /// </summary>
        public static bool IsQuadrilateralConvex(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            var isConvex = false;

            var abc = IsTriangleOrientedClockwise(a, b, c);
            var abd = IsTriangleOrientedClockwise(a, b, d);
            var bcd = IsTriangleOrientedClockwise(b, c, d);
            var cad = IsTriangleOrientedClockwise(c, a, d);

            if (abc && abd && bcd & !cad)
                isConvex = true;
            else if (abc && abd && !bcd & cad)
                isConvex = true;
            else if (abc && !abd && bcd & cad)
                isConvex = true;

            else if (!abc && !abd && !bcd & cad)
                isConvex = true;
            else if (!abc && !abd && bcd & !cad)
                isConvex = true;
            else if (!abc && abd && !bcd & !cad) isConvex = true;


            return isConvex;
        }

        /// <summary>
        /// Flip an edge
        /// </summary>
        private static void FlipEdge(HalfEdge one)
        {
            var two = one.NextEdge;
            var three = one.PrevEdge;

            var four = one.OppositeEdge;
            var five = one.OppositeEdge.NextEdge;
            var six = one.OppositeEdge.PrevEdge;

            var a = one.Vertex;
            var b = one.NextEdge.Vertex;
            var c = one.PrevEdge.Vertex;
            var d = one.OppositeEdge.NextEdge.Vertex;

            a.HalfEdge = one.NextEdge;
            c.HalfEdge = one.OppositeEdge.NextEdge;

            one.NextEdge = three;
            one.PrevEdge = five;

            two.NextEdge = four;
            two.PrevEdge = six;

            three.NextEdge = five;
            three.PrevEdge = one;

            four.NextEdge = six;
            four.PrevEdge = two;

            five.NextEdge = one;
            five.PrevEdge = three;

            six.NextEdge = two;
            six.PrevEdge = four;

            one.Vertex = b;
            two.Vertex = b;
            three.Vertex = c;
            four.Vertex = d;
            five.Vertex = d;
            six.Vertex = a;

            var t1 = one.Triangle;
            var t2 = four.Triangle;

            one.Triangle = t1;
            three.Triangle = t1;
            five.Triangle = t1;

            two.Triangle = t2;
            four.Triangle = t2;
            six.Triangle = t2;

            t1.V1 = b;
            t1.V2 = c;
            t1.V3 = d;

            t2.V1 = b;
            t2.V2 = d;
            t2.V3 = a;

            t1.HalfEdge = three;
            t2.HalfEdge = four;
        }
    }
}
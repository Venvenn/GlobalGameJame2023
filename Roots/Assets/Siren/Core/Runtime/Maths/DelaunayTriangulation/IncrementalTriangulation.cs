using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Siren
{
    /// <summary>
    /// Sort the points along one axis. The first 3 points form a triangle. Consider the next point and connect it with all
    /// previously connected points which are visible to the point. An edge is visible if the center of the edge is visible to the point.
    /// </summary>
    public static class IncrementalTriangulation
    {
        public static List<Triangle> TriangulatePoints(List<Vertex> points)
        {
            var triangles = new List<Triangle>();

            //Sort the points along x-axis
            //OrderBy is always soring in ascending order - use OrderByDescending to get in the other order
            points = points.OrderBy(n => n.Position.x).ToList();

            //The first 3 vertices are always forming a triangle
            var newTriangle = new Triangle(points[0].Position, points[1].Position, points[2].Position);

            triangles.Add(newTriangle);

            //All edges that form the triangles, so we have something to test against
            var edges = new List<Edge>();

            edges.Add(new Edge(newTriangle.V1, newTriangle.V2));
            edges.Add(new Edge(newTriangle.V2, newTriangle.V3));
            edges.Add(new Edge(newTriangle.V3, newTriangle.V1));

            //Add the other triangles one by one
            //Starts at 3 because we have already added 0,1,2
            for (var i = 3; i < points.Count; i++)
            {
                var currentPoint = points[i].Position;

                //The edges we add this loop or we will get stuck in an endless loop
                var newEdges = new List<Edge>();

                //Is this edge visible? We only need to check if the midpoint of the edge is visible 
                for (var j = 0; j < edges.Count; j++)
                {
                    var currentEdge = edges[j];

                    var midPoint = (currentEdge.Vertex1.Position + currentEdge.Vertex2.Position) / 2f;

                    var edgeToMidpoint = new Edge(currentPoint, midPoint);

                    //Check if this line is intersecting
                    var canSeeEdge = true;

                    for (var k = 0; k < edges.Count; k++)
                    {
                        //Dont compare the edge with itself
                        if (k == j) continue;

                        if (AreEdgesIntersecting(edgeToMidpoint, edges[k]))
                        {
                            canSeeEdge = false;

                            break;
                        }
                    }

                    //This is a valid triangle
                    if (canSeeEdge)
                    {
                        var edgeToPoint1 = new Edge(currentEdge.Vertex1, new Vertex(currentPoint));
                        var edgeToPoint2 = new Edge(currentEdge.Vertex2, new Vertex(currentPoint));

                        newEdges.Add(edgeToPoint1);
                        newEdges.Add(edgeToPoint2);

                        var newTri = new Triangle(edgeToPoint1.Vertex1, edgeToPoint1.Vertex2, edgeToPoint2.Vertex1);

                        triangles.Add(newTri);
                    }
                }
                
                for (var j = 0; j < newEdges.Count; j++)
                {
                    edges.Add(newEdges[j]);
                }
            }
            
            return triangles;
        }


        private static bool AreEdgesIntersecting(Edge edge1, Edge edge2)
        {
            var l1_p1 = new Vector2(edge1.Vertex1.Position.x, edge1.Vertex1.Position.z);
            var l1_p2 = new Vector2(edge1.Vertex2.Position.x, edge1.Vertex2.Position.z);

            var l2_p1 = new Vector2(edge2.Vertex1.Position.x, edge2.Vertex1.Position.z);
            var l2_p2 = new Vector2(edge2.Vertex2.Position.x, edge2.Vertex2.Position.z);

            var isIntersecting = MathsUtility.AreLinesIntersecting(l1_p1, l1_p2, l2_p1, l2_p2, true);

            return isIntersecting;
        }
    }
}
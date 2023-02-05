using System.Collections.Generic;
using UnityEngine;

namespace Siren
{
    public static class Urguhart
    {
        public static List<Edge> Create(List<Triangle> triangles)
        {
            var edges = new List<Edge>();
            var removedEdges = new List<Edge>();

            for (var i = 0; i < triangles.Count; i++)
            {
                var e1 = new Edge(triangles[i].V1.Position, triangles[i].V2.Position);
                var e2 = new Edge(triangles[i].V2.Position, triangles[i].V3.Position);
                var e3 = new Edge(triangles[i].V3.Position, triangles[i].V1.Position);

                if (e1.Weight <= e2.Weight || e1.Weight <= e3.Weight)
                    edges.Add(e1);
                else
                    removedEdges.Add(e1);

                if (e2.Weight <= e1.Weight || e2.Weight <= e3.Weight)
                    edges.Add(e2);
                else
                    removedEdges.Add(e2);

                if (e3.Weight <= e1.Weight || e3.Weight <= e2.Weight)
                    edges.Add(e3);
                else
                    removedEdges.Add(e3);
            }

            var toRemove = new List<Edge>();

            for (var i = 0; i < edges.Count; i++)
            for (var j = 0; j < removedEdges.Count; j++)
                if (edges[i] == removedEdges[j])
                    toRemove.Add(edges[i]);

            for (var i = 0; i < toRemove.Count; i++) edges.Remove(toRemove[i]);

            for (var i = 0; i < edges.Count; i++)
                Debug.DrawLine(edges[i].Vertex1.Position, edges[i].Vertex2.Position, Color.red, 3);

            return edges;
        }

        public static bool Valid(Edge edge, List<Edge> edges)
        {
            for (var i = 0; i < edges.Count; i++)
                if (edges[i] == edge)
                    return false;

            return true;
        }
    }
}
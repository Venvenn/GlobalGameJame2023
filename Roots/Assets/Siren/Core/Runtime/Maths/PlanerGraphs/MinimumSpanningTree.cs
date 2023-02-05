using System.Collections.Generic;
using UnityEngine;

namespace Siren
{
    public class MinimumSpanningTree
    {
        private readonly List<Edge> m_visitedEdge = new List<Edge>();
        
        public PriorityQueue<Edge> PriorityQueue;
        
        private int m_heapSize;
        
        public void Create(List<Edge> points, bool debug = false)
        {
            PriorityQueue = new PriorityQueue<Edge>(points.Count);
            for (var i = 0; i < points.Count; i++) PriorityQueue.Insert(i, points[i]);

            var safety = 0;
            var one = PriorityQueue.Pop();
            m_visitedEdge.Add(one);

            if (debug) Debug.DrawLine(one.Vertex1.Position, one.Vertex2.Position, Color.red, 100000);

            while (PriorityQueue.Count > 0 || safety > 1000)
            {
                safety++;

                var next = EvaluateEdge(one);

                if (!next.Equals(null))
                {
                    m_visitedEdge.Add(next);
                    one = next;

                    if (debug) Debug.DrawLine(one.Vertex1.Position, one.Vertex2.Position, Color.red, 100000);
                }
                else
                {
                    var moveOn = false;
                    while (!moveOn)
                        if (PriorityQueue.Count > 0)
                        {
                            var e = PriorityQueue.Pop();
                            next = EvaluateEdge(e);
                            if (next != null)
                            {
                                if (debug) Debug.DrawLine(next.Vertex1.Position, next.Vertex2.Position, Color.red, 100000);

                                m_visitedEdge.Add(next);
                                one = next;
                                moveOn = true;
                            }
                        }
                        else
                        {
                            return;
                        }
                }
            }
        }

        public Edge EvaluateEdge(Edge e)
        {
            var notNext = false;
            var notPrev = false;


            if (Visited(e.NextEdge.Vertex2)) notNext = true;

            if (Visited(e.PrevEdge.Vertex1)) notPrev = true;

            if (e.NextEdge.Weight < e.PrevEdge.Weight && !notNext)
                return e.NextEdge;
            if (!notPrev) return e.PrevEdge;

            return null;
        }

        public bool Visited(Vertex v)
        {
            for (var i = 0; i < m_visitedEdge.Count; i++)
                if (m_visitedEdge[i].Vertex1 == v || m_visitedEdge[i].Vertex2 == v)
                    return true;

            return false;
        }
    }
}
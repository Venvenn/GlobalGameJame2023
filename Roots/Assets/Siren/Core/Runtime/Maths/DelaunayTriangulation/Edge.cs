using System;
using UnityEngine;

namespace Siren
{
    /// <summary>
    /// An edge between two vertices
    /// </summary>
    public class Edge : IComparable<Edge>
    {
        // Is this edge intersecting with another edge?
        public bool IsIntersecting = false;

        public Edge NextEdge;
        public Edge PrevEdge;
        public Vertex Vertex1;
        public Vertex Vertex2;
        public float Weight;

        public Edge(Vertex v1, Vertex v2)
        {
            Vertex1 = v1;
            Vertex2 = v2;
            Weight = Vector3.Distance(v1.Position, Vertex2.Position);
        }

        public Edge(Vector3 v1, Vector3 v2)
        {
            Vertex1 = new Vertex(v1);
            Vertex2 = new Vertex(v2);
            Weight = Vector3.Distance(v1, v2);
        }

        public int CompareTo(Edge other)
        {
            if (Weight < other.Weight)
                return -1;
            if (Weight > other.Weight)
                return 1;
            return 0;
        }

        protected bool Equals(Edge other)
        {
            return Equals(Vertex1, other.Vertex1) && Equals(Vertex2, other.Vertex2) && Equals(NextEdge, other.NextEdge) &&
                   Equals(PrevEdge, other.PrevEdge) && Weight.Equals(other.Weight) &&
                   IsIntersecting == other.IsIntersecting;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Edge) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Vertex1 != null ? Vertex1.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Vertex2 != null ? Vertex2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NextEdge != null ? NextEdge.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PrevEdge != null ? PrevEdge.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Weight.GetHashCode();
                hashCode = (hashCode * 397) ^ IsIntersecting.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Get vertex in 2d space (assuming x, z)
        /// </summary>
        public Vector2 GetVertex2D(Vertex v)
        {
            return new Vector2(v.Position.x, v.Position.z);
        }

        public void FlipEdge()
        {
            var temp = Vertex1;

            Vertex1 = Vertex2;

            Vertex2 = temp;
        }

        public static bool operator ==(Edge e1, Edge e2)
        {
            if (e1.Vertex1.Position == e2.Vertex1.Position && e1.Vertex2.Position == e2.Vertex2.Position ||
                e1.Vertex2.Position == e2.Vertex1.Position && e1.Vertex1.Position == e2.Vertex2.Position)
                return true;

            return false;
        }

        public static bool operator !=(Edge e1, Edge e2)
        {
            if (e1.Vertex1.Position == e2.Vertex1.Position && e1.Vertex2.Position == e2.Vertex2.Position ||
                e1.Vertex2.Position == e2.Vertex1.Position && e1.Vertex1.Position == e2.Vertex2.Position)
                return false;

            return true;
        }
    }
}
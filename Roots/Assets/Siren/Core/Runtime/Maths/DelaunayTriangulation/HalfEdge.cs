using System;

namespace Siren
{
    public class HalfEdge : IComparable<HalfEdge>
    {
        //The next edge
        public HalfEdge NextEdge;

        //The previous
        public HalfEdge PrevEdge;

        //The face this edge is a part of
        public Triangle Triangle;

        //The vertex the edge points to
        public Vertex Vertex;

        //The edge going in the opposite direction
        public HalfEdge OppositeEdge;

        /// <summary>
        /// This structure assumes we have a vertex class with a reference to a half edge going from that vertex
        /// and a face (triangle) class with a reference to a half edge which is a part of this face 
        /// </summary>
        public HalfEdge(Vertex vertex)
        {
            Vertex = vertex;
        }

        public int CompareTo(HalfEdge other)
        {
            return Vertex.CompareTo(other.Vertex);
        }
    }
}
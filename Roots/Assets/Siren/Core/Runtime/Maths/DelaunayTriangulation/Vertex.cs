using System;
using UnityEngine;

namespace Siren
{
    public class Vertex : IComparable<Vertex>
    {
        //The outgoing halfedge (a halfedge that starts at this vertex). Doesnt matter which edge we connect to it
        public HalfEdge HalfEdge;
        public bool IsConvex;
        public bool IsEar;

        //Properties this vertex may have. Reflex is concave
        public bool IsReflex;
        public Vertex NextVertex;
        public Vector3 Position;

        //The previous and next vertex this vertex is attached to
        public Vertex PrevVertex;

        //Which triangle is this vertex a part of?
        public Triangle Triangle;

        //weighting for constructing trees
        public float Weight;

        public Vertex(Vector3 position)
        {
            Position = position;
        }

        public int CompareTo(Vertex other)
        {
            if (Weight < other.Weight)
                return -1;
            if (Weight > other.Weight)
                return 1;
            return 0;
        }


        /// <summary>
        /// Get 2d pos of this vertex
        /// </summary>
        public Vector2 GetXZPos2D()
        {
            var posXZ2d = new Vector2(Position.x, Position.z);

            return posXZ2d;
        }
    }
}
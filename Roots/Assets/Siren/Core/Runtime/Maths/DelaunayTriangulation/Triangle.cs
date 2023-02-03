using UnityEngine;

namespace Siren
{
    public class Triangle
    {
        public Color Colour = new Color(Random.value, Random.value, Random.value);

        //If we are using the half edge mesh structure, we just need one half edge
        public HalfEdge HalfEdge;

        //Corners
        public Vertex V1;
        public Vertex V2;
        public Vertex V3;

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            V1 = new Vertex(v1);
            V2 = new Vertex(v2);
            V3 = new Vertex(v3);
        }

        public Triangle(HalfEdge halfEdge)
        {
            HalfEdge = halfEdge;
        }

        /// <summary>
        /// Change orientation of triangle from cw -> ccw or ccw -> cw
        /// </summary>
        public void ChangeOrientation()
        {
            var temp = V1;

            V1 = V2;

            V2 = temp;
        }

        public void Draw()
        {
            Debug.DrawLine(V1.Position, V2.Position, Colour);
            Debug.DrawLine(V2.Position, V3.Position, Colour);
            Debug.DrawLine(V3.Position, V1.Position, Colour);
        }
    }
}
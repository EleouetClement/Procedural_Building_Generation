using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Primitive
{
    public class Primitive
    {
        public int FloorsNb { get; set; }
        public int [] Triangles { get; private set; }
        public Vector3[] Vertices { get; private set; }

        public int TriangleCount { get { return this.Triangles.Length; } }
        public int VertexCount { get { return this.Vertices.Length; } }


        public Primitive(int floorCount)
        {
            this.FloorsNb = floorCount;
        }

        public void SpawnPrimitive()
        {

        }

    }

}

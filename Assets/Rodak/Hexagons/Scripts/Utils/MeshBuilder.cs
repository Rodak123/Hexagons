using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rodak.Hexagons.HexUtils
{
    public class MeshBuilder
    {
        public readonly List<Vector3> Vertices = new();
        public readonly List<Vector2> UV = new();
        public readonly List<int> Triangles = new();

        private int vertexoffset = 0;

        public bool IsEmpty => Vertices.Count() == 0;

        public MeshBuilder() { }

        public MeshBuilder(IEnumerable<Vector3> vertices, IEnumerable<Vector2> uv, IEnumerable<int> triangles)
        {
            Append(vertices, uv, triangles);
        }

        public void Append(IEnumerable<Vector3> vertices, IEnumerable<Vector2> uv, IEnumerable<int> triangles)
        {
            if (vertices.Count() != uv.Count())
                throw new ArgumentException($"The counts of {nameof(vertices)} and {nameof(uv)} do not match");

            Vertices.AddRange(vertices);
            UV.AddRange(uv);
            Triangles.AddRange(triangles.Select((vertexIndex) => vertexIndex + vertexoffset));

            vertexoffset += vertices.Count();
        }

        public void Append(MeshBuilder other)
        {
            Append(other.Vertices, other.UV, other.Triangles);
        }

        public Mesh ToMesh()
        {
            Mesh mesh = new()
            {
                vertices = Vertices.ToArray(),
                uv = UV.ToArray(),
                triangles = Triangles.ToArray()
            };
            return mesh;
        }

    }
}

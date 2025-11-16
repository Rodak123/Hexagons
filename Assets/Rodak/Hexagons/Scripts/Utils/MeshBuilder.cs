using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rodak.Hexagons.HexUtils
{
    /// <summary>
    /// Utility class that helps with mesh creation.
    /// It collects vertices, UVs, and triangles from multiple sources to form a single mesh.
    /// </summary>
    public class MeshBuilder
    {
        /// <summary>
        /// All vertices.
        /// </summary>
        public readonly List<Vector3> Vertices = new();
        /// <summary>
        /// All uvs.
        /// </summary>
        public readonly List<Vector2> UV = new();
        /// <summary>
        /// All triangles.
        /// </summary>
        public readonly List<int> Triangles = new();

        private int vertexoffset = 0;

        /// <summary>
        /// Whether the mesh has no vertices.
        /// </summary>
        public bool IsEmpty => Vertices.Count() == 0;

        /// <summary>
        /// Initializes a new, empty instance of the MeshBuilder class.
        /// </summary>
        public MeshBuilder() { }

        /// <summary>
        /// Initializes a new instance of the MeshBuilder class and appends the initial data.
        /// </summary>
        /// <param name="vertices">Initial collection of vertices.</param>
        /// <param name="uv">Initial collection of UV coordinates.</param>
        /// <param name="triangles">Initial collection of triangle indices.</param>
        public MeshBuilder(IEnumerable<Vector3> vertices, IEnumerable<Vector2> uv, IEnumerable<int> triangles)
        {
            Append(vertices, uv, triangles);
        }

        /// <summary>
        /// Appends new mesh data to the current builder.
        /// </summary>
        /// <param name="vertices">Collection of vertices to add.</param>
        /// <param name="uv">Collection of UV coordinates to add.</param>
        /// <param name="triangles">Collection of triangle indices to add. These indices are offset to account for already existing vertices.</param>
        public void Append(IEnumerable<Vector3> vertices, IEnumerable<Vector2> uv, IEnumerable<int> triangles)
        {
            if (vertices.Count() != uv.Count())
                throw new ArgumentException($"The counts of {nameof(vertices)} and {nameof(uv)} do not match");

            Vertices.AddRange(vertices);
            UV.AddRange(uv);
            Triangles.AddRange(triangles.Select((vertexIndex) => vertexIndex + vertexoffset));

            vertexoffset += vertices.Count();
        }

        /// <summary>
        /// Merges self with another MeshBuilder.
        /// </summary>
        /// <param name="other">The MeshBuilder instance to append.</param>
        public void Append(MeshBuilder other)
        {
            Append(other.Vertices, other.UV, other.Triangles);
        }

        /// <summary>
        /// Creates a new Unity Mesh object out of the accumulated vertices, uvs and triangles.
        /// </summary>
        /// <returns>The created mesh</returns>
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
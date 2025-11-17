using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rodak.Hexagons.HexUtils
{
    /// <summary>
    /// Utility class that helps with mesh creation.
    /// It collects vertices, UVs, and triangles from multiple sources
    /// to form a single mesh.
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
        /// Per-submesh triangle lists.
        /// </summary>
        private readonly List<List<int>> submeshTriangles = new();

        private int vertexOffset = 0;

        /// <summary>
        /// Whether the mesh has no vertices.
        /// </summary>
        public bool IsEmpty => Vertices.Count == 0;

        /// <summary>
        /// Initializes a new, empty instance of the MeshBuilder class.
        /// </summary>
        public MeshBuilder()
        {
            EnsureSubmesh(0); // always at least one submesh
        }

        /// <summary>
        /// Ensures that the requested submesh index exists.
        /// </summary>
        private void EnsureSubmesh(int index)
        {
            while (submeshTriangles.Count <= index)
                submeshTriangles.Add(new List<int>());
        }

        /// <summary>
        /// Initializes a new instance of the MeshBuilder class and appends the initial data to submesh 0.
        /// </summary>
        public MeshBuilder(IEnumerable<Vector3> vertices, IEnumerable<Vector2> uv, IEnumerable<int> triangles)
        {
            EnsureSubmesh(0);
            AppendToSubmesh(0, vertices, uv, triangles);
        }

        /// <summary>
        /// Appends new mesh data to submesh 0.
        /// </summary>
        public void Append(IEnumerable<Vector3> vertices, IEnumerable<Vector2> uv, IEnumerable<int> triangles)
        {
            AppendToSubmesh(0, vertices, uv, triangles);
        }

        /// <summary>
        /// Appends new mesh data to the specified submesh.
        /// Triangle indices are automatically offset to account for existing vertices.
        /// </summary>
        /// <param name="submeshIndex">The submesh to append to.</param>
        /// <param name="vertices">Vertices to add.</param>
        /// <param name="uv">UV coordinates to add.</param>
        /// <param name="triangles">Triangle indices to add.</param>
        public void AppendToSubmesh(int submeshIndex,
                                    IEnumerable<Vector3> vertices,
                                    IEnumerable<Vector2> uv,
                                    IEnumerable<int> triangles)
        {
            if (vertices.Count() != uv.Count())
                throw new ArgumentException($"The counts of {nameof(vertices)} and {nameof(uv)} do not match");

            EnsureSubmesh(submeshIndex);

            Vertices.AddRange(vertices);
            UV.AddRange(uv);

            var triList = submeshTriangles[submeshIndex];
            triList.AddRange(triangles.Select(i => i + vertexOffset));

            vertexOffset += vertices.Count();
        }

        /// <summary>
        /// Merges self with another MeshBuilder into submesh 0.
        /// </summary>
        public void Append(MeshBuilder other) => Append(other);

        /// <summary>
        /// Merges self with another MeshBuilder into a submesh.
        /// </summary>
        public void Append(MeshBuilder other, int submeshIndex)
        {
            EnsureSubmesh(submeshIndex);
            AppendToSubmesh(submeshIndex, other.Vertices, other.UV, other.GetTriangles(0));
        }

        /// <summary>
        /// Gets triangles for the requested submesh.
        /// </summary>
        public IReadOnlyList<int> GetTriangles(int submeshIndex)
        {
            if (submeshIndex >= submeshTriangles.Count)
                return Array.Empty<int>();

            return submeshTriangles[submeshIndex];
        }

        /// <summary>
        /// Creates a new Unity Mesh object out of the accumulated vertices, uvs and triangles
        /// and assigns all submeshes.
        /// </summary>
        /// <returns>The created mesh.</returns>
        public Mesh ToMesh()
        {
            Mesh mesh = new()
            {
                vertices = Vertices.ToArray(),
                uv = UV.ToArray(),
                subMeshCount = submeshTriangles.Count
            };

            for (int i = 0; i < submeshTriangles.Count; i++)
            {
                mesh.SetTriangles(submeshTriangles[i], i);
            }

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}

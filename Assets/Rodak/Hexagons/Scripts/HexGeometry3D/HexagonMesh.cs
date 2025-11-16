using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rodak.Hexagons.HexUtils;
using Rodak.Hexagons.HexGeometry;

namespace Rodak.Hexagons.HexGeometry3D
{
    public static class HexagonMesh
    {
        public static MeshBuilder GetMesh(this Hexagon hexagon, bool flipped, float yOffset = 0, PlacementPlane placementPlane = null)
        {
            Vector3 vertexOffset = Vector3.up * yOffset;

            Vector3[] vertices = new Vector3[7];
            for (int i = 0; i < 6; i++)
                vertices[i] = hexagon.GetCorner3D(i, placementPlane) + vertexOffset;
            vertices[6] = hexagon.GetCenter3D(placementPlane) + vertexOffset;

            Vector2[] uv = new Vector2[7];
            for (int i = 0; i < 6; i++)
            {
                Vector2 cornerDirection = HexagonGeometry.GetCornerDirection(i);
                uv[i] = (cornerDirection + Vector2.one) / 2f;
            }
            uv[6] = Vector2.one / 2f;

            int[] triangles = new int[3 * 6];
            for (int i = 0; i < 6; i++)
            {
                triangles[i * 3 + 0] = i;
                triangles[i * 3 + (flipped ? 2 : 1)] = 6;
                triangles[i * 3 + (flipped ? 1 : 2)] = (i + 1) % 6;
            }

            return new(vertices, uv, triangles);
        }

        public static MeshBuilder GetPrismWallsMesh(this Hexagon hexagon, float height = 1, IEnumerable<Hexagon> excludedDirections = null, PlacementPlane placementPlane = null)
        {
            MeshBuilder meshBuillder = new();

            for (int i = 0; i < 6; i++)
            {
                if (excludedDirections?.Contains(HexagonGeometry.OrderedDirections[i]) ?? false)
                    continue;

                Vector3 startA = hexagon.GetCorner3D(i, placementPlane);
                Vector3 startB = hexagon.GetCorner3D((i + 1) % 6, placementPlane);

                float y = 0;
                while (y < height)
                {
                    Vector3 A = startA + Vector3.up * y;
                    Vector3 B = startB + Vector3.up * y;

                    Vector3 C = A + Vector3.up;
                    Vector3 D = B + Vector3.up;

                    Vector3[] vertices = new Vector3[]{
                        A, B,
                        C, D
                    };

                    Vector2[] uv = new Vector2[]{
                        new(0, 0),
                        new(1, 0),
                        new(0, 1),
                        new(1, 1),
                    };

                    int[] triangles = new int[] {
                        0, 2, 1, // A, C, B
                        1, 2, 3  // B, C, D
                    };
                    meshBuillder.Append(vertices, uv, triangles);

                    y = Mathf.Min(y + 1, height);
                }
            }

            return meshBuillder;
        }
    }
}
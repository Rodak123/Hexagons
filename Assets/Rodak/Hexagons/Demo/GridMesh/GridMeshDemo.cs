using System.Collections.Generic;
using Rodak.Hexagons.HexGeometry3D;
using Rodak.Hexagons.HexGrid;
using Rodak.Hexagons.HexUtils;
using UnityEngine;

namespace Rodak.Hexagons.Demo.GridMesh
{
    public class GridMeshDemo : MonoBehaviour
    {
        [SerializeField, Min(1)] private int size = 4;
        [SerializeField] private float stepHeight = 1;

        [Space]
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;

        private HexagonGrid<int> hexagonGrid;

        private int colorCount;

        private void Awake()
        {
            colorCount = meshRenderer.materials.Length;

            hexagonGrid = new(size, (position) => Random.Range(1, colorCount + 1));
            meshFilter.mesh = GenerateMesh();
        }

        private Mesh GenerateMesh()
        {
            PlacementPlane placementPlane = PlacementPlane.XZPlane;
            MeshBuilder meshBuilder = new();

            hexagonGrid.ForEach((Hexagon hexagon, int value) =>
            {
                int submeshIndex = value - 1;
                float height = value * stepHeight;

                MeshBuilder topCap = hexagon.GetMesh(flipped: false, height, placementPlane);
                meshBuilder.Append(topCap, submeshIndex);

                List<Hexagon> excludedDirections = new(); // all walls included
                MeshBuilder walls = hexagon.GetPrismWallsMesh(height, excludedDirections, placementPlane);
                meshBuilder.Append(walls, submeshIndex);
            });

            Mesh mesh = meshBuilder.ToMesh();
            mesh.name = "Grid Mesh Demo";
            return mesh;
        }
    }
}
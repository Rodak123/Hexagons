using System.Collections.Generic;
using Rodak.Hexagons.HexGeometry3D;
using Rodak.Hexagons.HexMap;
using Rodak.Hexagons.HexNavigation;
using Rodak.Hexagons.HexUtils;
using UnityEngine;

namespace Rodak.Hexagons.Demo.MapMesh
{
    public class MapChunkComponent : MonoBehaviour
    {
        private HexagonMap<MapTile> map;
        private HexagonChunk<MapTile> chunk;

        public void Init(HexagonMap<MapTile> map, HexagonChunk<MapTile> chunk, Material[] meshMaterials, PlacementPlane placementPlane, float stepHeight, int meshLayer)
        {
            this.chunk = chunk;
            this.map = map;

            gameObject.name = chunk.ToString();
            gameObject.layer = meshLayer;

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

            meshRenderer.materials = meshMaterials;

            Mesh mesh = GenerateMesh(placementPlane, meshMaterials, stepHeight);
            meshCollider.sharedMesh = mesh;
            meshFilter.mesh = mesh;
        }

        private Mesh GenerateMesh(PlacementPlane placementPlane, Material[] meshMaterials, float stepHeight)
        {
            MeshBuilder meshBuilder = new();

            meshBuilder.EnsureSubmesh(meshMaterials.Length);

            chunk.ForEach((Hexagon hexagonPosition, MapTile tile) =>
            {
                int submeshIndex = tile.Value;
                float height = tile.GetHeight(stepHeight);

                MeshBuilder topCap = hexagonPosition.GetMesh(flipped: false, height, placementPlane);
                meshBuilder.Append(topCap, submeshIndex);

                List<Hexagon> excludedDirections = new();
                foreach (Hexagon neighborDirection in HexagonRelationExtensions.Neighbours)
                {
                    Hexagon neighborPosition = hexagonPosition + neighborDirection;

                    if (!map.TryGetHexagonsChunk(neighborPosition, out HexagonChunk<MapTile> otherChunk))
                        continue; // Map edge

                    MapTile neighbor = otherChunk[neighborPosition];

                    if (tile.Value <= neighbor.Value)
                    {
                        // exlude if neighbor height is the same or larger
                        excludedDirections.Add(neighborDirection);
                    }
                }
                MeshBuilder walls = hexagonPosition.GetPrismWallsMesh(height, excludedDirections, placementPlane);
                meshBuilder.Append(walls, submeshIndex);
            });

            Mesh mesh = meshBuilder.ToMesh();
            mesh.name = $"{gameObject.name} Mesh";
            return mesh;
        }
    }
}
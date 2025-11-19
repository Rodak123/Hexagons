using Rodak.Hexagons.HexMap;
using UnityEngine;
using Rodak.Hexagons.HexUtils;
using System;
using Random = UnityEngine.Random;
using Rodak.Hexagons.HexGeometry3D;

namespace Rodak.Hexagons.Demo.MapMesh
{
    /// <summary>
    /// This demo shows how you can generate a procedural map mesh.
    /// And have a unit move on it.
    /// </summary>
    public class HexagonMapMeshDemo : MonoBehaviour
    {
        [SerializeField, Min(1)] private int chunkSize = 4;

        [Header("World Generation")]
        [SerializeField] private int seed = 0;
        [SerializeField] private bool randomSeed = true;
        [SerializeField] private float noiseScale = 0.02f;
        [SerializeField] private float stepHeight = 0.1f;

        [Header("Mesh Components")]
        [SerializeField] private Material[] meshMaterials;

        private HexagonMap<MapTile> map;

        private PlacementPlane placementPlane;

        private Vector2 seedOffset;

        public float StepHeight => stepHeight;
        public PlacementPlane PlacementPlane => placementPlane;

        private void Awake()
        {
            if (meshMaterials.Length == 0)
                throw new ArgumentException($"{nameof(meshMaterials)} can't be empty");

            if (randomSeed)
                seed = Mathf.FloorToInt(Random.value * int.MaxValue);

            System.Random rng = new(seed);
            const int MaxOffset = 100_000;
            seedOffset = new(
                rng.Next(-MaxOffset, MaxOffset),
                rng.Next(-MaxOffset, MaxOffset)
            );

            placementPlane = PlacementPlane.XZPlane;
            map = new(chunkSize, CreateHexagonMapValue);

            const int InitialSize = 2;
            for (int i = -InitialSize; i <= InitialSize; i++)
            {
                for (int j = -InitialSize; j <= InitialSize; j++)
                {
                    if (i + j > InitialSize || i + j < -InitialSize) continue;
                    Hexagon position = new(i, j);
                    map.GetOrCreateChunk(position, out bool wasCreated);
                }
            }

            map.ForEach((chunk) =>
            {
                CreateChunkComponent(chunk);
            });
        }

        private void CreateChunkComponent(HexagonChunk<MapTile> chunk)
        {
            GameObject chunkComponentGO = new("Chunk");
            chunkComponentGO.transform.parent = transform;

            MapChunkComponent chunkComponent = chunkComponentGO.AddComponent<MapChunkComponent>();
            chunkComponent.Init(map, chunk, meshMaterials, placementPlane, stepHeight, gameObject.layer);
        }

        private MapTile CreateHexagonMapValue(Hexagon hexagonPosition, Hexagon chunkPosition)
        {
            int colorCount = meshMaterials.Length;

            Vector2 noisePosition = new Vector2(hexagonPosition.Q, hexagonPosition.R) * noiseScale;
            float value = Mathf.Clamp01(Mathf.PerlinNoise(noisePosition.x + seedOffset.y, noisePosition.y + seedOffset.x));

            int colorIndex = Mathf.FloorToInt(value * colorCount);
            if (colorIndex == colorCount) colorIndex = colorCount - 1;

            return new(colorIndex);
        }

        public bool TryGetTileAt(Hexagon hexagonPosition, out MapTile mapTile)
        {
            if (!map.TryGetHexagonsChunk(hexagonPosition, out HexagonChunk<MapTile> chunk))
            {
                mapTile = null;
                return false; // Map edge
            }

            mapTile = chunk[hexagonPosition];
            return true;
        }

        public bool TryGetTilePositionOn(Ray ray, out Hexagon hexagonPosition)
        {
            int layerMask = 1 << gameObject.layer;
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                hexagonPosition = null;
                return false;
            }

            Vector3 hitPosition = hit.point - hit.normal * 0.0001f;
            hexagonPosition = HexagonGeometry3DExtensions.GetHexagonAt(hitPosition, placementPlane);
            return true;
        }
    }
}

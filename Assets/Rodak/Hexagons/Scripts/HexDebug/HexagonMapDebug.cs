
using Rodak.Hexagons.HexGeometry3D;
using Rodak.Hexagons.HexMap;
using UnityEngine;

namespace Rodak.Hexagons.HexDebug
{
    public static class HexagonMapDebug
    {
        public static void DebugDraw<T>(this HexagonMap<T> hexagonMap, PlacementPlane placementPlane, Color color, float duration = 0.0f, bool drawTriangles = false, bool depthTest = true)
        {
            hexagonMap.ForEach((HexagonChunk<T> hexagonChunk) =>
            {
                hexagonChunk.ForEach((Hexagon hexagon, T value) =>
                {
                    hexagon.DebugDraw(placementPlane, color, duration, drawTriangles, depthTest);
                });
            });
        }
    }
}
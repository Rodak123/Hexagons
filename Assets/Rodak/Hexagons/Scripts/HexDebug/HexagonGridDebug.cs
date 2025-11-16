using Rodak.Hexagons.HexGeometry3D;
using Rodak.Hexagons.HexGrid;
using UnityEngine;

namespace Rodak.Hexagons.HexDebug
{
    public static class HexagonGridDebug
    {
        public static void DebugDraw<T>(this HexagonGrid<T> hexagonGrid, PlacementPlane placementPlane, Color color, float duration = 0.0f, bool drawTriangles = false, bool depthTest = true)
        {
            hexagonGrid.ForEach((Hexagon hexagon, T value) =>
            {
                hexagon.DebugDraw(placementPlane, color, duration, drawTriangles, depthTest);
            });
        }
    }

}
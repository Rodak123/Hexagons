using Rodak.Hexagons.HexUtils;
using Rodak.Hexagons.HexMap;
using UnityEngine;

namespace Rodak.Hexagons.HexDebug
{
    /// <summary>
    /// Provides utility methods for drawing visual representations of HexagonMap contents in 3D for debugging.
    /// </summary>
    public static class HexagonMapDebugExtensions
    {
        /// <summary>
        /// Draws an outline of each hexagon in the map. 
        /// </summary>
        /// <typeparam name="T">Map value</typeparam>
        public static void DebugDraw<T>(
            this HexagonMap<T> hexagonMap,
            PlacementPlane placementPlane,
            Color color,
            float duration = 0.0f,
            bool drawTriangles = false,
            bool depthTest = true
            )
        {
            hexagonMap.ForEach((hexagonChunk) =>
            {
                hexagonChunk.ForEach((hexagon, value) =>
                {
                    hexagon.DebugDraw(placementPlane, color, duration, drawTriangles, depthTest);
                });
            });
        }
    }
}
using Rodak.Hexagons.HexGeometry3D;
using Rodak.Hexagons.HexMap;
using UnityEngine;

namespace Rodak.Hexagons.HexDebug
{
    /**
     * Provides utility methods for drawing visual representations of HexagonMap contents in 3D for debugging.
     */
    public static class HexagonMapDebug
    {
        /// <summary>
        /// Draws an outline of each hexagon in the map. 
        /// </summary>
        /// <typeparam name="T">Map value</typeparam>
        /// <param name="hexagonMap">Hexagon Map</param>
        /// <param name="placementPlane">3D Plane</param>
        /// <param name="color">Outline color</param>
        /// <param name="duration">Outline duration</param>
        /// <param name="drawTriangles">If true, individual edges are shown</param>
        /// <param name="depthTest">If true, renderer checks depth</param>
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
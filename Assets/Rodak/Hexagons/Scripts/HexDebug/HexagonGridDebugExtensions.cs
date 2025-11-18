using Rodak.Hexagons.HexGeometry3D;
using Rodak.Hexagons.HexGrid;
using UnityEngine;

namespace Rodak.Hexagons.HexDebug
{
    /**
     * Provides utility methods for drawing visual representations of HexagonGrid contents in 3D for debugging.
     */
    public static class HexagonGridDebugExtensions
    {

        /// <summary>
        /// Draws an outline of each hexagon in the grid. 
        /// </summary>
        /// <typeparam name="T">Grid value</typeparam>
        /// <param name="hexagonGrid">Hexagon Grid</param>
        /// <param name="placementPlane">3D Plane</param>
        /// <param name="color">Outline color</param>
        /// <param name="duration">Outline duration</param>
        /// <param name="drawTriangles">If true, individual edges are shown</param>
        /// <param name="depthTest">If true, renderer checks depth</param>
        public static void DebugDraw<T>(this HexagonGrid<T> hexagonGrid, PlacementPlane placementPlane, Color color, float duration = 0.0f, bool drawTriangles = false, bool depthTest = true)
        {
            hexagonGrid.ForEach((Hexagon hexagon, T value) =>
            {
                hexagon.DebugDraw(placementPlane, color, duration, drawTriangles, depthTest);
            });
        }
    }

}
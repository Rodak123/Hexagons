using Rodak.Hexagons.HexGeometry;
using UnityEngine;

namespace Rodak.Hexagons.HexGeometry3D
{
    /// <summary>
    /// Extends the hexagon class with many 3D geomtery related functions.
    /// </summary>
    public static class HexagonGeometry3DExtensions
    {
        /// <summary>
        /// Default PlacementPlane used when none specified.
        /// </summary>
        public static PlacementPlane DefaultPlacementPlane = PlacementPlane.XZPlane;

        private static PlacementPlane GetPlacementPlane(PlacementPlane placementPlane) => placementPlane ?? DefaultPlacementPlane;

        /// <summary>
        /// Calculates a hexagon's center on a 3D plane.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <param name="placementPlane">3D Plane</param>
        /// <returns>Center position</returns>
        public static Vector3 GetCenter3D(this Hexagon hexagon, PlacementPlane placementPlane = null)
        {
            return GetPlacementPlane(placementPlane).LayOnPlane(hexagon.GetCenter());
        }

        /// <summary>
        /// Calculates a hexagon's corner on a 3D plane.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <param name="index">Vertex index</param>
        /// <param name="placementPlane">3D plane</param>
        /// <returns>Corner position</returns>
        public static Vector3 GetCorner3D(this Hexagon hexagon, int index, PlacementPlane placementPlane = null)
        {
            return GetPlacementPlane(placementPlane).LayOnPlane(hexagon.GetCorner(index));
        }

        /// <summary>
        /// Calculates a hexagon's side on a 3D plane.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <param name="index">Vertex index</param>
        /// <param name="placementPlane">3D plane</param>
        /// <returns>Side center position</returns>
        public static Vector3 GetSide3D(this Hexagon hexagon, int index, PlacementPlane placementPlane = null)
        {
            return GetPlacementPlane(placementPlane).LayOnPlane(hexagon.GetSide(index));
        }

        /// <summary>
        /// Calculates a hexagon from a 3D point.
        /// </summary>
        /// <param name="point">3D point</param>
        /// <param name="placementPlane">3D plane</param>
        /// <returns>Hexagon containing this point</returns>
        public static Hexagon GetHexagonAt(Vector3 point, PlacementPlane placementPlane = null)
        {
            Vector2 planePosition = GetPlacementPlane(placementPlane).Get2DPosition(point);
            return HexagonGeometryExtensions.GetHexagonAt(planePosition.x, planePosition.y);
        }
    }
}
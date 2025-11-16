using Rodak.Hexagons.HexGeometry;
using UnityEngine;

namespace Rodak.Hexagons.HexGeometry3D
{
    public static class HexagonGeometry3D
    {
        private static Vector3 LayOnPlacementPlane(Vector3 vector, PlacementPlane placementPlane)
        {
            if (placementPlane == null) placementPlane = PlacementPlane.XZPlane;
            return placementPlane.LayOnPlane(vector);
        }

        public static Vector3 GetCorner3D(this Hexagon hexagon, int index, PlacementPlane placementPlane)
        {
            return LayOnPlacementPlane(hexagon.GetCorner(index), placementPlane);
        }

        public static Vector3 GetSide3D(this Hexagon hexagon, int index, PlacementPlane placementPlane)
        {
            return LayOnPlacementPlane(hexagon.GetSide(index), placementPlane);
        }

        public static Vector3 GetCenter3D(this Hexagon hexagon, PlacementPlane placementPlane)
        {
            return LayOnPlacementPlane(hexagon.GetCenter(), placementPlane);
        }

        public static Hexagon GetHexagonAt(float x, float z)
        {
            return HexagonGeometry.GetHexagonAt(x, z);
        }
    }
}
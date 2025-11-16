using Rodak.Hexagons.HexGeometry;
using UnityEngine;

namespace Rodak.Hexagons.HexGeometry3D
{
    public static class HexagonGeometry3D
    {
        public static PlacementPlane DefaultPlacementPlane = PlacementPlane.XZPlane;

        private static Vector3 LayOnPlacementPlane(Vector3 vector, PlacementPlane placementPlane = null)
        {
            if (placementPlane == null) placementPlane = DefaultPlacementPlane;
            return placementPlane.LayOnPlane(vector);
        }

        public static Vector3 GetCorner3D(this Hexagon hexagon, int index, PlacementPlane placementPlane = null)
        {
            return LayOnPlacementPlane(hexagon.GetCorner(index), placementPlane);
        }

        public static Vector3 GetSide3D(this Hexagon hexagon, int index, PlacementPlane placementPlane = null)
        {
            return LayOnPlacementPlane(hexagon.GetSide(index), placementPlane);
        }

        public static Vector3 GetCenter3D(this Hexagon hexagon, PlacementPlane placementPlane = null)
        {
            return LayOnPlacementPlane(hexagon.GetCenter(), placementPlane);
        }

        public static Hexagon GetHexagonAt(float x, float z)
        {
            return HexagonGeometry.GetHexagonAt(x, z);
        }
    }
}
using Rodak.Hexagons.HexGeometry3D;
using UnityEngine;

namespace Rodak.Hexagons.HexDebug
{
    public static class HexagonDebug
    {
        public static void DebugDraw(this Hexagon hexagon, PlacementPlane placementPlane, Color color, float duration = 5.0f, bool drawTriangles = false, bool depthTest = true)
        {
            void DrawLine(Vector3 start, Vector3 end) => Debug.DrawLine(start, end, color, duration, depthTest);

            Vector3 center = hexagon.GetCenter3D(placementPlane);
            for (int i = 0; i < 6; i++)
            {
                Vector3 a = hexagon.GetCorner3D(i + 0, placementPlane);
                Vector3 b = hexagon.GetCorner3D(i + 1, placementPlane);
                DrawLine(a, b);

                if (drawTriangles) DrawLine(a, center);
            }
        }
    }

}
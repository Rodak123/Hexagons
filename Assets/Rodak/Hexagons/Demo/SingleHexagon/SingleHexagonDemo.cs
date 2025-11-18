using Rodak.Hexagons.HexDebug;
using Rodak.Hexagons.HexEditor;
using Rodak.Hexagons.HexGeometry3D;
using Rodak.Hexagons.Utils;
using Unity.Collections;
using UnityEngine;

namespace Rodak.Hexagons.Demo.SingleHexagon
{
    public class SingleHexagonDemo : MonoBehaviour
    {
        [SerializeField] private EditableHexagon editableHexagon = Hexagon.Zero;
        [SerializeField, ReadOnly] private EditableHexagon readonlyHexagon = new Hexagon(12, 7);
        [SerializeField] private DemoPlacementPlane placementPlane = DemoPlacementPlane.XY;

        [Header("Visuals")]
        [SerializeField] private Transform center;
        [SerializeField] private Transform[] vertices;
        [SerializeField] private Transform[] sides;

        private void Update()
        {
            PlacementPlane plane = DemoPlacementPlanes.GetPlacementPlane(placementPlane);
            Hexagon hexagon = editableHexagon;

            hexagon.DebugDraw(plane, Color.red, 0, true);

            Vector3 centerPosition = hexagon.GetCenter3D(plane);
            center.position = centerPosition;

            for (int i = 0; i < 6; i++)
            {
                Vector3 vertexPosition = hexagon.GetCorner3D(i, plane);
                vertices[i].position = vertexPosition;
            }

            for (int i = 0; i < 6; i++)
            {
                Vector3 sidePosition = hexagon.GetSide3D(i, plane);

                Vector3 direction = sidePosition - centerPosition;

                sides[i].SetPositionAndRotation(sidePosition, Quaternion.LookRotation(direction));
            }
        }
    }
}
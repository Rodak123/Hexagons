using System.Collections.Generic;
using Rodak.Hexagons.HexDebug;
using Rodak.Hexagons.HexEditor;
using Rodak.Hexagons.HexNavigation;
using UnityEngine;
using Rodak.Hexagons.Utils;

namespace Rodak.Hexagons.Demo.Relations
{
    /// <summary>
    /// This demo shows what relations can you get around a hexagon.
    /// </summary>
    public class HexagonRelationsDemo : MonoBehaviour
    {
        [SerializeField] private EditableHexagon centerHexagon;
        [SerializeField] private DemoPlacementPlane placementPlane;

        [Space]
        [SerializeField] private Gradient neighborGradient;
        [SerializeField] private Gradient diagonalGradient;

        private void Update()
        {
            PlacementPlane plane = DemoPlacementPlanes.GetPlacementPlane(placementPlane);

            Hexagon center = centerHexagon;
            center.DebugDraw(plane, Color.red, 0, true);

            List<Hexagon> neighbors = center.GetNeighbors();
            for (int i = 0; i < neighbors.Count; i++)
            {
                Hexagon neighbor = neighbors[i];
                float t = (float)i / neighbors.Count;
                neighbor.DebugDraw(plane, neighborGradient.Evaluate(t), 0, true);
            }

            List<Hexagon> diagonals = center.GetDiagonals();
            for (int i = 0; i < diagonals.Count; i++)
            {
                Hexagon diagonal = diagonals[i];
                float t = (float)i / diagonals.Count;
                diagonal.DebugDraw(plane, diagonalGradient.Evaluate(t), 0, true);
            }
        }
    }
}

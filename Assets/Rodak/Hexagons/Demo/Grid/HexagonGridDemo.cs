using Rodak.Hexagons.HexDebug;
using Rodak.Hexagons.HexGrid;
using UnityEngine;

namespace Rodak.Hexagons.Demo.Grid
{
    public class HexagonGridDemo : MonoBehaviour
    {
        [SerializeField, Min(1)] private int size = 6;
        [SerializeField] private DemoPlacementPlane placementPlane = DemoPlacementPlane.XY;

        private void Update()
        {
            HexagonGrid<int> hexagonGrid = new(size, CreateHexagonGridValue);
            HexagonGridDebug.DebugDraw(hexagonGrid, DemoPlacementPlanes.GetPlacementPlane(placementPlane), Color.red);
        }

        private int CreateHexagonGridValue(Hexagon position)
        {
            return position.Q + position.R * size;
        }
    }
}

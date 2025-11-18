using Rodak.Hexagons.HexDebug;
using Rodak.Hexagons.HexGrid;
using UnityEngine;
using Rodak.Hexagons.Utils;

namespace Rodak.Hexagons.Demo.Grid
{
    public class HexagonGridDemo : MonoBehaviour
    {
        [SerializeField, Min(1)] private int size = 6;
        [SerializeField] private Vector3 planeUp = Vector3.up;

        private void Update()
        {
            PlacementPlane plane = new(planeUp);

            HexagonGrid<int> hexagonGrid = new(size, CreateHexagonGridValue);
            hexagonGrid.DebugDraw(plane, Color.red);
        }

        private int CreateHexagonGridValue(Hexagon position)
        {
            return position.Q + position.R * size;
        }
    }
}

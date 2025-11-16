using Rodak.Hexagons.HexDebug;
using Rodak.Hexagons.HexEditor;
using Rodak.Hexagons.HexGeometry3D;
using UnityEngine;

namespace Rodak.Hexagons.Demo.H
{
    public class HexagonDemo : MonoBehaviour
    {
        [SerializeField] private EditableHexagon editableHexagon = Hexagon.Zero;

        private void Update()
        {
            Hexagon hexagon = editableHexagon;
            hexagon.DebugDraw(PlacementPlane.XYPlane, Color.red, 0, true);
        }
    }
}
using Rodak.Hexagons.HexEditor;
using Rodak.Hexagons.HexGeometry3D;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rodak.Hexagons.Demo.MapMesh
{
    public class Wanderer : MonoBehaviour
    {
        [SerializeField] private EditableHexagon currentPosition;
        [SerializeField] private EditableHexagon targetPosition;

        [SerializeField] private HexagonMapMeshDemo map;

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            UpdateTargetPosition();
            UpdatePosition();
        }

        private void UpdateTargetPosition()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!map.TryGetTilePositionOn(ray, out Hexagon hexagonPosition))
            {
                return;
            }

            targetPosition = hexagonPosition;
        }

        private void UpdatePosition()
        {
            currentPosition = targetPosition;

            Hexagon hexagonPosition = currentPosition;
            if (!map.TryGetTileAt(hexagonPosition, out MapTile mapTile))
            {
                Debug.Log("Outside of map");
                return;
            }

            Vector3 position = hexagonPosition.GetCenter3D(map.PlacementPlane);
            position.y = mapTile.GetHeight(map.StepHeight);

            transform.position = position;
        }

    }
}
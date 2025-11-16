using UnityEngine;

namespace Rodak.Hexagons.HexGeometry3D
{
    public class PlacementPlane
    {
        public static readonly PlacementPlane XYPlane = new(0, 0, 1);
        public static readonly PlacementPlane XZPlane = new(0, 1, 0);
        public static readonly PlacementPlane YZPlane = new(1, 0, 0);

        public readonly Plane Plane;
        public Vector3 Normal => Plane.normal;

        private readonly Vector3 planeRight;
        private readonly Vector3 planeForward;

        public PlacementPlane(float upX, float upY, float upZ) : this(new Vector3(upX, upY, upZ))
        { }

        public PlacementPlane(Vector3 up) : this(new Plane(up.normalized, Vector3.zero))
        { }

        public PlacementPlane(Plane plane)
        {
            Plane = plane;

            Vector3 arbitraryAxis = (Mathf.Abs(Vector3.Dot(Plane.normal, Vector3.up)) > 0.99f)
                ? Vector3.right : Vector3.up;

            planeRight = Vector3.Cross(Plane.normal, arbitraryAxis).normalized;

            planeForward = Vector3.Cross(planeRight, Plane.normal).normalized;
        }

        public Vector3 LayOnPlane(Vector2 vector)
        {
            return (vector.x * planeRight) + (vector.y * planeForward);
        }
    }
}
using System;
using UnityEngine;

namespace Rodak.Hexagons.HexGeometry
{
    /// <summary>
    /// Extends the hexagon class with many 2D geomtery related functions.
    /// </summary>
    public static class HexagonGeometryExtensions
    {
        /// <summary>
        /// Unit vector hexagons ordered clockwise.
        /// </summary>
        public static readonly Hexagon[] OrderedDirections = new Hexagon[] { -Hexagon.RAxis, -Hexagon.SAxis, Hexagon.QAxis, Hexagon.RAxis, Hexagon.SAxis, -Hexagon.QAxis };

        private static readonly float SQRT_OF_THREE = (float)Math.Sqrt(3);

        /// <summary>
        /// The side width of a hexagon.
        /// </summary>
        public static readonly float SIZE_SIDE_TO_SIDE = SQRT_OF_THREE;
        /// <summary>
        /// The vertex width of a hexagon.
        /// </summary>
        public static readonly float SIZE_CORNER_TO_CORNER = 2;

        /// <summary>
        /// Calculates a hexagon's center on a 2D plane.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <returns>Center position</returns>
        public static Vector2 GetCenter(this Hexagon hexagon)
        {
            return new Vector2(
                SQRT_OF_THREE * hexagon.Q + SQRT_OF_THREE / 2f * hexagon.R,
                3f / 2f * hexagon.R
            );
        }

        /// <summary>
        /// Calculates a hexagon from a 2D point.
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Hexagon containing this point</returns>
        public static Hexagon GetHexagonAt(float x, float y)
        {
            float qFloat = SQRT_OF_THREE / 3f * x - 1f / 3f * y;
            float rFloat = 2f / 3f * y;
            return Hexagon.GetNearestHexagonRound(qFloat, rFloat);
        }

        /// <summary>
        /// Contains the vertex index from 0 to 6. 
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Vertex index</returns>
        public static int GetAbsoluteIndex(int index)
        {
            return ((index % 6) + 6) % 6;
        }

        /// <summary>
        /// Wheter the distance between these directions is of the offset.
        /// </summary>
        /// <param name="indexA">Index A</param>
        /// <param name="indexB">Index B</param>
        /// <param name="offset">Offset</param>
        /// <returns>True if indexA + offset = indexB</returns>
        public static bool AreDirectionsOffseted(int indexA, int indexB, int offset)
        {
            int offsetIndex = GetAbsoluteIndex(indexA + offset);
            indexB = GetAbsoluteIndex(indexB);
            return offsetIndex == indexB;
        }

        /// <summary>
        /// Calculates the angle of a hexagon corner.
        /// </summary>
        /// <param name="index">Vertex index</param>
        /// <returns>Angle in radians</returns>
        public static float GetCornerAngle(int index)
        {
            index = GetAbsoluteIndex(index);

            float angleDegrees = 60 * index - 30;
            return Mathf.Deg2Rad * angleDegrees;
        }

        /// <summary>
        /// Calculates the direction of the corner form the center.
        /// </summary>
        /// <param name="index">Vertex index</param>
        /// <returns>Angle in radians</returns>
        public static Vector2 GetCornerDirection(int index)
        {
            float angle = GetCornerAngle(index);

            return new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            );
        }

        /// <summary>
        /// Calculates a hexagon's corner on a 2D plane.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <param name="index">Vertex index</param>
        /// <returns>Corner position</returns>
        public static Vector2 GetCorner(this Hexagon hexagon, int index)
        {
            Vector2 center = hexagon.GetCenter();
            Vector2 cornerDirection = GetCornerDirection(index);
            return center + cornerDirection;
        }

        /// <summary>
        /// Calculates a hexagon's side direction on a 2D plane.
        /// </summary>
        /// <param name="index">Vertex index</param>
        /// <returns>Direction vector</returns>
        public static Vector2 GetSideDirection(int index)
        {
            float angle = GetCornerAngle(index);

            return new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            );
        }
        /// <summary>
        /// Calculates a hexagon's side on a 2D plane.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <param name="index">Vertex index</param>
        /// <returns>Side center position</returns>
        public static Vector2 GetSide(this Hexagon hexagon, int index)
        {
            index = GetAbsoluteIndex(index);

            Vector2 center = hexagon.GetCenter();
            Hexagon direction = OrderedDirections[index];
            return center + direction.GetCenter() / 2f;
        }

        /// <summary>
        /// Calculates the direction angle of the hexagon from the (0, 0) center.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <returns>Angle in degrees</returns>
        public static float GetDirectionAngle(this Hexagon hexagon)
        {
            Vector2 directionVector = hexagon.GetCenter().normalized;
            return 360f - (Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg + 360f) % 360f;
        }

        /// <summary>
        /// Calculates a clockwise rotated hexagon.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <returns>Clockwise rotated hexagon</returns>
        public static Hexagon RotateClockwise(this Hexagon hexagon) => new(-hexagon.R, -hexagon.S, -hexagon.Q);

        /// <summary>
        /// Calculates a clockwise rotated hexagon N times.
        /// </summary>
        /// <param name="hexagon">Hexagon</param>
        /// <param name="rotations">Number of rotation</param>
        /// <returns>N times clockwise rotated hexagon</returns>
        public static Hexagon RotateClockwise(this Hexagon hexagon, int rotations)
        {
            rotations %= 6;
            for (int n = 0; n < rotations; n++)
                hexagon = hexagon.RotateClockwise();
            return hexagon;
        }

    }
}
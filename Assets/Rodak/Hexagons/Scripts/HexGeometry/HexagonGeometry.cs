using System;
using UnityEngine;

namespace Rodak.Hexagons.HexGeometry
{
    public static class HexagonGeometry
    {
        public static readonly Hexagon[] OrderedDirections = new Hexagon[] { -Hexagon.RAxis, -Hexagon.SAxis, Hexagon.QAxis, Hexagon.RAxis, Hexagon.SAxis, -Hexagon.QAxis };

        private static readonly float SQRT_OF_THREE = (float)Math.Sqrt(3);

        public static readonly float SIZE_SIDE_TO_SIDE = SQRT_OF_THREE;
        public static readonly float SIZE_CORNER_TO_CORNER = 2;

        public static Vector2 GetCenter(this Hexagon hexagon)
        {
            return new Vector2(
                SQRT_OF_THREE * hexagon.Q + SQRT_OF_THREE / 2f * hexagon.R,
                3f / 2f * hexagon.R
            );
        }

        public static Hexagon GetHexagonAt(float x, float y)
        {
            float qFloat = SQRT_OF_THREE / 3f * x - 1f / 3f * y;
            float rFloat = 2f / 3f * y;
            return Hexagon.GetNearestHexagonRound(qFloat, rFloat);
        }

        public static int GetAbsoluteIndex(int index)
        {
            return ((index % 6) + 6) % 6;
        }

        public static bool AreDirectionsOffseted(int indexA, int indexB, int offset)
        {
            int offsetIndex = GetAbsoluteIndex(indexA + offset);
            indexB = GetAbsoluteIndex(indexB);
            return offsetIndex == indexB;
        }

        public static float GetCornerAngle(int index)
        {
            index = GetAbsoluteIndex(index);

            float angleDegrees = 60 * index - 30;
            return Mathf.Deg2Rad * angleDegrees;
        }

        public static Vector2 GetCornerDirection(int index)
        {
            float angle = GetCornerAngle(index);

            return new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            );
        }

        public static Vector2 GetCorner(this Hexagon hexagon, int index)
        {
            Vector2 center = hexagon.GetCenter();
            Vector2 cornerDirection = GetCornerDirection(index);
            return center + cornerDirection;
        }

        public static Vector2 GetSideDirection(int index)
        {
            float angle = GetCornerAngle(index);

            return new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            );
        }

        public static Vector3 GetSide(this Hexagon hexagon, int index)
        {
            index = GetAbsoluteIndex(index);

            Vector2 center = hexagon.GetCenter();
            Hexagon direction = OrderedDirections[index];
            return center + direction.GetCenter() / 2f;
        }

        /// <summary>
        /// Angle is in degrees
        /// </summary>
        public static float GetDirectionAngle(this Hexagon hexagon)
        {
            Vector2 directionVector = hexagon.GetCenter().normalized;
            return 360f - (Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg + 360f) % 360f;
        }

        public static Hexagon RotateClockwise(this Hexagon hexagon) => new(-hexagon.R, -hexagon.S, -hexagon.Q);
        public static Hexagon RotateClockwise(this Hexagon hexagon, int rotations)
        {
            rotations %= 6;
            for (int n = 0; n < rotations; n++)
                hexagon = hexagon.RotateClockwise();
            return hexagon;
        }

    }
}
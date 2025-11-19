using System;
using UnityEngine;

namespace Rodak.Hexagons
{
    /// <summary>
    /// Immutable definition of a 2D hexagon made out of 3 components Q, R and S.
    /// Defined: Q + R + S = 0
    /// </summary>
    public record Hexagon
    {
        /// <summary>
        /// The origin hexagon (0, 0, 0).
        /// </summary>
        public static Hexagon Zero => new(0, 0, 0);

        /// <summary>
        /// The direction where Q is unchanged.
        /// </summary>
        public static Hexagon QZero => new(0, -1, 1);

        /// <summary>
        /// The direction where R is unchanged.
        /// </summary>
        public static Hexagon RZero => new(-1, 0, 1);

        /// <summary>
        /// The direction where S is unchanged.
        /// </summary>
        public static Hexagon SZero => new(-1, 1, 0);

        /// <summary>
        /// Returns the closest hexagon based on rounded and precise values.
        /// This method corrects rounding errors to ensure Q + R + S = 0.
        /// </summary>
        /// <returns>The nearest valid Hexagon.</returns>
        public static Hexagon GetNearestHexagon(int q, int r, int s, float qFloat, float rFloat, float sFloat)
        {
            float qDiff = Math.Abs(q - qFloat);
            float rDiff = Math.Abs(r - rFloat);
            float sDiff = Math.Abs(s - sFloat);

            if (qDiff > rDiff && qDiff > sDiff)
                q = -r - s;
            else if (rDiff > sDiff)
                r = -q - s;
            else
                s = -q - r;

            return new(q, r, s);
        }

        /// <summary>
        /// Returns the rounded hexagon based on precise values.
        /// The components are rounded first, then corrected to ensure Q + R + S = 0.
        /// </summary>
        /// <returns>Rounded hexagon</returns>
        public static Hexagon GetNearestHexagonRound(float qFloat, float rFloat, float sFloat)
        {
            int q = (int)Math.Round(qFloat);
            int r = (int)Math.Round(rFloat);
            int s = (int)Math.Round(sFloat);
            return GetNearestHexagon(q, r, s, qFloat, rFloat, sFloat);
        }

        /// <summary>
        /// Returns the rounded hexagon based on precise Q and R values, calculating S.
        /// </summary>
        /// <returns>Rounded hexagon</returns>
        public static Hexagon GetNearestHexagonRound(float qFloat, float rFloat) => GetNearestHexagonRound(qFloat, rFloat, -(qFloat + rFloat));


        /// <summary>
        /// Returns the floored hexagon based on precise values.
        /// The components are floored first, then corrected to ensure Q + R + S = 0.
        /// </summary>
        /// <returns>Floored hexagon</returns>
        public static Hexagon GetNearestHexagonFloor(float qFloat, float rFloat, float sFloat)
        {
            int q = (int)Math.Floor(qFloat);
            int r = (int)Math.Floor(rFloat);
            int s = (int)Math.Floor(sFloat);
            return GetNearestHexagon(q, r, s, qFloat, rFloat, sFloat);
        }

        /// <summary>
        /// Returns the floored hexagon based on precise Q and R values, calculating S.
        /// </summary>
        /// <returns>Floored hexagon</returns>
        public static Hexagon GetNearestHexagonFloor(float qFloat, float rFloat) => GetNearestHexagonFloor(qFloat, rFloat, -(qFloat + rFloat));

        /// <summary>
        /// Calculates the distance between two hexagons.
        /// </summary>
        /// <returns>Distance</returns>
        public static float Distance(Hexagon a, Hexagon b)
        {
            Hexagon diff = a - b;
            return (Math.Abs(diff.Q) + Math.Abs(diff.R) + Math.Abs(diff.S)) / 2f;
        }

        /// <summary>
        /// Linearly interpolates between two hexagons.
        /// </summary>
        /// <returns>Interpolated hexagon.</returns>
        public static Hexagon Lerp(Hexagon start, Hexagon end, float t)
        {
            t = Mathf.Clamp01(t);

            float qFloat = Mathf.Lerp(start.Q, end.Q, t);
            float rFloat = Mathf.Lerp(start.R, end.R, t);
            float sFloat = Mathf.Lerp(start.S, end.S, t);

            return GetNearestHexagonRound(qFloat, rFloat, sFloat);
        }

        /// <summary>
        /// Checks whether the values represent a valid hexagon.
        /// Q + R + S = 0
        /// </summary>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValid(int q, int r, int s)
        {
            return (q + r + s) == 0;
        }

        /// <summary>Rounds the scalar multiplication of a Hexagon to the nearest integer coordinates.</summary>
        public static Hexagon MultRound(Hexagon a, float scalar) => GetNearestHexagonRound(a.Q * scalar, a.R * scalar, a.S * scalar);
        /// <summary>Floors the scalar multiplication of a Hexagon to the nearest integer coordinates.</summary>
        public static Hexagon MultFloor(Hexagon a, float scalar) => GetNearestHexagonFloor(a.Q * scalar, a.R * scalar, a.S * scalar);

        /// <summary>Rounds the scalar division of a Hexagon to the nearest integer coordinates.</summary>
        public static Hexagon DivRound(Hexagon a, float scalar) => GetNearestHexagonRound(a.Q / scalar, a.R / scalar, a.S / scalar);
        /// <summary>Floors the scalar division of a Hexagon to the nearest integer coordinates.</summary>
        public static Hexagon DivFloor(Hexagon a, float scalar) => GetNearestHexagonFloor(a.Q / scalar, a.R / scalar, a.S / scalar);

        /// <summary>Returns the Hexagon instance without modification.</summary>
        public static Hexagon operator +(Hexagon a) => a;
        /// <summary>Negative version of the hexagon.</summary>
        public static Hexagon operator -(Hexagon a) => new(-a.Q, -a.R, -a.S);

        /// <summary>Adds two Hexagon coordinates (vector addition).</summary>
        public static Hexagon operator +(Hexagon a, Hexagon b) => new(a.Q + b.Q, a.R + b.R, a.S + b.S);
        /// <summary>Subtracts one Hexagon coordinate from another (vector subtraction).</summary>
        public static Hexagon operator -(Hexagon a, Hexagon b) => new(a.Q - b.Q, a.R - b.R, a.S - b.S);

        /// <summary>Multiplies a Hexagon by a scalar and rounds the result to the nearest valid Hexagon.</summary>
        public static Hexagon operator *(Hexagon a, float scalar) => MultRound(a, scalar);
        /// <summary>Divides a Hexagon by a scalar and rounds the result to the nearest valid Hexagon.</summary>
        public static Hexagon operator /(Hexagon a, float scalar) => DivRound(a, scalar);

        /// <summary>
        /// Q axis position.
        /// </summary>
        public int Q { get; private set; }
        /// <summary>
        /// R axis position.
        /// </summary>
        public int R { get; private set; }
        /// <summary>
        /// S axis position.
        /// </summary>
        public int S { get; private set; }

        /// <summary>
        /// New hexagon, throws an <see cref="ArgumentException"/> when Q + R + S != 0. 
        /// </summary>
        /// <exception cref="ArgumentException">Q + R + S != 0</exception>
        public Hexagon(int q, int r, int s)
        {
            if (!IsValid(q, r, s))
                throw new ArgumentException($"Invalid hexagon, {nameof(q)} + {nameof(r)} + {nameof(s)} must be equal to {0}");
            Q = q;
            R = r;
            S = s;
        }

        /// <summary>
        /// New hexagon with S = -(Q + R).
        /// </summary>
        public Hexagon(int q, int r) : this(q, r, -(q + r)) { }

        /// <summary>
        /// Calculates a hash code for the Hexagon based on its Q, R, and S values.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Q, R, S);
        }

        public override string ToString()
        {
            return $"Hex[{Q}, {R}, {S}]";
        }
    }
}
using System;

namespace Rodak.Hexagons
{
    public record Hexagon
    {
        public static Hexagon Zero => new(0, 0, 0);

        public static Hexagon QAxis => new(-1, 1, 0);
        public static Hexagon RAxis => new(-1, 0, 1);
        public static Hexagon SAxis => new(0, -1, 1);

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

        public static Hexagon GetNearestHexagonRound(float qFloat, float rFloat, float sFloat)
        {
            int q = (int)Math.Round(qFloat);
            int r = (int)Math.Round(rFloat);
            int s = (int)Math.Round(sFloat);
            return GetNearestHexagon(q, r, s, qFloat, rFloat, sFloat);
        }
        public static Hexagon GetNearestHexagonRound(float qFloat, float rFloat) => GetNearestHexagonRound(qFloat, rFloat, -(qFloat + rFloat));

        public static Hexagon GetNearestHexagonFloor(float qFloat, float rFloat, float sFloat)
        {
            int q = (int)Math.Floor(qFloat);
            int r = (int)Math.Floor(rFloat);
            int s = (int)Math.Floor(sFloat);
            return GetNearestHexagon(q, r, s, qFloat, rFloat, sFloat);
        }
        public static Hexagon GetNearestHexagonFloor(float qFloat, float rFloat) => GetNearestHexagonFloor(qFloat, rFloat, -(qFloat + rFloat));

        public static float Distance(Hexagon a, Hexagon b)
        {
            Hexagon diff = a - b;
            return (Math.Abs(diff.Q) + Math.Abs(diff.R) + Math.Abs(diff.S)) / 2f;
        }

        public static Hexagon MultRound(Hexagon a, float scalar) => GetNearestHexagonRound(a.Q * scalar, a.R * scalar, a.S * scalar);
        public static Hexagon MultFloor(Hexagon a, float scalar) => GetNearestHexagonFloor(a.Q * scalar, a.R * scalar, a.S * scalar);

        public static Hexagon DivRound(Hexagon a, float scalar) => GetNearestHexagonRound(a.Q / scalar, a.R / scalar, a.S / scalar);
        public static Hexagon DivFloor(Hexagon a, float scalar) => GetNearestHexagonFloor(a.Q / scalar, a.R / scalar, a.S / scalar);

        public static Hexagon operator +(Hexagon a) => a;
        public static Hexagon operator -(Hexagon a) => new(-a.Q, -a.R, -a.S);

        public static Hexagon operator +(Hexagon a, Hexagon b) => new(a.Q + b.Q, a.R + b.R, a.S + b.S);
        public static Hexagon operator -(Hexagon a, Hexagon b) => new(a.Q - b.Q, a.R - b.R, a.S - b.S);

        public static Hexagon operator *(Hexagon a, float scalar) => MultRound(a, scalar);
        public static Hexagon operator /(Hexagon a, float scalar) => DivRound(a, scalar);

        public int Q { get; private set; }
        public int R { get; private set; }
        public int S { get; private set; }

        public Hexagon(int q, int r, int s)
        {
            if (q + r + s != 0) throw new ArgumentException($"Invalid hexagon, {nameof(q)} + {nameof(r)} + {nameof(s)} must be equal to {0}");
            Q = q;
            R = r;
            S = s;
        }

        public Hexagon(int q, int r) : this(q, r, -(q + r)) { }

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
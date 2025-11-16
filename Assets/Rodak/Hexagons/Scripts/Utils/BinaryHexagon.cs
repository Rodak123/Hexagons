using System.IO;

namespace Rodak.Hexagons.HexUtils
{
    public static class BinaryHexagon
    {
        public static void Write(this BinaryWriter bw, Hexagon hexagon)
        {
            bw.Write(hexagon.Q);
            bw.Write(hexagon.R);
        }

        public static Hexagon ReadHexagon(this BinaryReader br)
        {
            int q = br.ReadInt32();
            int r = br.ReadInt32();

            return new(q, r);
        }
    }
}
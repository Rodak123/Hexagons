using System.IO;

namespace Rodak.Hexagons.HexUtils
{
    /// <summary>
    /// Utility class that allow Hexagon to be saved into binary.
    /// </summary>
    public static class BinaryHexagon
    {
        /// <summary>
        /// Write the hexagon to the binary writer.
        /// </summary>
        /// <param name="bw">Binary writer</param>
        /// <param name="hexagon">The hexagon</param>
        public static void Write(this BinaryWriter bw, Hexagon hexagon)
        {
            bw.Write(hexagon.Q);
            bw.Write(hexagon.R);
        }

        /// <summary>
        /// Read the hexagon from the binary reader.
        /// </summary>
        /// <param name="br">Binary reader</param>
        /// <returns>The read Hexagon object.</returns>
        public static Hexagon ReadHexagon(this BinaryReader br)
        {
            int q = br.ReadInt32();
            int r = br.ReadInt32();

            return new(q, r);
        }
    }
}
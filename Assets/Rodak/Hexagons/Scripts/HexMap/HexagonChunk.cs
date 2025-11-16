using System;
using System.Collections.Generic;
using System.Linq;

namespace Rodak.Hexagons.HexMap
{
    public static class HexagonChunk
    {
        public static int GetSizeAcross(int chunkSize) => chunkSize * 2 + 1;
    }

    public class HexagonChunk<TTile>
    {
        private readonly Dictionary<Hexagon, TTile> values = new();
        public List<Hexagon> Hexagons => values.Keys.ToList();

        public readonly int Size;

        public readonly Hexagon Position;

        public HexagonChunk(Hexagon position, int size, Func<Hexagon, Hexagon, TTile> createValue)
        {
            if (size < 0) throw new ArgumentException($"{nameof(size)} of a {nameof(HexagonChunk<TTile>)} must be positive");
            Size = size;
            Position = position;

            int sizeAcross = HexagonChunk.GetSizeAcross(size);
            Hexagon chunkCenter = position * sizeAcross;
            for (int i = -Size; i <= Size; i++)
            {
                for (int j = -Size; j <= Size; j++)
                {
                    Hexagon hexagonPosition = chunkCenter + new Hexagon(i, j);
                    values.Add(hexagonPosition, createValue(hexagonPosition, position));
                }
            }
        }

        public void ForEach(Action<Hexagon, TTile> action)
        {
            Hexagons.ForEach((Hexagon position) =>
            {
                action(position, values[position]);
            });
        }

        public bool ContainsHexagon(Hexagon position)
        {
            return values.ContainsKey(position);
        }

        public bool TryGetValue(Hexagon position, out TTile value)
        {
            return values.TryGetValue(position, out value);
        }

        public bool SetValue(Hexagon position, TTile value)
        {
            if (!values.ContainsKey(position))
                return false;
            values[position] = value;
            return true;
        }

        public override string ToString()
        {
            return $"HexChunk[{Position}, {Size}]";
        }
    }
}
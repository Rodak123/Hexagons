
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rodak.Hexagons.HexGrid
{
    public class HexagonGrid<T>
    {
        private readonly Dictionary<Hexagon, T> values = new();
        public List<Hexagon> Hexagons => values.Keys.ToList();

        public int Size { get; private set; }
        public int SizeAcross => Size * 2 + 1;

        public HexagonGrid(int size, Func<Hexagon, T> createValue)
        {
            if (size < 0) throw new ArgumentException($"{nameof(size)} of a {nameof(HexagonGrid<T>)} must be positive");
            Size = size;

            for (int i = -Size; i <= Size; i++)
            {
                for (int j = -Size; j <= Size; j++)
                {
                    if (i + j > Size || i + j < -Size) continue;
                    Hexagon position = new(i, j);
                    values.Add(position, createValue(position));
                }
            }
        }

        public void ForEach(Action<Hexagon, T> action)
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

        public bool TryGetValue(Hexagon position, out T value)
        {
            return values.TryGetValue(position, out value);
        }

        public bool SetValue(Hexagon position, T value)
        {
            if (!values.ContainsKey(position))
                return false;
            values[position] = value;
            return true;
        }

        public override string ToString()
        {
            return $"HexGrid[{Size}, {typeof(T)}]";
        }
    }
}
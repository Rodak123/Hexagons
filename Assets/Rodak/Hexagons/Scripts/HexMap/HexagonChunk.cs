using System;
using System.Collections.Generic;
using System.Linq;

namespace Rodak.Hexagons.HexMap
{
    /**
     * Provides static utility functions for calculating properties of hexagon chunks.
     */
    public static class HexagonChunkExtensions
    {
        /// <summary>
        /// Calculates the total number of hexagons across the width or height of the chunk.
        /// </summary>
        /// <returns>The total size (width/height) of the chunk's bounding box.</returns>
        public static int GetSizeAcross(int chunkSize) => chunkSize * 2 + 1;
    }

    /// <summary>
    /// Represents a fixed-size square group of hexagon tiles for a larger map.
    /// </summary>
    /// <typeparam name="TTile">The type of value stored in each hexagon tile.</typeparam>
    public class HexagonChunk<TTile>
    {
        private readonly Dictionary<Hexagon, TTile> values = new();

        /// <summary>
        /// Gets a list of the Hexagon coordinates for all tiles contained in this chunk.
        /// </summary>
        public List<Hexagon> Hexagons => values.Keys.ToList();

        /// <summary>
        /// The size parameter used to define the chunk's boundaries. The actual size across is (2*Size + 1).
        /// </summary>
        public readonly int Size;

        /// <summary>
        /// The Hexagon coordinate of the chunk's center in the map.
        /// </summary>
        public readonly Hexagon Position;

        /// <summary>
        /// Initializes a new instance of the HexagonChunk class.
        /// </summary>
        public HexagonChunk(Hexagon position, int size, Func<Hexagon, Hexagon, TTile> createValue)
        {
            if (size < 0) throw new ArgumentException($"{nameof(size)} of a {nameof(HexagonChunk<TTile>)} must be positive");
            Size = size;
            Position = position;

            int sizeAcross = HexagonChunkExtensions.GetSizeAcross(size);
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

        /// <summary>
        /// Executes an action for every hexagon tile and its value in the chunk.
        /// </summary>
        public void ForEach(Action<Hexagon, TTile> action)
        {
            Hexagons.ForEach((Hexagon position) =>
            {
                action(position, values[position]);
            });
        }

        /// <summary>
        /// Checks if the chunk contains the tile at the given Hexagon coordinate.
        /// </summary>
        /// <returns>True if the hexagon is in the chunk, false otherwise.</returns>
        public bool ContainsHexagon(Hexagon position)
        {
            return values.ContainsKey(position);
        }

        /// <summary>
        /// Attempts to retrieve the value of a hexagon tile in the chunk.
        /// </summary>
        /// <returns>True if the value was successfully retrieved, false otherwise.</returns>
        public bool TryGetValue(Hexagon position, out TTile value)
        {
            return values.TryGetValue(position, out value);
        }

        /// <summary>
        /// Sets the value of a hexagon tile in the chunk.
        /// </summary>
        /// <returns>True if the value was successfully set, false if the hexagon is not in the chunk.</returns>
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
using System;

namespace Rodak.Hexagons.HexMap
{
    /// <summary>
    /// Represents a map of hexagon tiles that is automatically organized into chunks.
    /// It inherits basic map functionality from AHexagonMap.
    /// </summary>
    /// <typeparam name="T">The type of value stored in each hexagon tile.</typeparam>
    public class HexagonMap<T> : AHexagonMap<T, HexagonChunk<T>>
    {
        private Func<Hexagon, Hexagon, T> createValue;

        /// <summary>
        /// Initializes a new instance of the HexagonMap class.
        /// </summary>
        /// <param name="chunkSize">The size parameter for the chunks used in the map.</param>
        /// <param name="createValue">A function used to generate the value for each hexagon tile.</param>
        public HexagonMap(
            int chunkSize,
            Func<Hexagon, Hexagon, T> createValue
            ) : base(chunkSize)
        {
            this.createValue = createValue;
        }

        /// <summary>
        /// Creates a new HexagonChunk instance at a specified position using the stored creation function.
        /// </summary>
        /// <param name="chunkPosition">The Hexagon coordinate of the chunk's center.</param>
        /// <returns>A new HexagonChunk object.</returns>
        protected override HexagonChunk<T> CreateChunk(Hexagon chunkPosition)
        {
            return new(chunkPosition, ChunkSize, createValue);
        }
    }
}